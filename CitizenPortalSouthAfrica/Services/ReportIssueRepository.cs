//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     10/09/2024
 * Last Modified:    17/09/2024
 * 
 * Description:
 * This service class is responsible for handling the saving of report issues into an SQLite database
 * for the CitizenPortalSouthAfrica application. It manages database connections, transactions, and 
 * ensures data integrity when inserting report issue records and their associated files.
 * 
 * Note: Entity Framework was initially considered but did not work, so SQLite commands are used instead.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using CitizenPortalSouthAfrica.Models;
using System;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CitizenPortalSouthAfrica.Services
{
    /// <summary>
    /// Service for handling the database interactions related to report issues.
    /// Manages the insertion of report issues and associated files into the SQLite database.
    /// </summary>
    public class ReportIssueRepository
    {
        // Fetches the SQLite database connection string
        private readonly string _connectionString = $"Data Source={AppDomain.CurrentDomain.BaseDirectory}/IssueReports.db";

        /// <summary>
        /// Asynchronously adds a generated report issue to the SQLite database.
        /// This method uses transactions to ensure data consistency and parameterized queries to prevent SQL injection.
        /// </summary>
        /// <param name="reportIssue">The report issue model containing details such as location, category, description, and associated files.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the report issue object is null.</exception>
        /// <exception cref="ApplicationException">Thrown when an error occurs during database interaction.</exception>
        public async Task AddReportIssueAsync(ReportIssue reportIssue)
        {
            if (reportIssue == null)
                throw new ArgumentNullException(nameof(reportIssue));

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Use a transaction to ensure atomicity and data integrity
                    using (var transaction = connection.BeginTransaction())
                    {
                        // SQL query to insert the report issue into the ReportIssues table
                        var insertQuery = @"
                        INSERT INTO ReportIssues (Location, Category, Description, Status, CreationDate)
                        VALUES (@Location, @Category, @Description, @Status, @CreationDate);
                        SELECT last_insert_rowid();";

                        int reportId;

                        using (var command = new SQLiteCommand(insertQuery, connection, transaction))
                        {
                            // Add parameters to prevent SQL injection
                            command.Parameters.AddWithValue("@Location", reportIssue.Location);
                            command.Parameters.AddWithValue("@Category", reportIssue.Category);
                            command.Parameters.AddWithValue("@Description", reportIssue.Description);
                            command.Parameters.AddWithValue("@Status", reportIssue.Status);
                            command.Parameters.AddWithValue("@CreationDate", reportIssue.CreationDate);

                            // Execute the query and retrieve the ID of the newly inserted report
                            reportId = Convert.ToInt32(await command.ExecuteScalarAsync());
                        }

                        // If there are associated files, insert them into the ReportIssueFiles table
                        if (reportIssue.Files != null && reportIssue.Files.Count > 0)
                        {
                            foreach (var file in reportIssue.Files)
                            {
                                // SQL query to insert associated files for the report issue
                                var insertFileQuery = @"
                                INSERT INTO ReportIssueFiles (ReportId, FileData)
                                VALUES (@ReportId, @FileData)";

                                using (var command = new SQLiteCommand(insertFileQuery, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@ReportId", reportId);
                                    // Assumes file data is a byte array. Encrypting sensitive data may be considered.
                                    command.Parameters.AddWithValue("@FileData", file);

                                    // Execute the query to insert file data
                                    await command.ExecuteNonQueryAsync();
                                }
                            }
                        }

                        // Commit the transaction once all inserts have been successfully executed
                        transaction.Commit();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                // Handle any database-related errors and rethrow as an ApplicationException
                throw new ApplicationException("An error occurred while saving the report. Please try again.", ex);
            }
            catch (Exception ex)
            {
                // Rethrow any non-SQLite exceptions
                throw;
            }
        }


        public ReportBST GetReportBST()
        {
            var reportBST = new ReportBST();

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    var query = "SELECT Id, Location, Category, Description, Status, CreationDate FROM ReportIssues";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var report = new Report();

                                // Safely retrieve each column
                                report.Id = reader["Id"] is DBNull ? 0 : Convert.ToInt32(reader["Id"]);
                                report.Location = reader["Location"] as string ?? string.Empty;
                                report.Name = "Report " + (reader["Id"] is DBNull ? "Unknown" : reader["Id"].ToString());
                                report.Category = reader["Category"] as string ?? string.Empty;
                                report.Description = reader["Description"] as string ?? string.Empty;
                                report.Status = reader["Status"] as string ?? string.Empty;

                                // Handle CreationDate conversion
                                if (reader["CreationDate"] is DBNull)
                                {
                                    report.CreationDate = DateTime.MinValue; // Or a suitable default value
                                }
                                else
                                {
                                    report.CreationDate = Convert.ToDateTime(reader["CreationDate"]);
                                }

                                reportBST.Insert(report); // Insert directly into the BST
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new ApplicationException("An error occurred while retrieving reports.", ex);
            }

            return reportBST;
        }

    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\
