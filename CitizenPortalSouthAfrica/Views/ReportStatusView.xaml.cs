//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     11/10/2024
 * Last Modified:    16/11/2024
 * 
 * Description:
 * This class defines the interaction logic for the ReportStatusView in the CitizenPortalSouthAfrica application.
 * The view displays a list of reports and allows users to interact with each report to expand or collapse details.
 * It supports fetching and clearing related reports based on user interaction and maintains the expanded/collapsed state for each report.
 * 
 * Dependencies:
 * - ReportStatusViewModel: ViewModel for managing the report data.
 * - Report: Model representing a report.
 * 
 * Methods:
 * - ReportStatusView(): Initializes the view and subscribes to the Loaded event.
 * - ReportStatusView_Loaded(): Refreshes reports when the view is loaded.
 * - OnReportClicked(): Handles the logic for expanding/collapsing a report and fetching related reports.
 * 
 * Implementation Details:
 * - Uses DataContext to bind the view to the ReportStatusViewModel.
 * - Subscribes to the Loaded event to refresh the report list upon view load.
 * - Implements report expansion/collapse logic and interacts with the ViewModel to fetch related data.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using CitizenPortalSouthAfrica.Models;
using CitizenPortalSouthAfrica.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CitizenPortalSouthAfrica.Views
{
    /// <summary>
    /// Interaction logic for ReportStatusView.xaml
    /// </summary>
    public partial class ReportStatusView : UserControl
    {
        /// <summary>
        /// Initializes the ReportStatusView and sets up data binding with the ReportStatusViewModel.
        /// Subscribes to the Loaded event to refresh reports when the view is loaded.
        /// </summary>
        public ReportStatusView()
        {
            InitializeComponent();

            // Set the DataContext for the view to the ViewModel
            this.DataContext = new ReportStatusViewModel();

            // Subscribe to the Loaded event to refresh reports when the view is loaded
            this.Loaded += ReportStatusView_Loaded;
        }

        /// <summary>
        /// Handles the Loaded event for the ReportStatusView.
        /// Calls the RefreshReports method of the ViewModel to refresh the list of reports when the view is loaded.
        /// </summary>
        private void ReportStatusView_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as ReportStatusViewModel;
            viewModel?.RefreshReports(); // Refresh reports when the view is loaded
        }

        /// <summary>
        /// Handles the click event for a report. 
        /// Expands or collapses the clicked report and fetches or clears related reports based on the report's expanded state.
        /// </summary>
        private void OnReportClicked(object sender, MouseButtonEventArgs e)
        {
            // Get the report object associated with the clicked item
            var report = (sender as FrameworkElement).DataContext as Report;

            // Ensure the report is not null before proceeding
            if (report != null)
            {
                // Access the ViewModel to manipulate the report data
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
                        // Clear related reports if the clicked report is collapsing
                        viewModel.ClearRelatedReports();
                    }
                    else
                    {
                        // Fetch related reports if the clicked report is expanding
                        viewModel.FetchRelatedReports(report);
                    }

                    // Toggle the expanded/collapsed state of the clicked report
                    report.IsExpanded = !report.IsExpanded;
                }
            }
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\