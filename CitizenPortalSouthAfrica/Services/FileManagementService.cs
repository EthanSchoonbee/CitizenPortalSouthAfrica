//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     10/09/2024
 * Last Modified:    17/09/2024
 * 
 * Description:
 * This service class handles file management operations in the CitizenPortalSouthAfrica application. 
 * It allows users to attach files to their reports with validation on file types and sizes. The service 
 * supports reading files asynchronously, validating file formats, and notifying users of errors.
 * 
 * File restrictions:
 * - Maximum file size: 25 MB
 * - Supported file types: .jpg, .jpeg, .png, .pdf, .docx
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using CitizenPortalSouthAfrica.Resources;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CitizenPortalSouthAfrica.Services
{
    /// <summary>
    /// Service for handling file attachments, including validation and file reading.
    /// Supports attaching multiple files to reports and ensures only valid files are accepted.
    /// </summary>
    public class FileManagementService : IFileManagementService
    {
        // Define maximum allowed file size (25 MB) and valid file extensions
        private const int MaxFileSize = 25 * 1024 * 1024; // 25 MB
        private readonly string[] ValidExtensions = {
            ".jpg", ".jpeg", ".png", ".pdf", ".docx",
            ".mp4", ".mkv", ".txt", ".xlsx", ".pptx", ".zip"
        };

        /// <summary>
        /// Asynchronous task to allow users to attach and validate files for report issues.
        /// The method opens a file dialog, validates each selected file, and returns a list of valid files 
        /// and their names as byte arrays.
        /// </summary>
        /// <returns>
        /// A tuple containing:
        /// - List of valid files as byte arrays.
        /// - List of valid file names.
        /// </returns>
        public async Task<(List<byte[]> Files, List<string> FileNames)> AttachFilesAsync()
        {
            // Lists to store valid files and file names
            var validFiles = new List<byte[]>();
            var validFileNames = new List<string>();

            // Open the file dialog to allow file selection
            var openFileDialog = CreateOpenFileDialog();

            // Check if the user selected files and validate each one
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var filePath in openFileDialog.FileNames)
                {
                    if (TryValidateFile(filePath, out var errorMessage))
                    {
                        try
                        {
                            // Read and convert the selected file into a byte array
                            byte[] fileData = await ReadFileAsync(filePath);
                            validFiles.Add(fileData);
                            validFileNames.Add(Path.GetFileName(filePath));
                        }
                        catch (IOException ex)
                        {
                            // Display an error message if there's an issue reading the file
                            MessageBox.Show($"Error reading file '{filePath}': {ex.Message}", "File Read Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        // Display a warning if the file doesn't pass validation
                        MessageBox.Show(errorMessage, "File Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }

            // Return the lists of valid files and file names
            return (validFiles, validFileNames);
        }

        /// <summary>
        /// Creates and returns an OpenFileDialog instance with predefined filter options.
        /// Supports selecting multiple files and filters based on file types.
        /// </summary>
        /// <returns>An OpenFileDialog object.</returns>
        private OpenFileDialog CreateOpenFileDialog()
        {
            // Set the dialog properties for file selection
            return new OpenFileDialog
            {
                Multiselect = true,  // Allows multiple file selection
                Filter = "Image Files|*.jpg;*.jpeg;*.png|Document Files|*.pdf;*.docx|Text Files|*.txt|Excel Files|*.xlsx|PowerPoint Files|*.pptx|Video Files|*.mp4;*.mkv|Zip Files|*.zip|All Files|*.*",
                FilterIndex = 3
            };
        }

        /// <summary>
        /// Validates a file by checking its type and size. Returns true if the file is valid.
        /// Otherwise, provides an error message.
        /// </summary>
        /// <param name="filePath">Path of the file to validate.</param>
        /// <param name="errorMessage">Error message to return if validation fails.</param>
        /// <returns>True if the file is valid, otherwise false.</returns>
        private bool TryValidateFile(string filePath, out string errorMessage)
        {
            // Check if the file has a valid extension
            if (!IsValidFileType(filePath))
            {
                errorMessage = $"File '{filePath}' is not a supported type. Please upload an image or document.";
                return false;
            }

            // Check if the file size is within the allowed limit
            if (!IsValidFileSize(filePath))
            {
                errorMessage = $"File '{Path.GetFileName(filePath)}' is too large. Maximum allowed size is 25 MB.";
                return false;
            }

            errorMessage = null;
            return true;
        }

        /// <summary>
        /// Checks if the selected file has a valid extension (jpg, jpeg, png, pdf, docx).
        /// </summary>
        /// <param name="filePath">Path of the file to check.</param>
        /// <returns>True if the file has a valid extension, otherwise false.</returns>
        private bool IsValidFileType(string filePath)
        {
            return ValidExtensions.Any(ext => filePath.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Validates the file size to ensure it does not exceed the 25 MB limit.
        /// </summary>
        /// <param name="filePath">Path of the file to check.</param>
        /// <returns>True if the file size is within the limit, otherwise false.</returns>
        private bool IsValidFileSize(string filePath)
        {
            return new FileInfo(filePath).Length <= MaxFileSize;
        }

        /// <summary>
        /// Asynchronously reads the contents of a file and returns it as a byte array.
        /// </summary>
        /// <param name="filePath">Path of the file to read.</param>
        /// <returns>A byte array representing the file's contents.</returns>
        private async Task<byte[]> ReadFileAsync(string filePath)
        {
            // Reads the file's bytes asynchronously to avoid blocking the UI thread
            return await Task.Run(() => File.ReadAllBytes(filePath));
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\
