using GUI.ViewModels;
using System.Windows;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            MainWindowViewModel vm = new MainWindowViewModel(); 
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}
