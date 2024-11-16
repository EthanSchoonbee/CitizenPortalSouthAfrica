//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     13/10/2024
 * Last Modified:    13/10/2024
 * 
 * Description:
 * This class serves as the ViewModel for the Events and Announcements 
 * user control in the CitizenPortalSouthAfrica WPF application. It 
 * handles loading data from the repository, managing user interactions, 
 * and providing commands for various actions such as searching and 
 * navigating through the application.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CitizenPortalSouthAfrica.Models;
using CitizenPortalSouthAfrica.Services;
using GalaSoft.MvvmLight.Command;

namespace CitizenPortalSouthAfrica.ViewModels
{
    /// <summary>
    /// ViewModel for managing events and announcements in the application.
    /// Implements INotifyPropertyChanged for data binding with the UI.
    /// </summary>
    public class EventsAndAnoncementsViewModel : INotifyPropertyChanged
    {
        private readonly EventAndAnnouncementRepository _repository; // Repository for accessing event and announcement data

        public ObservableCollection<Event> Events { get; private set; } // Collection of events for UI binding
        public ObservableCollection<Announcement> Announcements { get; private set; } // Collection of announcements for UI binding

        // Dictionary to store user search history and frequencies for recommendations
        private readonly Dictionary<string, int> _searchHistoryFrequency = new Dictionary<string, int>();
        // Stack to maintain search history order and limit displayed history size
        private Stack<string> _searchHistoryStack = new Stack<string>();

        private DispatcherTimer _debounceTimer;


        private string _searchedValue; // The current search query entered by the user
        public string SearchedValue
        {
            get => _searchedValue; // Getter for searched value
            set
            {
                if (_searchedValue != value) // Only update if the value has changed
                {
                    _searchedValue = value; // Set the new searched value
                    OnPropertyChanged(nameof(SearchedValue)); // Notify that searched value has changed
                    // If the searched value is empty, filter events and announcements
                    if (string.IsNullOrWhiteSpace(_searchedValue))
                    {
                        FilterEventsAndAnnouncements(); // Run filter method when the value is empty
                    }

                    UpdateSearchHistory(); // Update the search history with the new value
                    UpdateSearchSuggestions(); // Update suggestions based on the new search value
                }
            }
        }

        private ObservableCollection<string> _searchHistory; // Collection for binding search history to the UI
        public ObservableCollection<string> SearchHistory
        {
            get => _searchHistory; // Getter for search history
            set
            {
                _searchHistory = value; // Set the new search history
                OnPropertyChanged(); // Notify that the property has changed
                OnPropertyChanged(nameof(SearchHistory)); // Notify change for the search history property
                OnPropertyChanged(nameof(HistoryHeights)); // Notify change for UI height calculation
                OnPropertyChanged(nameof(IsHistoryEmpty)); // Notify change for checking if history is empty
            }
        }

        public bool IsHistoryEmpty => !SearchHistory.Any(); // Check if search history is empty

        private ObservableCollection<string> searchSuggestions; // Collection for search suggestions based on input
        public ObservableCollection<string> SearchSuggestions
        {
            get => searchSuggestions; // Getter for search suggestions
            set
            {
                searchSuggestions = value; // Set the new suggestions
                OnPropertyChanged(nameof(SearchSuggestions)); // Notify change for search suggestions
            }
        }

        private ObservableCollection<string> _searchRecommendations; // Collection for search recommendations
        public ObservableCollection<string> SearchRecommendations
        {
            get => _searchRecommendations; // Getter for search recommendations
            set
            {
                _searchRecommendations = value; // Set the new recommendations
                OnPropertyChanged(); // Notify change for recommendations
                OnPropertyChanged(nameof(SearchRecommendations)); // Notify change for the search recommendations property
                OnPropertyChanged(nameof(IsRecommendationsEmpty)); // Notify change for checking if recommendations are empty
            }
        }

        public bool IsRecommendationsEmpty => !SearchRecommendations.Any(); // Check if recommendations are empty

        private bool _noEventResultsFound; // Flag for no event results found
        public bool NoEventResultsFound
        {
            get => _noEventResultsFound; // Getter for no event results found flag
            set
            {
                if (_noEventResultsFound != value) // Only update if the value has changed
                {
                    _noEventResultsFound = value; // Set the new flag value
                    OnPropertyChanged(nameof(NoEventResultsFound)); // Notify property change for binding
                }
            }
        }

        private bool _noAnnouncementResultsFound; // Flag for no announcement results found
        public bool NoAnnouncementResultsFound
        {
            get => _noAnnouncementResultsFound; // Getter for no announcement results found flag
            set
            {
                if (_noAnnouncementResultsFound != value) // Only update if the value has changed
                {
                    _noAnnouncementResultsFound = value; // Set the new flag value
                    OnPropertyChanged(nameof(NoAnnouncementResultsFound)); // Notify property change for binding
                }
            }
        }

