using CitizenPortalSouthAfrica.Resources;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CitizenPortalSouthAfrica.Services
{
    public class FileManagementService : IFileManagementService
    {
        public async Task<(List<byte[]> Files, List<string> FileNames)> AttachFilesAsync()
        {
            var validFiles = new List<byte[]>();
            var validFileNames = new List<string>();

            var openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image Files|*.jpg;*.jpeg;*.png|Document Files|*.pdf;*.docx|All Files|*.*",
                FilterIndex = 3
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var filePath in openFileDialog.FileNames)
                {
                    if (!IsValidFileType(filePath))
                    {
                        MessageBox.Show($"File '{filePath}' is not a supported type. Please upload an image or document.", "File Type Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        continue;
                    }

                    if (!IsValidFileSize(filePath))
                    {
                        MessageBox.Show($"File '{Path.GetFileName(filePath)}' is too large. Maximum allowed size is 25 MB.", "File Size Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        continue;
                    }

                    try
                    {
                        byte[] fileData = await Task.Run(() => File.ReadAllBytes(filePath));
                        validFiles.Add(fileData);
                        validFileNames.Add(Path.GetFileName(filePath));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error reading file '{filePath}': {ex.Message}", "File Read Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

            return (validFiles, validFileNames);
        }

        private bool IsValidFileType(string filePath)
        {
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".docx" };
            return validExtensions.Any(ext => filePath.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsValidFileSize(string filePath)
        {
            var maxSize = 25 * 1024 * 1024; // 25 MB
            return new FileInfo(filePath).Length <= maxSize;
        }

    }
}
