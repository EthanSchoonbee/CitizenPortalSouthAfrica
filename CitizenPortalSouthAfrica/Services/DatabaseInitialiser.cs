using System;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;

namespace CitizenPortalSouthAfrica.Services
{
    public class DatabaseInitialiser
    {
        public static async Task InitializeAsync()
        {
            string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IssueReports.db");
            string connectionString = $"Data Source={databasePath}";

            if (!File.Exists(databasePath))
            {
                SQLiteConnection.CreateFile(databasePath);
            }

            await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string createReportIssuesTableQuery = @"
                        CREATE TABLE IF NOT EXISTS ReportIssues (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Location TEXT,
                        Category TEXT,
                        Description TEXT
                    )";

                    using (var command = new SQLiteCommand(createReportIssuesTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    string createReportIssueFilesTableQuery = @"
                        CREATE TABLE IF NOT EXISTS ReportIssueFiles (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ReportId INTEGER,
                        FileData BLOB,
                        FOREIGN KEY (ReportId) REFERENCES ReportIssues(Id)
                    )";

                    using (var command = new SQLiteCommand(createReportIssueFilesTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            });
        }
    }
}
