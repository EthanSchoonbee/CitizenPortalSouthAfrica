//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     10/09/2024
 * Last Modified:    17/11/2024
 * 
 * Description:
 * ViewModel for managing the status of reports in the CitizenPortalSouthAfrica application.
 * This class provides functionality for filtering, searching, and retrieving related reports
 * based on location. It also manages commands for navigating between views and performing
 * actions like searching or clearing the search query.
 * 
 * Dependencies:
 * - FileManagementService: Responsible for handling file operations.
 * - ValidationService: Used for validating inputs in the report status form.
 * - ReportIssueRepository: Provides methods to interact with the report issue data storage.
 * - ReportBST: A Red-Black Binary Search Tree implementation for storing and sorting reports.
 * - Graph: A data structure to manage relationships between report locations.
 * 
 * Commands:
 * - NavigateToHomeCommand: Navigates to the home view.
 * - NavigateToEventsAndAnnouncementsCommand: Navigates to the events and announcements view.
 * - NavigateToReportIssuesCommand: Navigates to the report issues view.
 * - ExitCommand: Exits the application.
 * - SearchCommand: Filters the reports based on the search query.
 * - ClearCommand: Clears the search query and resets the filter.
 * - FetchRelatedReportsCommand: Fetches reports related to the selected report based on location.
 * 
 * Properties:
 * - Reports: A collection of all available reports.
 * - FilteredReports: A collection of reports filtered based on the search query.
 * - IsFilteredReportsEmpty: Boolean flag indicating whether the filtered reports collection is empty.
 * - SearchQuery: The query string entered by the user to filter the reports.
 * - RelatedReports: A collection of reports that are related to the selected report based on location.
 * 
 * Methods:
 * - LoadReportsFromDatabase: Fetches and loads all reports from the database, initializing the BST and location graph.
 * - FilterReports: Filters the reports based on the current search query and updates the filtered reports collection.
 * - FetchRelatedReports: Fetches reports related to the selected report, using location information stored in the graph.
 * - GetReportsByLocation: Retrieves reports related to a specific location by using the location graph.
 * - ClearRelatedReports: Clears the related reports collection.
 * - ClearSearchQuery: Clears the search query and refreshes the filtered reports.
 * - RefreshReports: Reloads the reports from the database and updates the filtered reports collection.
 * 
 * Implementation Details:
 * - Uses `ObservableCollection` for binding to the UI for reports and related reports.
 * - Implements `INotifyPropertyChanged` to notify the UI of property changes.
 * - Uses `RelayCommand` from MVVM Light for handling commands.
 * - Uses `ReportBST` (a Red-Black Tree) for efficiently managing and searching through reports.
 * - Employs `Graph<string>` for managing relationships between report locations.
 * 
 * Notes:
 * - The class is designed to be used in an MVVM pattern, where it acts as the ViewModel that interacts with the view.
 * - Command logic is handled using `RelayCommand` to separate concerns and improve maintainability.
 * - Data is fetched from a SQLite database and stored in an in-memory BST for fast searching.
 * 
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using CitizenPortalSouthAfrica.Models;
using CitizenPortalSouthAfrica.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CitizenPortalSouthAfrica.ViewModels
{
    /// <summary>
    /// ViewModel that handles the logic for the report status view.
    /// Manages fetching, filtering, and displaying reports, and provides commands for navigating and interacting with the UI.
    /// </summary>
    public class ReportStatusViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<Report> Reports { get; set; } // Collection of all reports
        public ObservableCollection<Report> FilteredReports { get; set; } // Collection of reports after filtering

        private readonly FileManagementService _fileManagementService; // Service for file operations
        private readonly ValidationService _validationService; // Service for form validation
        private readonly ReportIssueRepository _repository; // Repository for report issue data
        private ReportBST _reportBST; //Red-Black Binary Search Tree to store reports
        private Graph<string> _locationGraph; //Graph to manage location relationships of reports

        private string _searchQuery; // Search query for filtering reports
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

        private bool _isFilteredReportsEmpty; // Flag to indicate if there are no filtered reports
        public bool IsFilteredReportsEmpty
        {
            get => _isFilteredReportsEmpty;
            set
            {
                _isFilteredReportsEmpty = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Report> _relatedReports; // Related reports based on location
        public ObservableCollection<Report> RelatedReports
        {
            get => _relatedReports;
            set
            {
                if (_relatedReports != value)
                {
                    _relatedReports = value;
                    OnPropertyChanged(nameof(RelatedReports)); // Notify the UI of changes
                }
            }
        }

        // Commands for user actions
        public ICommand NavigateToHomeCommand { get; } // Command to navigate to the home view
        public ICommand NavigateToEventsAndAnnouncementsCommand { get; } // Command to navigate to the home view
        public ICommand NavigateToReportIssuesCommand { get; } // Command to navigate to the request status view
        public ICommand ExitCommand { get; } // Command to exit the application
        public ICommand SearchCommand { get; } // Command to filter reports
        public ICommand ClearCommand { get; } // Command to clear the search query
        public ICommand FetchRelatedReportsCommand { get; } // Command to fetch related reports by location


        // Constructor initializing services and commands
        public ReportStatusViewModel() 
        {
            // Initialize services and repository
            _fileManagementService = new FileManagementService();
            _validationService = new ValidationService();
            _repository = new ReportIssueRepository();
            _reportBST = new ReportBST();
            _locationGraph = new Graph<string>();

            Reports = new ObservableCollection<Report>();
            FilteredReports = new ObservableCollection<Report>();
            FilteredReports.CollectionChanged += (s, e) =>
            { 
                IsFilteredReportsEmpty = FilteredReports.Count == 0; // Set flag when no reports are available
            };
            RelatedReports = new ObservableCollection<Report>(); // Initialize RelatedReports here

            LoadReportsFromDatabase(); // Populate Reports and Graph

            // Initialize commands
            FetchRelatedReportsCommand = new RelayCommand<Report>(FetchRelatedReports);

            SearchCommand = new RelayCommand(FilterReports);
            ClearCommand = new RelayCommand(ClearSearchQuery);

            FilterReports(); // Initialize FilteredReports with all reports

            ExitCommand = new RelayCommand(() => Services.NavigationService.GetInstance().ExitApplication()); // Command to exit the application
            NavigateToHomeCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.Home));
            NavigateToEventsAndAnnouncementsCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.EventsAndAnnouncements));
            NavigateToReportIssuesCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.ReportIssues));
        }

        /// <summary>
        /// Loads reports from the database, initializes the BST and location graph, and populates the Reports collection.
        /// </summary>
        private void LoadReportsFromDatabase()
        {
            // Fetch reports from SQLite database into Red-Black BST
            _reportBST = _repository.GetReportBST();

            Reports.Clear(); // Clear the current reports
            _locationGraph.Clear(); // Clear the location graph

            // Use in-order traversal to get sorted reports
            var sortedReports = _reportBST.InOrderTraversal();

            // Populate the ObservableCollection with sorted reports
            foreach (var report in sortedReports)
            {
                Reports.Add(report);

                // Check if the edge already exists before adding
                if (!_locationGraph.ContainsEdge(report.Location, report.Id.ToString()))
                {
                    _locationGraph.AddEdge(report.Location, report.Id.ToString()); // Add location edge
                }
            }
        }

        /// <summary>
        /// Filters reports based on the search query. 
        /// If the query is empty, all reports are displayed; otherwise, matching reports are shown.
        /// </summary>
        private void FilterReports()
        {
            FilteredReports.Clear(); // Clear the existing filtered reports

            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                // in-order traversal to show all reports if there's no query
                var allReports = _reportBST.InOrderTraversal();
                foreach (var report in allReports)
                {
                    FilteredReports.Add(report);
                }
            }
            else
            {
                // BST search to find matching reports
                var matchingReports = _reportBST.Search(SearchQuery);
                foreach (var report in matchingReports)
                {
                    FilteredReports.Add(report);
                }
            }
        }

        /// <summary>
        /// Fetches reports related to the selected report based on its location.
        /// </summary>
        public void FetchRelatedReports(Report selectedReport)
        {
            if (selectedReport != null)
            {
                RelatedReports.Clear(); // Clear the existing items
                var newReports = GetReportsByLocation(selectedReport);
                foreach (var report in newReports)
                {
                    RelatedReports.Add(report); // Add new reports to the related collection
                }
            }
        }

        /// <summary>
        /// Retrieves related reports based on the location of the selected report.
        /// </summary>
        public ObservableCollection<Report> GetReportsByLocation(Report selectedReport)
        {
            var relatedReports = new ObservableCollection<Report>();
            var reportIds = _locationGraph.GetReportsAtLocation(selectedReport.Location);

            if (reportIds == null || !reportIds.Any())
            {
                return relatedReports; // Return empty if no related reports
            }

            // Add related reports to the collection
            foreach (var reportId in reportIds)
            {
                var matchingReport = Reports.FirstOrDefault(r => r.Id.ToString() == reportId); 

                if (matchingReport != null && matchingReport.Id != selectedReport.Id) // Exclude selected report
                {
                    relatedReports.Add(matchingReport);
                }
                else
                {
                    Console.WriteLine($"No matching report found for ID: {reportId}");
                }
            }

            return relatedReports;
        }

        /// <summary>
        /// Clears the related reports collection.
        /// </summary>
        public void ClearRelatedReports()
        {
            RelatedReports.Clear(); // Clear related reports
        }

        /// <summary>
        /// Clears the search query and refreshes the filtered reports.
        /// </summary>
        private void ClearSearchQuery()
        {
            SearchQuery = ""; // Clear the search query
            FilterReports();  // Refresh the filtered reports
        }

        /// <summary>
        /// Refreshes the reports by re-fetching them from the database and updating the filtered reports.
        /// </summary>
        public void RefreshReports()
        {
            LoadReportsFromDatabase(); // Re-fetch reports from the database
            FilterReports(); // Update the filtered reports
        }

        // Event to notify the UI of property changes
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event to notify the UI of changes to a property.
        /// </summary>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Raise the event
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\