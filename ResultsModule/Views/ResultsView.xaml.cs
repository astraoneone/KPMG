using ResultsModule.ViewModels;
using System.Windows.Controls;

namespace ResultsModule.Views
{
    /// <summary>
    /// Interaction logic for ResultsView.xaml
    /// </summary>
    public partial class ResultsView : UserControl
    {
        private ResultsViewModel vm;

        public ResultsView(ResultsViewModel vm)
        {
            this.vm = vm;
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}