        // Commands for user actions
        public ICommand NavigateToHomeCommand { get; } // Command to navigate to the home view
        public ICommand NavigateToReportIssuesCommand { get; } // Command to navigate to the report issues view
        public ICommand ExitCommand { get; } // Command to exit the application
        public ICommand NavigateToRequestStatusCommand { get; } // Command to navigate to the request status view
        public ICommand FilterCommand { get; } // Command to filter events and announcements
        public ICommand SearchCommand { get; } // Command to perform a search

        /// <summary>
        /// Initializes a new instance of the EventsAndAnoncementsViewModel class.
        /// Sets up the repository and initializes collections and commands.
        /// </summary>
        public EventsAndAnoncementsViewModel()
        {
            _repository = new EventAndAnnouncementRepository(); // Instantiate the repository

            // Initialize collections for events, announcements, suggestions, history, and recommendations
            Events = new ObservableCollection<Event>();
            Announcements = new ObservableCollection<Announcement>();
            SearchSuggestions = new ObservableCollection<string>();
            SearchHistory = new ObservableCollection<string>();
            OnPropertyChanged(nameof(IsHistoryEmpty));
            SearchRecommendations = new ObservableCollection<string>();
            OnPropertyChanged(nameof(IsRecommendationsEmpty));

            SearchedValue = string.Empty; // Initialize searched value to empty  

            // Initialize commands for user actions
            ExitCommand = new RelayCommand(() => Services.NavigationService.GetInstance().ExitApplication()); // Command to exit the application
            NavigateToHomeCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.Home));
            NavigateToReportIssuesCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.ReportIssues));
            NavigateToRequestStatusCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.RequestStatus));

            FilterCommand = new RelayCommand(FilterEventsAndAnnouncements); // Command to filter events and announcements
            SearchCommand = new RelayCommand(PerformSearch); // Command to perform a search

            // Asynchronously load events and announcements data
            _ = LoadEventAndAnnouncementDataAsync();
        }


        /// <summary>
        /// Asynchronously loads event and announcement data from the repository.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task LoadEventAndAnnouncementDataAsync()
        {
            try
            {
                // Set the flags to false initially, as we're starting to load data.
                NoEventResultsFound = false;
                NoAnnouncementResultsFound = false;

                // Await the asynchronous method to load events and announcements from the repository.
                await _repository.LoadEventsAndAnnouncementsAsync();

                // Populate the local collections with the loaded data.
                LoadAllEventsAndAnnouncements();
            }
            catch (Exception e)
            {
                // Show an error message if data loading fails.
                MessageBox.Show(Constants.ErrorMessages.ErrorLoadingEventsAndAnnoucements + e.Message);
            }
        }

        /// <summary>
        /// Loads all events and announcements into their respective collections.
        /// </summary>
        private void LoadAllEventsAndAnnouncements()
        {
            // Load all events using the ClearAndPopulate method for better management.
            ClearAndPopulate(Events, _repository.SortedEvents.Values.SelectMany(list => list));

            // Load all announcements similarly.
            ClearAndPopulate(Announcements, _repository.SortedAnnouncements.Values.SelectMany(list => list));

            // Update the flags based on whether events and announcements are present.
            NoEventResultsFound = !Events.Any();
            NoAnnouncementResultsFound = !Announcements.Any();
        }

        /// <summary>
        /// Clears the specified collection and populates it with new items.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="collection">The observable collection to be cleared and populated.</param>
        /// <param name="items">The items to add to the collection.</param>
        private void ClearAndPopulate<T>(ObservableCollection<T> collection, IEnumerable<T> items)
        {
            // Clear existing items in the collection.
            collection.Clear();

            // Add each item from the source to the collection.
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Performs a search based on the user input.
        /// </summary>
        public void PerformSearch()
        {
            // Track the user's search term for analysis and suggestions.
            TrackUserSearch(SearchedValue); // Track search only when user performs search

            // Filter events and announcements based on the search term.
            FilterEventsAndAnnouncements();

            // Update history and suggestions based on the current search.
            UpdateSearchHistory();
            UpdateSearchSuggestions();
            UpdateRecommendations();
        }

        /// <summary>
        /// Filters events and announcements based on the current search value.
        /// </summary>
        public async void FilterEventsAndAnnouncements()
        {
            // Clear previous results.
            Events.Clear();
            Announcements.Clear();

            // If the search value is not empty, apply filtering.
            if (!string.IsNullOrEmpty(SearchedValue))
            {
                // Filter the events and announcements collections.
                FilterCollection(Events, _repository.SortedEvents, ev => ev.Title);
                FilterCollection(Announcements, _repository.SortedAnnouncements, ann => ann.Title);
            }
            else
            {
                // If the search value is empty, reload all events and announcements.
                LoadAllEventsAndAnnouncements();
            }

            // Set the flags based on whether any results are found.
            NoEventResultsFound = !Events.Any();
            NoAnnouncementResultsFound = !Announcements.Any();
        }

        /// <summary>
        /// Filters a collection based on the provided source and search criteria.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="collection">The observable collection to be filtered.</param>
        /// <param name="source">The source dictionary containing items to filter.</param>
        /// <param name="getTitle">A function to extract the title from an item.</param>
        private void FilterCollection<T>(ObservableCollection<T> collection, IDictionary<string, List<T>> source, Func<T, string> getTitle)
        {
            // Iterate through each category in the source dictionary.
            foreach (var category in source.Keys)
            {
                // Check if the category matches the search term.
                if (category.IndexOf(SearchedValue, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Add all items from the matched category to the collection.
                    foreach (var item in source[category])
                    {
                        collection.Add(item);
                    }
                }
                else
                {
                    // If the category doesn't match, check each item's title.
                    foreach (var item in source[category])
                    {
                        if (getTitle(item).IndexOf(SearchedValue, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            collection.Add(item); // Add matching items to the collection.
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tracks the user's search term for recommendations and history.
        /// </summary>
        /// <param name="search">The search term entered by the user.</param>
        private void TrackUserSearch(string search)
        {
            // Exit if the search term is null or empty.
            if (string.IsNullOrEmpty(search)) return;

            // Update the frequency of the search term in the history dictionary.
            _searchHistoryFrequency[search] = _searchHistoryFrequency.ContainsKey(search)
                ? _searchHistoryFrequency[search] + 1 // Increment frequency if it already exists.
                : 1; // Otherwise, initialize it to 1.

            // Update the search history stack while maintaining a maximum size.
            _searchHistoryStack = new Stack<string>(
                _searchHistoryStack.Where(s => s != search).Prepend(search).Take(3).Reverse()
            );

            // Refresh the search history and suggestions based on the updated stack.
            UpdateSearchHistory();
            UpdateSearchSuggestions();
        }

        /// <summary>
        /// Updates the search history collection to reflect recent searches.
        /// </summary>
        public void UpdateSearchHistory()
        {
            // Clear the existing search history.
            SearchHistory.Clear();

            // Reverse the stack to show the most recent searches at the top.
            foreach (var history in _searchHistoryStack)
            {
                SearchHistory.Add(history);
            }

            // Notify the UI that the height of the history has changed.
            OnPropertyChanged(nameof(HistoryHeights));
            OnPropertyChanged(nameof(IsHistoryEmpty));
        }

        /// <summary>
        /// Gets the heights of the search history based on the number of items.
        /// </summary>
        private double historyHeights;
        public double HistoryHeights
        {
            get
            {
                // Determine the height based on the count of search history items.
                switch (SearchHistory.Count)
                {
                    case 0:
                        return 0; // No items.
                    case 1:
                        return 30; // Height for one item (adjust as needed).
                    case 2:
                        return 50; // Height for two items (adjust as needed).
                    case 3:
                        return 70; // Height for three items (adjust as needed).
                    default:
                        return 0; // Default case if there are more than three.
                }
            }
        }

        /// <summary>
        /// Updates the search suggestions based on similar items.
        /// </summary>
        private void UpdateSearchSuggestions()
        {
            // Get similar items based on the current search value.
            var similarItems = GetSimilarEventsAndAnnouncements(SearchedValue)
                        .Where(item => !item.Equals(SearchedValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();

            // Clear existing suggestions.
            SearchSuggestions.Clear();

            // Add similar items to the suggestions list if not already present.
            foreach (var item in similarItems)
            {
                if (!SearchSuggestions.Contains(item))
                {
                    SearchSuggestions.Add(item);
                }
            }
        }

        /// <summary>
        /// Retrieves similar events and announcements based on the search term.
        /// </summary>
        /// <param name="searchTerm">The term used to search for similar items.</param>
        /// <returns>A collection of similar event and announcement titles.</returns>
        private IEnumerable<string> GetSimilarEventsAndAnnouncements(string searchTerm)
        {
            var results = new HashSet<string>();

            // If the search term is null or empty, return empty results.
            if (string.IsNullOrEmpty(searchTerm))
                return results;

            // Search through sorted events for matches.
            foreach (var category in _repository.SortedEvents.Keys)
            {
                if (category.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(category); // Add the matching category.
                }

                // Check each event in the matched category.
                foreach (var ev in _repository.SortedEvents[category])
                {
                    if (ev.Title.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        results.Add(ev.Title); // Add matching event titles.
                    }
                }
            }

            // Repeat the same for sorted announcements.
            foreach (var category in _repository.SortedAnnouncements.Keys)
            {
                if (category.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(category); // Add the matching category.
                }

                foreach (var ann in _repository.SortedAnnouncements[category])
                {
                    if (ann.Title.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        results.Add(ann.Title); // Add matching announcement titles.
                    }
                }
            }

            // Return the results ordered alphabetically.
            return results.OrderBy(r => r);
        }

        /// <summary>
        /// Updates the search recommendations based on user search history and frequency.
        /// </summary>
        public void UpdateRecommendations()
        {
            SearchRecommendations.Clear(); // Clear existing recommendations.

            // Get the top frequency searches from history.
            var frequencySuggestions = GetFrequencyRecommendedSearches();

            // Get recommendations based on frequency and existing events and announcements.
            var recommendedItems = GetRecommendedItems(frequencySuggestions);

            // Add each recommended item to the search recommendations collection.
            foreach (var item in recommendedItems)
            {
                SearchRecommendations.Add(item);
            }

            OnPropertyChanged(nameof(IsRecommendationsEmpty));
        }

        /// <summary>
        /// Retrieves the top frequency searches from the user's search history.
        /// </summary>
        /// <returns>A collection of the most frequent search terms.</returns>
        private IEnumerable<string> GetFrequencyRecommendedSearches()
        {
            return _searchHistoryFrequency
                .OrderByDescending(x => x.Value) // Order by frequency, descending.
                .Select(x => x.Key) // Select the search terms.
                .Distinct() // Ensure uniqueness.
                .Take(5); // Limit to the top 5 terms.
        }

        /// <summary>
        /// Gets recommended items based on the user's search frequency suggestions.
        /// </summary>
        /// <param name="frequencySuggestions">A collection of search terms that the user has frequently searched for.</param>
        /// <returns>A sorted collection of recommended item titles based on their search frequency.</returns>
        private IEnumerable<string> GetRecommendedItems(IEnumerable<string> frequencySuggestions)
        {
            var results = new HashSet<string>(); // Use a HashSet to avoid duplicate recommendations.

            // Iterate over each frequency suggestion to find matching items in events and announcements.
            foreach (var search in frequencySuggestions)
            {
                // Add matching items from the SortedEvents and SortedAnnouncements to results.
                results.UnionWith(GetMatchingItems(search, _repository.SortedEvents));
                results.UnionWith(GetMatchingItems(search, _repository.SortedAnnouncements));
            }

            // Return the results sorted by their frequency in descending order.
            return results.OrderByDescending(item =>
                _searchHistoryFrequency.TryGetValue(item, out var frequency) ? frequency : 0);
        }

        /// <summary>
        /// Gets matching items from the sorted event dictionary based on the search term.
        /// </summary>
        /// <param name="search">The search term to filter events.</param>
        /// <param name="source">The sorted dictionary of events categorized by strings.</param>
        /// <returns>A collection of matching event titles.</returns>
        private IEnumerable<string> GetMatchingItems(string search, SortedDictionary<string, List<Event>> source)
        {
            return source
                // Filter categories that match the search term, ignoring case.
                .Where(category => category.Key.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                // Flatten the list of events that match the search term in their titles.
                .SelectMany(category => category.Value
                    .Where(ev => ev.Title.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    .Select(ev => ev.Title))
                // Include categories that match the search term as well.
                .Concat(source.Keys.Where(c => c.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0));
        }

        /// <summary>
        /// Gets matching items from the sorted announcement dictionary based on the search term.
        /// </summary>
        /// <param name="search">The search term to filter announcements.</param>
        /// <param name="source">The sorted dictionary of announcements categorized by strings.</param>
        /// <returns>A collection of matching announcement titles.</returns>
        private IEnumerable<string> GetMatchingItems(string search, SortedDictionary<string, List<Announcement>> source)
        {
            return source
                // Filter categories that match the search term, ignoring case.
                .Where(category => category.Key.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                // Flatten the list of announcements that match the search term in their titles.
                .SelectMany(category => category.Value
                    .Where(an => an.Title.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    .Select(an => an.Title))
                // Include categories that match the search term as well.
                .Concat(source.Keys.Where(c => c.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0));
        }

        /// <summary>
        /// Event that is triggered when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes the PropertyChanged event for the given property name.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed. If not provided, it uses the caller's member name.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Invoke the PropertyChanged event to notify listeners about the property change.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\