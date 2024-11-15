using CitizenPortalSouthAfrica.Models;
using CitizenPortalSouthAfrica.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CitizenPortalSouthAfrica.ViewModels
{
    public class ReportStatusViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<ReportIssue> Reports { get; set; }

        private readonly FileManagementService _fileManagementService; // Service for file operations
        private readonly ValidationService _validationService; // Service for form validation
        private readonly ReportIssueRepository _repository; // Repository for report issue data

        // Commands for user actions
        public ICommand NavigateToHomeCommand { get; } // Command to navigate to the home view
        public ICommand NavigateToEventsAndAnnouncementsCommand { get; } // Command to navigate to the home view
        public ICommand NavigateToReportIssuesCommand { get; } // Command to navigate to the request status view
        public ICommand ExitCommand { get; } // Command to exit the application

        // Constructor initializing services and commands
        public ReportStatusViewModel()
        {
            // Initialize services and repository
            _fileManagementService = new FileManagementService();
            _validationService = new ValidationService();
            _repository = new ReportIssueRepository();

            Reports = new ObservableCollection<ReportIssue>
            {
                new ReportIssue { Id = 1, Location = "Location A" },
                new ReportIssue { Id = 2, Location = "Location B" },
                new ReportIssue { Id = 3, Location = "Location C" },
                new ReportIssue { Id = 4, Location = "Location D" },
                new ReportIssue { Id = 5, Location = "Location E" },
                new ReportIssue { Id = 6, Location = "Location F" },
                new ReportIssue { Id = 7, Location = "Location G" },
                new ReportIssue { Id = 8, Location = "Location H" },
                new ReportIssue { Id = 9, Location = "Location I" },
                new ReportIssue { Id = 10, Location = "Location J" },
                // Add more reports
            };

            // Initialize commands
            ExitCommand = new RelayCommand(() => Services.NavigationService.GetInstance().ExitApplication()); // Command to exit the application
            NavigateToHomeCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.Home));
            NavigateToEventsAndAnnouncementsCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.EventsAndAnnouncements));
            NavigateToReportIssuesCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.ReportIssues));
        }

        // Event to notify the UI of property changes
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to raise the PropertyChanged event
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
