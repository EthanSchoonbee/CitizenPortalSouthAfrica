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

                    viewModel.FetchRelatedReports(report);

                    // Toggle the clicked report
                    report.IsExpanded = !report.IsExpanded;
                }
            }
        }

        private void OnToggleExpandCollapse(object sender, RoutedEventArgs e)
        {
            var toggleButton = sender as ToggleButton;
            var report = toggleButton?.DataContext as Report; // Adjust based on your actual model

            if (report != null)
            {
                var expandedContent = FindVisualChild<Grid>(toggleButton);
                var animation = (Storyboard)FindResource("ExpandCollapseAnimation");

                if (expandedContent.Visibility == Visibility.Collapsed)
                {
                    expandedContent.Visibility = Visibility.Visible;
                    animation.Begin(expandedContent);

                }
                else
                {
                    expandedContent.Visibility = Visibility.Collapsed;
                }
            }
        }

        // Helper method to find a child of type T
        private static T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}
