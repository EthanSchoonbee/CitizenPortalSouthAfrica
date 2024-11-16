//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     10/09/2024
 * Last Modified:    17/09/2024
 * 
 * Description:
 * This service class is responsible for handling navigation between different views
 * in the CitizenPortalSouthAfrica application using a singleton pattern. It provides
 * methods to navigate to specified views and to exit the application.
 * 
 * Note: The service works with the MainWindowViewModel to change the current view
 * in the application and ensures that the navigation service is initialized before use.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using CitizenPortalSouthAfrica.Resources;
using CitizenPortalSouthAfrica.ViewModels;
using CitizenPortalSouthAfrica.Views;
using System;
using System.Windows;

namespace CitizenPortalSouthAfrica.Services
{
    /// <summary>
    /// Service to handle application navigation using a singleton pattern.
    /// This service allows switching between views and cleanly exiting the application.
    /// </summary>
    public class NavigationService : INavigationService
    {
        // Singleton instance of the NavigationService
        private static NavigationService _instance;
        // Instance of the MainWindowViewModel to manage the current view
        private readonly MainWindowViewModel _mainWindowViewModel;

        /// <summary>
        /// Private constructor to initialize the service with the MainWindowViewModel instance.
        /// Ensures that only one instance of the service exists (singleton pattern).
        /// </summary>
        /// <param name="mainWindowViewModel">The view model responsible for managing the application's main window and views.</param>
        private NavigationService(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        /// <summary>
        /// Retrieves the singleton instance of the NavigationService.
        /// Throws an exception if the service has not been initialized.
        /// </summary>
        /// <returns>The singleton instance of the NavigationService.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the service is not initialized.</exception>
        public static NavigationService GetInstance()
        {
            if (_instance == null)
            {
                // Throws an exception if trying to access the service before initialization
                throw new InvalidOperationException("NavigationService not initialized.");
            }
            return _instance;
        }

        /// <summary>
        /// Initializes the singleton instance of the NavigationService.
        /// This method must be called before trying to get the instance of the service.
        /// </summary>
        /// <param name="mainWindowViewModel">The MainWindowViewModel used to manage views.</param>
        public static void Initialize(MainWindowViewModel mainWindowViewModel)
        {
            if (_instance == null)
            {
                _instance = new NavigationService(mainWindowViewModel);
            }
        }

        /// <summary>
        /// Navigates to a specified view based on the provided view name.
        /// Updates the CurrentView property in the MainWindowViewModel to switch views.
        /// </summary>
        /// <param name="viewName">The name of the view to navigate to (e.g., "Home", "ReportIssues").</param>
        public void NavigateTo(string viewName)
        {
            switch (viewName)
            {
                case "Home":
                    // Navigates to the HomeView
                    _mainWindowViewModel.CurrentView = new HomeView();
                    break;
                case "ReportIssues":
                    // Navigates to the ReportIssuesView and binds the associated ViewModel
                    _mainWindowViewModel.CurrentView = new ReportIssuesView { DataContext = _mainWindowViewModel.ReportIssuesViewModel };
                    break;
                case "EventsAndAnnouncements":
                    // Navigates to the EventsAndAnnouncementsView and binds the associated ViewModel
                    _mainWindowViewModel.CurrentView = new EventsAndAnnouncementsView { DataContext = _mainWindowViewModel.EventsAndAnoncementsViewModel };
                    break;
                case "RequestStatus":
                    // Navigates to the ReportStatusView and binds the associated ViewModel
                    _mainWindowViewModel.CurrentView = new ReportStatusView { DataContext = _mainWindowViewModel.ReportStatusViewModel };
                    break;
            }
        }

        /// <summary>
        /// Exits the application by shutting down the current WPF application.
        /// </summary>
        public void ExitApplication()
        {
            Application.Current.Shutdown();
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\
