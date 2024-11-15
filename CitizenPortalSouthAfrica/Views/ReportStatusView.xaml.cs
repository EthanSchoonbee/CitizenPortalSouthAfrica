using CitizenPortalSouthAfrica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CitizenPortalSouthAfrica.Views
{
    /// <summary>
    /// Interaction logic for ReportStatusView.xaml
    /// </summary>
    public partial class ReportStatusView : UserControl
    {
        public ReportStatusView()
        {
            InitializeComponent();
        }

        private void OnReportClicked(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var report = border?.DataContext as ReportIssue;
            if (report != null)
            {
                // Show detailed view (e.g., open a dialog or navigate to a new page)
                MessageBox.Show($"Showing details for: {report.Id}");
            }
        }
    }
}
