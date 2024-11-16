using CitizenPortalSouthAfrica.Models;
using CitizenPortalSouthAfrica.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CitizenPortalSouthAfrica.ViewModels
{
    public class ReportStatusViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<Report> Reports { get; set; }
        public ObservableCollection<Report> FilteredReports { get; set; }


        private readonly FileManagementService _fileManagementService; // Service for file operations
        private readonly ValidationService _validationService; // Service for form validation
        private readonly ReportIssueRepository _repository; // Repository for report issue data
        private ReportBST reportBST; //Red-Black Binary Search Tree to store reports


        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
                FilterReports(); // Automatically filter reports as user types
            }
        }

        private bool _isFilteredReportsEmpty;
        public bool IsFilteredReportsEmpty
        {
            get => _isFilteredReportsEmpty;
            set
            {
                _isFilteredReportsEmpty = value;
                OnPropertyChanged();
            }
        }

        // Commands for user actions
        public ICommand NavigateToHomeCommand { get; } // Command to navigate to the home view
        public ICommand NavigateToEventsAndAnnouncementsCommand { get; } // Command to navigate to the home view
        public ICommand NavigateToReportIssuesCommand { get; } // Command to navigate to the request status view
        public ICommand ExitCommand { get; } // Command to exit the application
        public ICommand SearchCommand { get; } // Command to filter reports
        public ICommand ClearCommand { get; } // Command to clear the search query

        // Constructor initializing services and commands
        public ReportStatusViewModel()
        {
            // Initialize services and repository
            _fileManagementService = new FileManagementService();
            _validationService = new ValidationService();
            _repository = new ReportIssueRepository();
            reportBST = new ReportBST();

            Reports = new ObservableCollection<Report>();
            FilteredReports = new ObservableCollection<Report>();
            FilteredReports.CollectionChanged += (s, e) =>
            {
                IsFilteredReportsEmpty = FilteredReports.Count == 0;
            };

            LoadReportsFromDatabase();

            // Initialize commands
            SearchCommand = new RelayCommand(FilterReports);
            ClearCommand = new RelayCommand(ClearSearchQuery);

            FilterReports(); // Initialize FilteredReports with all reports

            ExitCommand = new RelayCommand(() => Services.NavigationService.GetInstance().ExitApplication()); // Command to exit the application
            NavigateToHomeCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.Home));
            NavigateToEventsAndAnnouncementsCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.EventsAndAnnouncements));
            NavigateToReportIssuesCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.ReportIssues));
        }

        private void LoadReportsFromDatabase()
        {
            // Fetch reports from SQLite database
            //List<Report> reportsFromDb = _repository.GetReports(); // Assuming your repository fetches reports

            List<Report> reportsFromDb = new List<Report>
            {
                new Report { Id = 1, Location = "Location A", Name = "Report 1", CreationDate = DateTime.Parse("2024-11-15 20:35:55.4071655"), Category = "Cat", Description= "This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!", Status = "Open", IsExpanded = false },
                new Report { Id = 2, Location = "Location B", Name = "Report 2", CreationDate = DateTime.Parse("2024-11-15 20:35:55.4071655"), Category = "Cat", Description= "This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!", Status = "Closed", IsExpanded = false },
                new Report { Id = 3, Location = "Location C", Name = "Report 3", CreationDate = DateTime.Parse("2024-11-15 20:35:55.4071655"), Category = "Cat", Description= "This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!", Status = "In Progress", IsExpanded = false },
                new Report { Id = 4, Location = "Location D", Name = "Report 4", CreationDate = DateTime.Parse("2024-11-15 20:35:55.4071655"), Category = "Cat", Description= "This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!", Status = "Open", IsExpanded = false },
                new Report { Id = 5, Location = "Location E", Name = "Report 5", CreationDate = DateTime.Parse("2024-11-15 20:35:55.4071655"), Category = "Cat", Description= "This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!This is a test description and isnt real so dont think about it too much and ignore it infact cause fuck this shit!", Status = "Closed", IsExpanded = false },
            };

            // Insert each report into the BST
            foreach (var report in reportsFromDb)
            {
                reportBST.Insert(report);
            }

            // Use in-order traversal to get sorted reports
            var sortedReports = reportBST.InOrderTraversal();

            // Populate the ObservableCollection with sorted reports
            foreach (var report in sortedReports)
            {
                Reports.Add(report);
            }
        }

        private void FilterReports()
        {
            FilteredReports.Clear();

            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                // in-order traversal to show all reports if there's no query
                var allReports = reportBST.InOrderTraversal();
                foreach (var report in allReports)
                {
                    FilteredReports.Add(report);
                }
            }
            else
            {
                // BST search to find matching reports
                var matchingReports = reportBST.Search(SearchQuery);
                foreach (var report in matchingReports)
                {
                    FilteredReports.Add(report);
                }
            }
        }
        
        private void ClearSearchQuery()
        {
            SearchQuery = "";
            FilterReports();  // Refresh the filtered reports
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
