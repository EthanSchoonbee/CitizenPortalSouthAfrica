using CitizenPortalSouthAfrica.Models;
using CitizenPortalSouthAfrica.ViewModels;
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

            this.DataContext = new ReportStatusViewModel();
        }

        private void OnReportClicked(object sender, MouseButtonEventArgs e)
        {
            // Get the clicked report
            var report = (sender as FrameworkElement).DataContext as Report;

            if (report != null)
            {
                // Toggle the IsExpanded property
                report.IsExpanded = !report.IsExpanded;

                // Inform the view model that the report has changed
                var viewModel = this.DataContext as ReportStatusViewModel;
                if (viewModel != null)
                {
                    // You can either directly notify the UI or manipulate the reports collection
                    viewModel.OnPropertyChanged(nameof(viewModel.Reports));
                }
            }
        }
    }
}
