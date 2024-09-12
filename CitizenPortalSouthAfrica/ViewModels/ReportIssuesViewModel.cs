using CitizenPortalSouthAfrica.Models;
using CitizenPortalSouthAfrica.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.IO;
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

        readonly FileManagementService _fileManagementService;

        private string _location;
        private string _category;
        private string _description;
        private ObservableCollection<string> _fileNames;
        private ObservableCollection<byte[]> _fileData;
        private int _formCompletionPercentage;
        private string _guideText;
        private bool _locationClicked;
        private bool _categoryClicked;
        private bool _descriptionClicked;
        private string _locationError;
        private string _categoryError;
        private string _descriptionError;


        private System.Timers.Timer _debounceTimer;

        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged();
                UpdateFormCompletionPercentage();
            }
        }

        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
                UpdateFormCompletionPercentage();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
                UpdateFormCompletionPercentage();
            }
        }

        public ObservableCollection<string> FileNames
        {
            get => _fileNames;
            set
            {
                _fileNames = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<byte[]> FileData
        {
            get => _fileData;
            set
            {
                _fileData = value;
                OnPropertyChanged();
            }
        }

        public int FormCompletionPercentage
        {
            get => _formCompletionPercentage;
            set
            {
                _formCompletionPercentage = value;
                OnPropertyChanged();
            }
        }
        public string GuideText
        {
            get => _guideText;
            set
            {
                _guideText = value;
                OnPropertyChanged(nameof(GuideText));
            }
        }

        public bool LocationClicked
        {
            get => _locationClicked;
            set
            {
                _locationClicked = value;
                OnPropertyChanged();
            }
        }

        public bool CategoryClicked
        {
            get => _categoryClicked;
            set
            {
                _categoryClicked = value;
                OnPropertyChanged();
            }
        }

        public bool DescriptionClicked
        {
            get => _descriptionClicked;
            set
            {
                _descriptionClicked = value;
                OnPropertyChanged();
            }
        }

        public string LocationError
        {
            get => _locationError;
            set
            {
                _locationError = value;
                OnPropertyChanged();
            }
        }

        public string CategoryError
        {
            get => _categoryError;
            set
            {
                _categoryError = value;
                OnPropertyChanged();
            }
        }

        public string DescriptionError
        {
            get => _descriptionError;
            set
            {
                _descriptionError = value;
                OnPropertyChanged();
            }
        }

        public ICommand NavigateToHomeCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand AttachFilesCommand { get; }
        public ICommand RemoveFileCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand ClearCommand { get; }

        public ReportIssuesViewModel()
        {
            _fileManagementService = new FileManagementService();

            FileNames = new ObservableCollection<string>();
            FileData = new ObservableCollection<byte[]>();

            _debounceTimer = new System.Timers.Timer(1000);
            _debounceTimer.Elapsed += (sender, args) =>
            {
                _debounceTimer.Stop();
                UpdateFormCompletionPercentage();
            };

            GuideText = "Hi, I'm Guru your local guide!\nBelow is the form for submitting issues.\nClick on the fields to get guidance on what to do.\nStart by filling in the Location of your issue below:";

            ExitCommand = new RelayCommand(() => Services.NavigationService.GetInstance().ExitApplication());
            NavigateToHomeCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo("Home"));
            SubmitCommand = new RelayCommand(OnSubmit);
            AttachFilesCommand = new RelayCommand(async () => await AttachFilesAsync());
            RemoveFileCommand = new RelayCommand<string>(OnRemoveFile);
            ClearCommand = new RelayCommand(ClearInputs);
        }

        private async void OnSubmit()
        {
            LocationError = string.Empty;
            CategoryError = string.Empty;
            DescriptionError = string.Empty;

            if (string.IsNullOrWhiteSpace(Location))
            {
                LocationError = "Location is required.";
            }

            if (string.IsNullOrWhiteSpace(Category))
            {
                CategoryError = "Please select a category.";
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                DescriptionError = "Description is required.";
            }

            // If there are any errors, we return early to avoid submitting the form
            if (!string.IsNullOrWhiteSpace(LocationError) ||
                !string.IsNullOrWhiteSpace(CategoryError) ||
                !string.IsNullOrWhiteSpace(DescriptionError))
            {
                return;
            }

            var reportIssue = new ReportIssue
            {
                Location = Location,
                Category = Category,
                Description = Description,
                Files = FileData.ToList()
            };

            // Save to the local SQLite database using the DbContext
            var repository = new ReportIssueRepository();
            await repository.AddReportIssueAsync(reportIssue);

            ClearInputs();

            MessageBox.Show("Issue report saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information); 
        }

        private void ClearInputs()
        {
            Location = string.Empty;
            Category = "";
            Description = string.Empty;
            FileNames.Clear();
            FileData.Clear();
        }

        private async Task AttachFilesAsync()
        {
            var (files, fileNames) = await _fileManagementService.AttachFilesAsync();

            // Update collections only if files are processed successfully
            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    FileData.Add(file);
                }

                foreach (var fileName in fileNames)
                {
                    FileNames.Add(fileName);
                }
            }
        }

        private void OnRemoveFile(string fileName)
        {
            var index = FileNames.IndexOf(fileName);
            if (index >= 0)
            {
                FileNames.RemoveAt(index);
                FileData.RemoveAt(index);
            }
        }

        private bool IsValidFileType(string filePath)
        {
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".docx" };
            return validExtensions.Any(ext => filePath.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsValidFileSize(string filePath)
        {
            var maxSize = 10 * 1024 * 1024; // 10 MB
            return new FileInfo(filePath).Length <= maxSize;
        }

        public void UpdateFormCompletionPercentage()
        {
            if (LocationClicked)
            {
                GuideText = "Please enter the location where the issue occurred.";
            }
            else if (CategoryClicked)
            {
                GuideText = "Select the category that best describes the issue.";
            }
            else if (DescriptionClicked)
            {
                GuideText = "Provide a detailed description of the issue.";
            }

            // Now calculate form completion percentage based on field values
            int filledFields = 0;
            int totalFields = 3;

            if (!string.IsNullOrWhiteSpace(Location)) filledFields++;
            if (!string.IsNullOrWhiteSpace(Category)) filledFields++;
            if (!string.IsNullOrWhiteSpace(Description)) filledFields++;

            FormCompletionPercentage = (int)((float)filledFields / totalFields * 100);

            if (FormCompletionPercentage == 100)
            {
                GuideText = "Well done! You can now add relevant pictures or documents if needed and submit the form!";
            }
        }

        private void DebounceUpdate()
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
