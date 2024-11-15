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
        public ObservableCollection<Report> Reports { get; set; }

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

            Reports = new ObservableCollection<Report>
            {
                new Report { Id = 1, Location = "Location A", Name = "Report A", Category = "Cat", Description= "This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!", Status = "Open", IsExpanded = false },
                new Report { Id = 2, Location = "Location B", Name = "Report B", Category = "Cat", Description= "This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!", Status = "Closed", IsExpanded = false },
                new Report { Id = 3, Location = "Location C", Name = "Report C", Category = "Cat", Description= "This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!", Status = "In Progress", IsExpanded = false },
                new Report { Id = 4, Location = "Location D", Name = "Report D", Category = "Cat", Description= "This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!", Status = "Open", IsExpanded = false },
                new Report { Id = 5, Location = "Location E", Name = "Report E", Category = "Cat", Description= "This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!", Status = "Closed", IsExpanded = false },
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
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
