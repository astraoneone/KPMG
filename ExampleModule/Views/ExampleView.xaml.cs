namespace ExampleModule.Views
{
    using ExampleModule.ViewModels;
    using Microsoft.Win32;
    using System.Windows.Controls;

    public partial class ExampleView : UserControl
    {
        private ExampleViewModel vm;

        public ExampleView(ExampleViewModel vm)
        {
            this.vm = vm;
            InitializeComponent();
            this.DataContext = vm;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string ourFile = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                ourFile = openFileDialog.FileName;
            }

            if(!string.IsNullOrEmpty(ourFile))
            {
                this.vm.ButtonOnePressCommand.Execute(ourFile);
            }          
        }
    }
}
