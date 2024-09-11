using CitizenPortalSouthAfrica.ViewModels;
using System.Windows;

namespace CitizenPortalSouthAfrica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }
    }
}
