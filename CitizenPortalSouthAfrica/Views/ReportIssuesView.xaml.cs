using CitizenPortalSouthAfrica.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace CitizenPortalSouthAfrica.Views
{
    /// <summary>
    /// Interaction logic for ReportIssuesView.xaml
    /// </summary>
    public partial class ReportIssuesView : UserControl
    {
        public ReportIssuesViewModel ViewModel { get; private set; }

        public ReportIssuesView()
        {
            InitializeComponent();
            /*ViewModel = new ReportIssuesViewModel();
            DataContext = ViewModel;*/
        }

        private void LocationTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var viewModel = (ReportIssuesViewModel)DataContext;
            viewModel.LocationClicked = true;
            viewModel.CategoryClicked = false;
            viewModel.DescriptionClicked = false;
            viewModel.UpdateFormCompletionPercentage();
        }

        private void CategoryComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var viewModel = (ReportIssuesViewModel)DataContext;
            viewModel.LocationClicked = false;
            viewModel.CategoryClicked = true;
            viewModel.DescriptionClicked = false;
            viewModel.UpdateFormCompletionPercentage();
        }

        private void DescriptionRichTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var viewModel = (ReportIssuesViewModel)DataContext;
            viewModel.LocationClicked = false;
            viewModel.CategoryClicked = false;
            viewModel.DescriptionClicked = true;
            viewModel.UpdateFormCompletionPercentage();
        }
        private void DescriptionRichTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // Prevent the default action of the Enter key
            }
        }
    }
}
