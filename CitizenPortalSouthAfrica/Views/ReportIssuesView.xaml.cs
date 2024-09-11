using CitizenPortalSouthAfrica.ViewModels;
using System.Windows.Controls;

namespace CitizenPortalSouthAfrica.Views
{
    /// <summary>
    /// Interaction logic for ReportIssuesView.xaml
    /// </summary>
    public partial class ReportIssuesView : UserControl
    {
        public ReportIssuesView()
        {
            InitializeComponent();
            DataContext = new ReportIssuesViewModel();
        }
    }
}
