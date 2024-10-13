using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CitizenPortalSouthAfrica.Models;
using CitizenPortalSouthAfrica.Services;
using GalaSoft.MvvmLight.Command;

namespace CitizenPortalSouthAfrica.ViewModels
{
    public class EventsAndAnoncementsViewModel : INotifyPropertyChanged
    {
        private readonly EventAndAnnouncementRepository _repository;

        public ObservableCollection<Event> Events { get; private set; }
        public ObservableCollection<Announcement> Announcements { get; private set; }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                    FilterEventsAndAnnouncements(); // Call the filter method when the category changes
                }
            }
        }

        private bool _noEventResultsFound;
        public bool NoEventResultsFound
        {
            get => _noEventResultsFound;
            set
            {
                if (_noEventResultsFound != value)
                {
                    _noEventResultsFound = value;
                    OnPropertyChanged(nameof(NoEventResultsFound)); // Notify property change for data binding
                }
            }
        }

        private bool _noAnnouncementResultsFound;
        public bool NoAnnouncementResultsFound
        {
            get => _noAnnouncementResultsFound;
            set
            {
                if (_noAnnouncementResultsFound != value)
                {
                    _noAnnouncementResultsFound = value;
                    OnPropertyChanged(nameof(NoAnnouncementResultsFound)); // Notify property change for data binding
                }
            }
        }

        // Commands for user actions
        public ICommand NavigateToHomeCommand { get; } // Command to navigate to the home view
        public ICommand NavigateToReportIssuesCommand { get; } // Command to navigate to the report issues view
        public ICommand ExitCommand { get; } // Command to exit the application
        public ICommand NavigateToRequestStatusCommand { get; }
        public ICommand FilterCommand { get; }
        
        public EventsAndAnoncementsViewModel()
        {
            _repository = new EventAndAnnouncementRepository();

            Events = new ObservableCollection<Event>();
            Announcements = new ObservableCollection<Announcement>();

            SelectedCategory = string.Empty;

            // Initialize commands
            ExitCommand = new RelayCommand(() => Services.NavigationService.GetInstance().ExitApplication()); // Command to exit the application
            NavigateToHomeCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo("Home")); // Command to navigate to home
            NavigateToReportIssuesCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo("ReportIssues")); // Command to navigate to report issues
            NavigateToRequestStatusCommand = new RelayCommand(() => MessageBox.Show("This feature is currently under development. Please check back later!",
                                                                                    "Feature Not Available",
                                                                                    MessageBoxButton.OK,
                                                                                    MessageBoxImage.Exclamation));
            FilterCommand = new RelayCommand(FilterEventsAndAnnouncements);

            _ = LoadEventAndAnnouncementDataAsync();
        }

        private void FilterEventsAndAnnouncements()
        {
            Events.Clear();
            Announcements.Clear();

            // Check if the selected category is not empty
            if (!string.IsNullOrEmpty(SelectedCategory))
            {
                bool eventsFound = false;
                bool announcementsFound = false;

                // Filter events based on similar categories
                foreach (var category in _repository.SortedEvents.Keys)
                {
                    if (category.IndexOf(SelectedCategory, StringComparison.OrdinalIgnoreCase) >= 0) // Check for similarity
                    {
                        foreach (var ev in _repository.SortedEvents[category])
                        {
                            Events.Add(ev);
                        }
                        eventsFound = true;
                    }
                }

                // Filter announcements based on similar categories
                foreach (var category in _repository.SortedAnnouncements.Keys)
                {
                    if (category.IndexOf(SelectedCategory, StringComparison.OrdinalIgnoreCase) >= 0) // Check for similarity
                    {
                        foreach (var ann in _repository.SortedAnnouncements[category])
                        {
                            Announcements.Add(ann);
                        }
                        announcementsFound = true;
                    }
                }

                NoEventResultsFound = !eventsFound;
                NoAnnouncementResultsFound = !announcementsFound;
            }
            else // If no category is selected, show all events and announcements
            {
                LoadAllEventsAndAnnouncements();
                NoEventResultsFound = false;
                NoAnnouncementResultsFound = false;
            }
        }

        private void LoadAllEventsAndAnnouncements()
        {
            // Clear current collections
            Events.Clear();
            Announcements.Clear();

            // Load all events
            foreach (var category in _repository.SortedEvents.Keys)
            {
                foreach (var ev in _repository.SortedEvents[category])
                {
                    Events.Add(ev);
                }
            }

            // Load all announcements
            foreach (var category in _repository.SortedAnnouncements.Keys)
            {
                foreach (var ann in _repository.SortedAnnouncements[category])
                {
                    Announcements.Add(ann);
                }
            }
        }

        public async Task LoadEventAndAnnouncementDataAsync()
        {
            Events.Clear();
            Announcements.Clear();

            try
            {
                await _repository.LoadEventsAndAnnouncementsAsync();

                foreach (var ev in _repository.SortedEvents.Values.SelectMany(list => list))
                {
                    Events.Add(ev);
                }

                foreach (var announcement in _repository.SortedAnnouncements.Values.SelectMany(list => list))
                {
                    Announcements.Add(announcement);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading data: " + e.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
