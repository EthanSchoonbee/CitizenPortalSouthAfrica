//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     10/09/2024
 * Last Modified:    17/09/2024
 * 
 * Description:
 * This class represents the view model for the Main Window of the CitizenPortalSouthAfrica
 * WPF application. It manages the navigation logic between different views (such as Home, 
 * ReportIssues, and others) and handles application exit logic. The class follows the MVVM 
 * pattern, utilizing commands for navigation and managing the currently active view.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using CitizenPortalSouthAfrica.Models;
using CitizenPortalSouthAfrica.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Controls;

namespace CitizenPortalSouthAfrica.ViewModels
{
    /// <summary>
    /// ViewModel for the main window logic.
    /// This class handles the navigation between different views and manages commands for
    /// navigation and exiting the application.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        // Variable to store the currently active UserControl view
        private UserControl _currentView;

        /// <summary>
        /// Property to get and set the current active view.
        /// This property is bound to the MainWindow and allows changing the view
        /// dynamically.
        /// </summary>
        public UserControl CurrentView
        {
            get => _currentView;
            set => Set(ref _currentView, value);
        }

        /// <summary>
        /// Command to handle exiting the application.
        /// </summary>
        public RelayCommand ExitCommand { get; private set; }

        /// <summary>
        /// Command to navigate to the Home view.
        /// </summary>
        public RelayCommand NavigateToHomeCommand { get; private set; }

        /// <summary>
        /// Command to navigate to the Report Issues view.
        /// </summary>
        public RelayCommand NavigateToReportIssuesCommand { get; private set; }

        /// <summary>
        /// Command to navigate to the Events and Announcements view.
        /// (Currently commented out for future implementation)
        /// </summary>
        public RelayCommand NavigateToEventsAndAnnouncementsCommand { get; private set; }

        /// <summary>
        /// Command to navigate to the Request Status view.
        /// (Currently commented out for future implementation)
        /// </summary>
        public RelayCommand NavigateToRequestStatusCommand { get; private set; }

        /// <summary>
        /// ViewModel for the Report Issues functionality.
        /// </summary>
        public ReportIssuesViewModel ReportIssuesViewModel { get; private set; }
        public EventsAndAnoncementsViewModel EventsAndAnoncementsViewModel { get; private set; }


        /// <summary>
        /// Constructor for MainWindowViewModel.
        /// Initializes the navigation service, sets up the commands, and sets the default view to Home.
        /// </summary>
        public MainWindowViewModel()
        {
            // Initialize navigation service with this view model
            Services.NavigationService.Initialize(this);

            // Set up commands for navigation and application exit
            ExitCommand = new RelayCommand(() => Services.NavigationService.GetInstance().ExitApplication());
            NavigateToHomeCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.Home));
            NavigateToReportIssuesCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.ReportIssues));
            NavigateToEventsAndAnnouncementsCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.EventsAndAnnouncements));
            NavigateToRequestStatusCommand = new RelayCommand(() => MessageBox.Show(Constants.ErrorMessages.FeatureNotAvaliableMessage,
                                                                                    Constants.ErrorMessages.FeatureNotAvaliableHeader,
                                                                                    MessageBoxButton.OK,
                                                                                    MessageBoxImage.Exclamation));
            // Uncomment and implement these commands when needed

            // Initialize the ReportIssuesViewModel
            ReportIssuesViewModel = new ReportIssuesViewModel();
            EventsAndAnoncementsViewModel = new EventsAndAnoncementsViewModel();
            // Set the default view to Home
            CurrentView = new HomeView();
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\