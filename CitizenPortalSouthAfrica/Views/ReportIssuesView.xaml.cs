//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     10/09/2024
 * Last Modified:    16/11/2024
 * 
 * Description:
 * This class defines the interaction logic for the ReportIssuesView in the CitizenPortalSouthAfrica application.
 * The ReportIssuesView allows users to report issues by interacting with the form fields (Location, Category, 
 * and Description). It manages focus events for the fields and tracks the completion percentage of the form based 
 * on user interaction. The form also prevents the default behavior of the Enter key in the description field.
 * 
 * Dependencies:
 * - ReportIssuesViewModel: ViewModel for managing data and logic for reporting issues.
 * 
 * Methods:
 * - ReportIssuesView(): Constructor that initializes the view and prepares the DataContext (ViewModel).
 * - LocationTextBox_GotFocus(): Event handler that sets the focus state for the Location field and updates the form completion.
 * - CategoryComboBox_GotFocus(): Event handler that sets the focus state for the Category field and updates the form completion.
 * - DescriptionRichTextBox_GotFocus(): Event handler that sets the focus state for the Description field and updates the form completion.
 * - DescriptionRichTextBox_PreviewKeyDown(): Event handler that prevents the default Enter key behavior in the Description field.
 * 
 * Implementation Details:
 * - Uses DataContext to bind the view to the ReportIssuesViewModel for managing form data.
 * - Tracks focus events on form fields to adjust the form's completion percentage.
 * - Implements a key press handler for the Description field to prevent the Enter key from submitting the form.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using CitizenPortalSouthAfrica.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CitizenPortalSouthAfrica.Views
{
    /// <summary>
    /// Interaction logic for ReportIssuesView.xaml
    /// </summary>
    public partial class ReportIssuesView : UserControl
    {
        /// <summary>
        /// Gets or sets the ViewModel associated with this view.
        /// The ViewModel is responsible for managing the data and business logic for reporting issues.
        /// </summary>
        public ReportIssuesViewModel ViewModel { get; private set; }

        /// <summary>
        /// Constructor for the ReportIssuesView class.
        /// Initializes the view and prepares the DataContext by binding it to the ViewModel.
        /// </summary>
        public ReportIssuesView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the Location text box when it receives focus.
        /// It sets the focus state for the Location field, and updates the form completion percentage.
        /// </summary>
        private void LocationTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Access the ViewModel and update the focus state for fields
            var viewModel = (ReportIssuesViewModel)DataContext;
            viewModel.LocationClicked = true;
            viewModel.CategoryClicked = false;
            viewModel.DescriptionClicked = false;
            // Update the form completion percentage
            viewModel.UpdateFormCompletionPercentage();
        }

        /// <summary>
        /// Event handler for the Category combo box when it receives focus.
        /// It sets the focus state for the Category field, and updates the form completion percentage.
        /// </summary>
        private void CategoryComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Access the ViewModel and update the focus state for fields
            var viewModel = (ReportIssuesViewModel)DataContext;
            viewModel.LocationClicked = false;
            viewModel.CategoryClicked = true;
            viewModel.DescriptionClicked = false;
            // Update the form completion percentage
            viewModel.UpdateFormCompletionPercentage();
        }

        /// <summary>
        /// Event handler for the Description rich text box when it receives focus.
        /// It sets the focus state for the Description field, and updates the form completion percentage.
        /// </summary>
        private void DescriptionRichTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Access the ViewModel and update the focus state for fields
            var viewModel = (ReportIssuesViewModel)DataContext;
            viewModel.LocationClicked = false;
            viewModel.CategoryClicked = false;
            viewModel.DescriptionClicked = true;
            // Update the form completion percentage
            viewModel.UpdateFormCompletionPercentage();
        }

        /// <summary>
        /// Event handler for the key press action in the Description rich text box.
        /// Specifically, it prevents the Enter key from performing its default behavior (submitting the form).
        /// </summary>
        private void DescriptionRichTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Prevent the default action of the Enter key
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // Prevent the default action of the Enter key
            }
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\