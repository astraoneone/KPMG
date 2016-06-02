namespace ResultsModule.ViewModels
{
    using GuiShared.Context;
    using GuiShared.DomainClasses;
    using GuiShared.PubSubEvents;
    using GuiShared.PubSubPayloads;
    using Prism.Events;
    using Prism.Mvvm;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class ResultsViewModel : BindableBase
    {
        private IEventAggregator eventAggregator;
        private string infoText;
        private string selectedFile;
        private int progBarValue;
        private string progBarText;
        private bool progBarActive;

        public ResultsViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.InfoText = string.Empty;
            this.SelectedFile = string.Empty;
            this.eventAggregator.GetEvent<ButtonOneEvent>().Subscribe(this.ButtonOneEventDetected);
        }

        public string InfoText
        {
            get
            {
                return this.infoText;
            }

            set
            {
                this.SetProperty(ref this.infoText, value);
            }
        }

        public string SelectedFile
        {
            get
            {
                return this.selectedFile;
            }

            set
            {
                this.SetProperty(ref this.selectedFile, value);
            }
        }

        public int ProgBarValue
        {
            get
            {
                return this.progBarValue;
            }

            set
            {
                this.SetProperty(ref this.progBarValue, value);
            }
        }

        public string ProgBarText
        {
            get
            {
                return this.progBarText;
            }

            set
            {
                this.SetProperty(ref this.progBarText, value);
            }
        }
        public bool ProgBarActive
        {
            get
            {
                return this.progBarActive;
            }

            set
            {
                this.SetProperty(ref this.progBarActive, value);
            }
        }

        public async void ButtonOneEventDetected(ButtonOneInfo info)
        {
            // progress and progress2 will be run on the UI thread.
            var fileImportProgress = new Progress<int>(percent =>
            {
                ProgBarValue = percent;
                ProgBarText = "Importing - " + percent.ToString() + "%";
            });

            var databaseUploadProgress = new Progress<int>(percent =>
            {
                ProgBarValue = percent;
                ProgBarText = "Database Upload - " + percent.ToString() + "%";
            });

            // parseCSV is run on the thread pool.
            try
            {
                this.ProgBarActive = true;
                this.InfoText = "Fetching latest Currency Codes.";
                this.SelectedFile = Path.GetFullPath(info.FilePath);
                await Task.Run(() => ImportAndParseCsvFile(fileImportProgress, databaseUploadProgress, info.FilePath));
                this.ProgBarActive = false;
            }
            catch(Exception e)
            {
                this.InfoText = "Error importing file (" + e.Message + ")";
            }
        }

        public void ImportAndParseCsvFile(IProgress<int> fileImportProgress, IProgress<int> databaseUploadProgress, string path)
        {         
            List<TaxReturn> parsedData = new List<TaxReturn>();
            List<CurrencyCode> curCodes = new List<CurrencyCode>();
            int totalRecords = 0;
            int validRecords = 0;
            int currentRecord = 0;

            try
            {
                bool validRow;

                using (var db = new SampleDbContext())
                {
                    // Colect an in-memory list of valid currency 
                    // codes - this is quicker for validation (below)  
                    // than interrogating the database with each loop.
                    foreach (var curCode in db.CurrencyCodes)
                    {
                        curCodes.Add(curCode);
                    }

                    this.InfoText = "Importing File";

                    // Determine the total number of records
                    totalRecords = CountCsvRows(path);
                    
                    // Parse the data held in the file
                    using (StreamReader readFile = new StreamReader(path))
                    {
                        string csvLine;
                        string[] csvLineContents;
                        decimal taxAmount;
                        int progressBarIncrements = totalRecords/100;

                        while ((csvLine = readFile.ReadLine()) != null)
                        {
                            validRow = true;
                            currentRecord++;
                            csvLineContents = csvLine.Split(',');
                            
                            // Validate account number (assuming lenth must be 8 characters)
                            if (csvLineContents[0] == null || csvLineContents[0].Length != 8)
                            {
                                validRow = false;
                            }

                            // Validate description
                            if (csvLineContents[1] == null || csvLineContents[1].Length < 1)
                            {
                                validRow = false;
                            }

                            // Validate tax code
                            String codeToExamine = csvLineContents[2];
                            if (!curCodes.Any(rowx => codeToExamine == rowx.Code))
                            {
                                validRow = false;
                            }

                            // Validate tax amount
                            if (!decimal.TryParse(csvLineContents[3], out taxAmount))
                            {
                                validRow = false;
                            }

                            // If all of the data in this row is validated, add it to the list
                            if (validRow)
                            {
                                validRecords++;

                                var ret = new TaxReturn()
                                {
                                    ImportedOn = DateTime.UtcNow,
                                    Account = csvLineContents[0],
                                    Description = csvLineContents[1],
                                    CurrencyCode = csvLineContents[2],
                                    TaxValue = taxAmount
                                };

                                parsedData.Add(ret);
                                db.TaxReturns.Add(ret);
                            }

                            // Update the progress bar
                            if (fileImportProgress != null)
                            {
                                    
                                fileImportProgress.Report((int)(((double)100 / (double)totalRecords) * currentRecord));

                            }
                        }

                        db.SaveChanges();
                        this.InfoText = "Parsed " + totalRecords.ToString() + " record(s) - " + validRecords + " imported, " + (totalRecords - validRecords).ToString() + " errors.";
                    }
                }               
            }
            catch (Exception e)
            {
                this.InfoText = "File was not imported!";
                Debug.WriteLine(e.Message);
            }
        }

        public int CountCsvRows(string fileToCheck)
        {
            int recordCount = 0;

            using (StreamReader readFile = new StreamReader(fileToCheck))
            {
                string line;
                while ((line = readFile.ReadLine()) != null)
                {
                    recordCount++;
                }
            }

            return recordCount;
        }
    }
}
