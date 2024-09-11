using CitizenPortalSouthAfrica.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace CitizenPortalSouthAfrica.ViewModels
{
    public class ReportIssuesViewModel : ViewModelBase
    {
        private string _location;
        private string _category;
        private string _description;
        private ObservableCollection<string> _fileNames;
        private List<byte[]> _fileData;


        public string Location
        {
            get => _location;
            set => Set(ref _location, value);
        }

        public string Category
        {
            get => _category;
            set => Set(ref _category, value);
        }

        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        public ObservableCollection<string> FileNames
        {
            get => _fileNames;
            set
            {
                _fileNames = value;
                Set(ref _fileNames, value);
            }
        }

        public List<byte[]> FileData
        {
            get => _fileData;
            set => Set(ref _fileData, value);
        }

        public ICommand NavigateToHomeCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand AttachFilesCommand { get; }
        public ICommand ExitCommand { get; }

        public ReportIssuesViewModel()
        {
            FileNames = new ObservableCollection<string>();
            FileData = new List<byte[]>();

            Category = "Sanitation";

            ExitCommand = new RelayCommand(() => Services.NavigationService.GetInstance().ExitApplication());
            NavigateToHomeCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo("Home"));
            SubmitCommand = new RelayCommand(OnSubmit);
            AttachFilesCommand = new RelayCommand(async () => await AttachFilesAsync());
        }

        private void OnSubmit()
        {
            var reportIssue = new ReportIssue
            {
                Location = Location,
                Category = Category,
                Description = Description,
                Files = FileData
            };

            MessageBox.Show("Saving the report issue...\n\n");

            // Save to database or perform other actions
        }

        private async Task AttachFilesAsync()
        {
            bool filesProcessed = false;

            while (!filesProcessed)
            {
                var openFileDialog = new OpenFileDialog
                {
                    Multiselect = true,
                    Filter = "Image Files|*.jpg;*.jpeg;*.png|Document Files|*.pdf;*.docx|All Files|*.*",
                    FilterIndex = 3
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        // Use a list to temporarily hold the valid file data
                        var validFiles = new List<byte[]>();
                        bool hasError = false;

                        foreach (var filePath in openFileDialog.FileNames)
                        {
                            if (!IsValidFileType(filePath))
                            {
                                MessageBox.Show($"File '{filePath}' is not a supported type. Please upload an image or document.", "File Type Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                hasError = true;
                                break;
                            }

                            if (!IsValidFileSize(filePath))
                            {
                                MessageBox.Show($"File '{Path.GetFileName(filePath)}' is too large. Maximum allowed size is 25 MB.", "File Size Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                hasError = true;
                                break;
                            }

                            try
                            {
                                byte[] fileData = await Task.Run(() => File.ReadAllBytes(filePath));
                                validFiles.Add(fileData);
                                FileNames.Add(Path.GetFileName(filePath));
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error reading file '{filePath}': {ex.Message}", "File Read Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                hasError = true;
                                break;
                            }
                        }

                        // Only update the collection if all files are processed successfully
                        if (!hasError)
                        {
                            FileData.Clear();
                            FileData.AddRange(validFiles);
                            filesProcessed = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while attaching files: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        filesProcessed = false;
                    }
                }
                else
                {
                    filesProcessed = true; // User cancelled the dialog
                }
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
    }
}
