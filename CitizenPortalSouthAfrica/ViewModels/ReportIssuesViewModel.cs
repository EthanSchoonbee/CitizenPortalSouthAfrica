//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     10/09/2024
 * Last Modified:    17/09/2024
 * 
 * Description:
 * ViewModel for the ReportIssues view in the CitizenPortalSouthAfrica application.
 * Handles data binding, command execution, and form validation for issue reporting.
 * Provides functionality for managing file attachments and updating the form's completion percentage.
 * 
 * Dependencies:
 * - FileManagementService: Manages file operations.
 * - ValidationService: Handles form validation.
 * - ReportIssueRepository: Interacts with data storage for report issues.
 * 
 * Commands:
 * - NavigateToHomeCommand: Navigates to the home view.
 * - SubmitCommand: Submits the issue report form.
 * - AttachFilesCommand: Opens file dialog to attach files.
 * - RemoveFileCommand: Removes a file from the attachment list.
 * - ExitCommand: Closes the application.
 * - ClearCommand: Clears the form inputs and resets the view.
 * 
 * Properties:
 * - Location, Category, Description: Bound to user inputs for reporting issues.
 * - GuideText: Provides contextual help text based on user interactions.
 * - LocationError, CategoryError, DescriptionError: Displays validation error messages.
 * - LocationClicked, CategoryClicked, DescriptionClicked: Flags indicating if fields have been interacted with.
 * - FormCompletionPercentage: Reflects the completion state of the form.
 * - AttachedFilesVisibility: Determines visibility of attached files.
 * 
 * Methods:
 * - OnSubmit: Handles the submission of the issue report.
 * - ValidateForm: Validates the form inputs.
 * - ClearInputs: Resets all form fields and clears attachments.
 * - AttachFilesAsync: Asynchronously attaches files to the issue report.
 * - OnRemoveFile: Removes a file from the list of attachments.
 * - UpdateFormCompletionPercentage: Updates the form completion percentage based on filled fields.
 * - GetGuideText: Returns appropriate guide text based on the form state.
 * - DebounceAction: Handles debounce timing for form updates.
 * - SetField: A helper method to update properties and raise notifications.
 * 
 * Implementation Details:
 * - Uses ObservableCollection for managing file attachments.
 * - Implements INotifyPropertyChanged to support data binding.
 * - Employs MVVM Light’s RelayCommand for command handling.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using CitizenPortalSouthAfrica.Models;
