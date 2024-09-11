using CitizenPortalSouthAfrica.Models;
using CitizenPortalSouthAfrica.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Data.SQLite;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
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

        public ICommand NavigateToHomeCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand AttachFilesCommand { get; }
        public ICommand RemoveFileCommand { get; }
        public ICommand ExitCommand { get; }

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

            Category = "Sanitation";

            ExitCommand = new RelayCommand(() => Services.NavigationService.GetInstance().ExitApplication());
            NavigateToHomeCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo("Home"));
            SubmitCommand = new RelayCommand(OnSubmit);
            AttachFilesCommand = new RelayCommand(async () => await AttachFilesAsync());
            RemoveFileCommand = new RelayCommand<string>(OnRemoveFile);
        }

        private async void OnSubmit()
        {
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
            Category = "Sanitation";
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

        private void UpdateFormCompletionPercentage()
        {
            int filledFields = 0;
            int totalFields = 3; // Update this number if you have more fields

            if (!string.IsNullOrWhiteSpace(Location)) filledFields++;
            if (!string.IsNullOrWhiteSpace(Category)) filledFields++;
            if (!string.IsNullOrWhiteSpace(Description)) filledFields++;

            FormCompletionPercentage = (int)((float)filledFields / totalFields * 100);
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
