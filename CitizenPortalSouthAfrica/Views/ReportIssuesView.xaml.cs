using CitizenPortalSouthAfrica.ViewModels;
using System.Windows;
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

        private void LocationTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            GuideText.Text = "Please enter the location where the issue occurred.";
        }

        private void CategoryComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            GuideText.Text = "Select the category that best describes the issue.";
        }

        private void DescriptionRichTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            GuideText.Text = "Provide a detailed description of the issue.";
        }
    }
}