using CitizenPortalSouthAfrica.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CitizenPortalSouthAfrica.ViewModels
{
    public class ReportIssuesViewModel : INotifyPropertyChanged
    {
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        /* 
         * Fields and Properties
         * 
         * Description:
         * This section defines the private fields, public properties, and commands used in the ReportIssuesViewModel class.
         * 
         * Private Fields:
         * - _fileManagementService: Service responsible for managing file operations.
         * - _validationService: Service used for validating form inputs.
         * - _repository: Repository for interacting with data storage related to issue reports.
         * - _location, _category, _description: Fields to store user input for location, category, and description of the report.
         * - _guideText: Provides contextual help text based on user interactions.
         * - _locationError, _categoryError, _descriptionError: Stores validation error messages for each input field.
         * - _locationClicked, _categoryClicked, _descriptionClicked: Flags indicating if a field has been interacted with.
         * - _formCompletionPercentage: Represents the percentage of the form that has been completed based on filled fields.
         * - _debounceTimer: Timer used to handle debounce actions for form updates.
         * 
         * Public Properties:
         * - FileNames: ObservableCollection of file names attached to the report.
         * - FileData: ObservableCollection of file data associated with the attached files.
         * - Location, Category, Description: Properties bound to user input fields for the report.
         * - GuideText: Property to hold the current guide text.
         * - LocationError, CategoryError, DescriptionError: Properties to display validation errors.
         * - LocationClicked, CategoryClicked, DescriptionClicked: Properties to track if fields have been interacted with.
         * - FormCompletionPercentage: Property representing the form's completion percentage.
         * - AttachedFilesVisibility: Property to determine the visibility of the attached files section.
         * 
         * Commands:
         * - NavigateToHomeCommand: Command to navigate back to the home view.
         * - SubmitCommand: Command to submit the issue report.
         * - AttachFilesCommand: Command to open a file dialog for attaching files.
         * - RemoveFileCommand: Command to remove an attached file.
         * - ExitCommand: Command to close the application.
         * - ClearCommand: Command to clear all form inputs and attachments.
         */
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        private readonly FileManagementService _fileManagementService; // Service for file operations
        private readonly ValidationService _validationService; // Service for form validation
        private readonly ReportIssueRepository _repository; // Repository for report issue data

        // Fields to store user inputs and form state
        private string _location; // Location field value
        private string _category; // Category field value
        private string _description; // Description field value
        private string _guideText; // Guide text for contextual help
        private string _locationError; // Error message for location field
        private string _categoryError; // Error message for category field
        private string _descriptionError; // Error message for description field

        private bool _locationClicked; // Flag to check if location field has been interacted with
        private bool _categoryClicked; // Flag to check if category field has been interacted with
        private bool _descriptionClicked; // Flag to check if description field has been interacted with

        public int _formCompletionPercentage; // Percentage of form completion

        private System.Timers.Timer _debounceTimer; // Timer for debounce actions

        // Collections for managing attached files
        public ObservableCollection<string> FileNames { get; set; } // List of file names
        public ObservableCollection<byte[]> FileData { get; set; } // List of file data

        // Location property with validation and update handling
        public string Location
        {
            get => _location; // Get current value of location
            set
            {
                // Update field and raise property changed notification
                SetField(ref _location, value, nameof(Location), UpdateFormCompletionPercentage);
                LocationError = string.Empty; // Clear error message when value changes
            }
        }

        // Category property with validation and update handling
        public string Category
        {
            get => _category; // Get current value of category
            set
            {
                // Update field and raise property changed notification
                SetField(ref _category, value, nameof(Category), UpdateFormCompletionPercentage);
                CategoryError = string.Empty; // Clear error message when value changes
            }
        }

        // Description property with validation and update handling
        public string Description
        {
            get => _description; // Get current value of description
            set
            {
                // Update field and raise property changed notification
                SetField(ref _description, value, nameof(Description), UpdateFormCompletionPercentage);
                DescriptionError = string.Empty; // Clear error message when value changes
            }
        }

        // Guide text property for contextual help
        public string GuideText
        {
            get => _guideText; // Get current guide text
            set => SetField(ref _guideText, value); // Update field and raise property changed notification
        }

        // Error message properties for each input field
        public string LocationError
        {
            get => _locationError; // Get error message for location field
            set => SetField(ref _locationError, value); // Update error message and raise property changed notification
        }

        public string CategoryError
        {
            get => _categoryError; // Get error message for category field
            set => SetField(ref _categoryError, value); // Update error message and raise property changed notification
        }

        public string DescriptionError
        {
            get => _descriptionError; // Get error message for description field
            set => SetField(ref _descriptionError, value); // Update error message and raise property changed notification
        }

        // Flags to track if fields have been interacted with
        public bool LocationClicked
        {
            get => _locationClicked; // Get if location field has been clicked
            set => SetField(ref _locationClicked, value); // Update flag and raise property changed notification
        }

        public bool CategoryClicked
        {
            get => _categoryClicked; // Get if category field has been clicked
            set => SetField(ref _categoryClicked, value); // Update flag and raise property changed notification
        }

        public bool DescriptionClicked
        {
            get => _descriptionClicked; // Get if description field has been clicked
            set => SetField(ref _descriptionClicked, value); // Update flag and raise property changed notification
        }

        // Form completion percentage based on filled fields
        public int FormCompletionPercentage
        {
            get => _formCompletionPercentage; // Get form completion percentage
            set => SetField(ref _formCompletionPercentage, value, nameof(FormCompletionPercentage)); // Update percentage and raise property changed notification
        }

        // Visibility of the attached files section
        public Visibility AttachedFilesVisibility
        {
            get => FileNames.Any() ? Visibility.Visible : Visibility.Collapsed; // Determine visibility based on the presence of attached files
        }

        // Updates visibility of attached files section when files are added or removed
        private void OnFileNamesChanged()
        {
            OnPropertyChanged(nameof(AttachedFilesVisibility)); // Notify of change in file names to update visibility
        }

        // Commands for user actions
        public ICommand NavigateToHomeCommand { get; } // Command to navigate to the home view
        public ICommand NavigateToEventsAndAnnouncementsCommand { get; } // Command to navigate to the home view
        public ICommand NavigateToRequestStatusCommand { get; } // Command to navigate to the request status view
        public ICommand SubmitCommand { get; } // Command to submit the issue report
        public ICommand AttachFilesCommand { get; } // Command to attach files
        public ICommand RemoveFileCommand { get; } // Command to remove an attached file
        public ICommand ExitCommand { get; } // Command to exit the application
        public ICommand ClearCommand { get; } // Command to clear form inputs and attachments

        // Constructor initializing services and commands
        public ReportIssuesViewModel()
        {
            // Initialize services and repository
            _fileManagementService = new FileManagementService();
            _validationService = new ValidationService();
            _repository = new ReportIssueRepository();

            // Initialize collections
            FileNames = new ObservableCollection<string>();
            FileData = new ObservableCollection<byte[]>();

            // Set up debounce timer
            _debounceTimer = new System.Timers.Timer(1000);
            _debounceTimer.Elapsed += (sender, args) => DebounceAction(); // Attach debounce action to timer

            // Set initial guide text
            GuideText = Constants.GuideText.InitialGuide;

            // Initialize commands
            ExitCommand = new RelayCommand(() => Services.NavigationService.GetInstance().ExitApplication()); // Command to exit the application
            NavigateToHomeCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.Home));
            NavigateToEventsAndAnnouncementsCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.EventsAndAnnouncements));
            NavigateToRequestStatusCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo(Constants.NavigationHeaders.RequestStatus));

            SubmitCommand = new RelayCommand(OnSubmit); // Command to submit the report
            AttachFilesCommand = new RelayCommand(async () => await AttachFilesAsync()); // Command to attach files
            RemoveFileCommand = new RelayCommand<string>(OnRemoveFile); // Command to remove a file
            ClearCommand = new RelayCommand(ClearInputs); // Command to clear inputs
        }


        // O(1) complexity: Submits the report after validating the form
        private async void OnSubmit()
        {
            // Validate the form before proceeding
            if (!ValidateForm())
            {
                return;
            }

            // Create a new ReportIssue object with the provided data
            var reportIssue = new ReportIssue
            {
                Location = Location,
                Category = Category,
                Description = Description,
                Files = FileData.ToList()
            };

            // Save the report to the repository asynchronously
            await _repository.AddReportIssueAsync(reportIssue);

            // Clear the input fields after submission
            ClearInputs();

            // Notify the user that the report was saved successfully
            MessageBox.Show(Constants.SuccessMessages.IssueSaved, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Validates the form fields and sets error messages if validation fails
        private bool ValidateForm()
        {
            // Validate the form fields using the ValidationService
            var isValid = _validationService.ValidateForm(Location, Category, Description,
                          out var locationError, out var categoryError, out var descriptionError);

            // Set error messages for invalid fields
            LocationError = locationError;
            CategoryError = categoryError;
            DescriptionError = descriptionError;

            // Return the result of the validation
            return isValid;
        }

        // O(1) complexity: Clears all input fields and attached files
        private void ClearInputs()
        {
            // Reset location, category, and description fields
            Location = string.Empty;
            Category = string.Empty;
            Description = string.Empty;

            // Clear the file collections
            FileNames.Clear();
            FileData.Clear();

            // Update UI based on the changes to attached files
            OnFileNamesChanged();

            // Reset the guide text to the initial state
            GuideText = Constants.GuideText.InitialGuide;
        }

        // O(n) complexity: Asynchronously attaches files and updates the file collections
        private async Task AttachFilesAsync()
        {
            // Use the FileManagementService to retrieve files and their names
            var (files, fileNames) = await _fileManagementService.AttachFilesAsync();

            // If files were selected, update the FileData and FileNames collections
            if (files.Any())
            {
                // Add each file to the FileData collection
                foreach (var file in files)
                {
                    FileData.Add(file);
                }

                // Add each filename to the FileNames collection
                foreach (var fileName in fileNames)
                {
                    FileNames.Add(fileName);
                }

                // Notify that the attached files section has changed
                OnFileNamesChanged();
            }
        }

        // O(1) complexity: Removes a file from the attached files list based on its name
        private void OnRemoveFile(string fileName)
        {
            // Find the index of the file in the FileNames collection
            var index = FileNames.IndexOf(fileName);

            // If the file exists, remove it from both the FileNames and FileData collections
            if (index >= 0)
            {
                FileNames.RemoveAt(index);
                FileData.RemoveAt(index);

                // Update the UI to reflect the changes
                OnFileNamesChanged();
            }
        }

        // O(1) complexity: Updates the percentage of form completion based on filled fields
        public void UpdateFormCompletionPercentage()
        {
            const int numberOfFields = 3;

            // Count the number of non-empty fields (location, category, description)
            var filledFields = new[] { Location, Category, Description }
                .Count(field => !string.IsNullOrWhiteSpace(field));

            // Calculate the form completion percentage
            FormCompletionPercentage = (int)((float)filledFields / numberOfFields * 100);

            // Update the guide text based on form completion
            GuideText = GetGuideText();
        }

        // Returns the appropriate guide text based on form completion and clicked fields
        private string GetGuideText()
        {
            // If the form is fully completed, show the completion guide
            if (FormCompletionPercentage == 100)
            {
                return Constants.GuideText.CompletionGuide;
            }

            // Display specific guide text based on which field has been clicked
            if (LocationClicked)
            {
                return Constants.GuideText.LocationGuide;
            }
            if (CategoryClicked)
            {
                return Constants.GuideText.CategoryGuide;
            }
            if (DescriptionClicked)
            {
                return Constants.GuideText.DescriptionGuide;
            }

            // Return the default guide text if none of the fields were clicked
            return GuideText;
        }

        // O(1) complexity: Debounce method to delay actions by resetting the timer
        private void DebounceAction()
        {
            // Stop and restart the debounce timer
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        // Helper method to update a field's value, trigger property changes, and invoke additional actions
        private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null, Action onChanged = null)
        {
            // Only update the field if the value has changed
            if (!Equals(field, value))
            {
                field = value;

                // Notify the UI that the property has changed
                OnPropertyChanged(propertyName);

                // Invoke any additional actions (e.g., updating form completion)
                onChanged?.Invoke();
            }
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
