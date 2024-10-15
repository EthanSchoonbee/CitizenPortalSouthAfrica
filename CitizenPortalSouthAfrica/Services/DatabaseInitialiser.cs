//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     10/09/2024
 * Last Modified:    17/09/2024
 * 
 * Description:
 * This service class is responsible for initializing the SQLite database required for the CitizenPortalSouthAfrica 
 * application. It ensures that the database is created if it does not already exist and sets up the necessary 
 * tables to support report issues and file attachments. The class handles the creation of the `ReportIssues` 
 * and `ReportIssueFiles` tables, including their schema definitions.
 * 
 * Tables Created:
 * - ReportIssues: Stores information about reported issues, including location, category, and description.
 * - ReportIssueFiles: Stores files attached to reports, linking them to the corresponding report.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using System;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;

namespace CitizenPortalSouthAfrica.Services
{
    /// <summary>
    /// Class for initializing the SQLite database on application startup.
    /// If the database does not exist, it creates one with the specified schema.
    /// </summary>
    public class DatabaseInitialiser
    {
        /// <summary>
        /// Asynchronously initializes the SQLite database. Creates the database file if it doesn't exist and 
        /// sets up the required tables: `ReportIssues` and `ReportIssueFiles`.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task InitializeAsync()
        {
            // Get the path for the database file and construct the connection string
            string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IssueReports.db");
            string connectionString = $"Data Source={databasePath};Version=3;";

            try
            {
                // Check if the database file exists; if not, create it
                if (!File.Exists(databasePath))
                {
                    SQLiteConnection.CreateFile(databasePath);
                }

                // Asynchronously open a connection to the database
                using (var connection = new SQLiteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Use a transaction to ensure that all table creations are completed successfully
                    using (var transaction = connection.BeginTransaction())
                    {
                        // Create the ReportIssues table if it does not already exist
                        string createReportIssuesTableQuery = @"
                        CREATE TABLE IF NOT EXISTS ReportIssues (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Location TEXT NOT NULL,
                            Category TEXT NOT NULL,
                            Description TEXT NOT NULL
                        )";

                        // Execute the command to create the ReportIssues table
                        using (var command = new SQLiteCommand(createReportIssuesTableQuery, connection, transaction))
                        {
                            await command.ExecuteNonQueryAsync();
                        }

                        // Create the ReportIssueFiles table if it does not already exist
                        string createReportIssueFilesTableQuery = @"
                        CREATE TABLE IF NOT EXISTS ReportIssueFiles (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            ReportId INTEGER NOT NULL,
                            FileData BLOB NOT NULL,
                            FOREIGN KEY (ReportId) REFERENCES ReportIssues(Id)
                        )";

                        // Execute the command to create the ReportIssueFiles table
                        using (var command = new SQLiteCommand(createReportIssueFilesTableQuery, connection, transaction))
                        {
                            await command.ExecuteNonQueryAsync();
                        }

                        string createEventsTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Events (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Title TEXT NOT NULL,
                            Description TEXT NOT NULL,
                            Image BLOB,
                            Category TEXT NOT NULL,
                            Date DATETIME NOT NULL
                        )";

                        using (var command = new SQLiteCommand(createEventsTableQuery, connection))
                        {
                            await command.ExecuteNonQueryAsync();
                        }

                        string createAnnouncementsTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Announcements (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Title TEXT NOT NULL,
                            Description TEXT NOT NULL,
                            Image BLOB,
                            Category TEXT NOT NULL,
                            Date DATETIME NOT NULL
                        )";

                        using (var command = new SQLiteCommand(createAnnouncementsTableQuery, connection))
                        {
                            await command.ExecuteNonQueryAsync();
                        }

                        // Commit the transaction to finalize the table creation
                        transaction.Commit();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                // Handle SQLite-specific exceptions and provide a meaningful message
                throw new ApplicationException("An error occurred while initializing the database.", ex);
            }
            catch (Exception ex)
            {
                // Catch any other general exceptions
                throw;
            }
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\
