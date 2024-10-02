using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace CitizenPortalSouthAfrica.ViewModels
{
    public class EventsAndAnoncementsViewModel
    {

        // Commands for user actions
        public ICommand NavigateToHomeCommand { get; } // Command to navigate to the home view
        public ICommand NavigateToReportIssuesCommand { get; } // Command to navigate to the report issues view
        public ICommand ExitCommand { get; } // Command to exit the application

        public EventsAndAnoncementsViewModel()
        {
            // Initialize commands
            ExitCommand = new RelayCommand(() => Services.NavigationService.GetInstance().ExitApplication()); // Command to exit the application
            NavigateToHomeCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo("Home")); // Command to navigate to home
            NavigateToReportIssuesCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo("ReportIssues")); // Command to navigate to report issues
        }

    }
}
