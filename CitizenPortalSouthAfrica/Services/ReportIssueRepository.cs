using CitizenPortalSouthAfrica.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenPortalSouthAfrica.Services
{
    public class ReportIssueRepository
    {
        private readonly string _connectionString = $"Data Source={AppDomain.CurrentDomain.BaseDirectory}/IssueReports.db";

        public async Task AddReportIssueAsync(ReportIssue reportIssue)
        {
            await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var insertQuery = @"
                    INSERT INTO ReportIssues (Location, Category, Description)
                    VALUES (@Location, @Category, @Description);
                    SELECT last_insert_rowid();";

                    int reportId;

                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Location", reportIssue.Location);
                        command.Parameters.AddWithValue("@Category", reportIssue.Category);
                        command.Parameters.AddWithValue("@Description", reportIssue.Description);

                        reportId = Convert.ToInt32(command.ExecuteScalar());
                    }

                    foreach (var file in reportIssue.Files)
                    {
                        var insertFileQuery = @"
                        INSERT INTO ReportIssueFiles (ReportId, FileData)
                        VALUES (@ReportId, @FileData)";

                        using (var command = new SQLiteCommand(insertFileQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ReportId", reportId);
                            command.Parameters.AddWithValue("@FileData", file);

                            command.ExecuteNonQuery();
                        }
                    }
                }
            });
        }
    }
}
