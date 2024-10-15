//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     13/10/2024
 * Last Modified:    13/10/2024
 * 
 * Description:
 * This class represents the user control for displaying events and announcements
 * in the CitizenPortalSouthAfrica WPF application. It handles loading data and 
 * user interactions such as searching and selecting suggestions.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using CitizenPortalSouthAfrica.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CitizenPortalSouthAfrica.Views
{
    /// <summary>
    /// Interaction logic for EventsAndAnnouncementsView.xaml
    /// This class represents the user control for displaying events and announcements.
    /// It handles loading data and user interactions such as searching and selecting suggestions.
    /// </summary>
    public partial class EventsAndAnnouncementsView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventsAndAnnouncementsView"/> class.
        /// Sets up the Loaded event handler to load data when the view is displayed.
        /// </summary>
        public EventsAndAnnouncementsView()
        {
            InitializeComponent();
            this.Loaded += EventsAndAnnouncementsView_Loaded; // Subscribe to the Loaded event
        }

        /// <summary>
        /// Event handler for the Loaded event of the EventsAndAnnouncementsView.
        /// Loads event and announcement data asynchronously if the collections are empty.
        /// </summary>
        private void EventsAndAnnouncementsView_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is EventsAndAnoncementsViewModel viewModel)
            {
                // Only load data if both events and announcements collections are empty
                if (!viewModel.Events.Any() && !viewModel.Announcements.Any())
                {
                    _ = viewModel.LoadEventAndAnnouncementDataAsync(); // Asynchronously load data
                }
            }
        }

        /// <summary>
        /// Event handler for the KeyDown event of the TextBox.
        /// Executes the search command when the Enter key is pressed.
        /// </summary>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                (DataContext as EventsAndAnoncementsViewModel)?.SearchCommand.Execute(null); // Execute search command
            }
        }

        /// <summary>
        /// Event handler for the SelectionChanged event of the SearchSuggestionsListBox.
        /// Updates the searched value and filters events and announcements based on the selected suggestion.
        /// </summary>
        private void SearchSuggestionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is string selectedSuggestion)
            {
                // Assuming your DataContext is set to the ViewModel
                var viewModel = DataContext as EventsAndAnoncementsViewModel;
                if (viewModel != null)
                {
                    viewModel.SearchedValue = selectedSuggestion; // Update the SearchedValue property
                    viewModel.FilterEventsAndAnnouncements(); // Filter events and announcements based on searched value
                    listBox.SelectedItem = null; // Deselect the item to allow for re-selection
                }
            }
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\