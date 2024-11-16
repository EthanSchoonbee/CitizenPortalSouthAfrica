using CitizenPortalSouthAfrica.Models;
using CitizenPortalSouthAfrica.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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

            this.DataContext = new ReportStatusViewModel();

            // Subscribe to the Loaded event
            this.Loaded += ReportStatusView_Loaded;
        }

        private void ReportStatusView_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as ReportStatusViewModel;
            viewModel?.RefreshReports(); // Refresh reports when the view is loaded
        }

        private void OnReportClicked(object sender, MouseButtonEventArgs e)
        {
            var report = (sender as FrameworkElement).DataContext as Report;

            if (report != null)
            {
                // Access the view model
                var viewModel = this.DataContext as ReportStatusViewModel;

                if (viewModel != null)
                {
                    // Collapse all reports except the clicked one
                    foreach (var r in viewModel.Reports)
                    {
                        if (r != report)
                        {
                            r.IsExpanded = false;
                        }
                    }

                    // Check if the clicked report is being collapsed
                    if (report.IsExpanded)
                    {
                        // Clear related reports if collapsing
                        viewModel.ClearRelatedReports();
                    }
                    else
                    {
                        // Fetch related reports if expanding
                        viewModel.FetchRelatedReports(report);
                    }

                    // Toggle the clicked report
                    report.IsExpanded = !report.IsExpanded;
                }
            }
        }
    }
}
