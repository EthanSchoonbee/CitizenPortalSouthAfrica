//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     10/09/2024
 * Last Modified:    17/09/2024
 * 
 * Description:
 * This repository class is responsible for handling the retrieval of events and announcements 
 * from an SQLite database for the CitizenPortalSouthAfrica application. It manages database 
 * connections, queries, and ensures data integrity when pulling records into models.
 * 
 * Note: Entity Framework was initially considered but did not work, so SQLite commands are used instead.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using CitizenPortalSouthAfrica.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;

namespace CitizenPortalSouthAfrica.Services
{
    /// <summary>
    /// Repository for handling database interactions related to events and announcements.
    /// Manages the retrieval of events and announcements from the SQLite database.
    /// </summary>
    public class EventAndAnnouncementRepository
    {
        // Fetches the SQLite database connection string
        private readonly string _connectionString = $"Data Source={AppDomain.CurrentDomain.BaseDirectory}/IssueReports.db";
        private readonly SQLiteConnection _connection;

        public SortedDictionary<string, List<Event>> SortedEvents { get; private set; }
        public SortedDictionary<string, List<Announcement>> SortedAnnouncements { get; private set; }


        public EventAndAnnouncementRepository()
        {
            SortedEvents = new SortedDictionary<string, List<Event>>();
            SortedAnnouncements = new SortedDictionary<string, List<Announcement>>();

            SeedTestData();
        }

        public void SeedTestData()
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    // Example event data
                    var event1 = new Event
                    {
                        Title = "Community Cleanup",
                        Description = "Join us for a day of cleaning the local park.",
                        Image = ConvertImageToByteArray("C:\\Users\\schoo\\Desktop\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\Assets\\audience-1853662_640.jpg"),
                        Category = "Community",
                        Date = DateTime.Now
                    };

                    var event2 = new Event
                    {
                        Title = "Food Drive",
                        Description = "Help us collect food for those in need.",
                        Image = ConvertImageToByteArray("C:\\Users\\schoo\\Desktop\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\Assets\\audience-1853662_640.jpg"),
                        Category = "Charity",
                        Date = DateTime.Now
                    };

                    var event3 = new Event
                    {
                        Title = "Cancer Drive",
                        Description = "Help us collect money for the sick.",
                        Image = ConvertImageToByteArray("C:\\Users\\schoo\\Desktop\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\Assets\\audience-1853662_640.jpg"),
                        Category = "Charity",
                        Date = DateTime.Now
                    };

                    var event4 = new Event
                    {
                        Title = "Blood Drive",
                        Description = "Help us by donating blood!.",
                        Image = ConvertImageToByteArray("C:\\Users\\schoo\\Desktop\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\Assets\\audience-1853662_640.jpg"),
                        Category = "Charity",
                        Date = DateTime.Now
                    };

                    // Example announcement data
                    var announcement1 = new Announcement
                    {
                        Title = "New Library Opening",
                        Description = "We are excited to announce the opening of the new community library.",
                        Image = ConvertImageToByteArray("C:\\Users\\schoo\\Desktop\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\Assets\\istockphoto-1401607744-612x612.jpg"),
                        Category = "Announcement",
                        Date = DateTime.Now
                    };

                    var announcement2 = new Announcement
                    {
                        Title = "Park Renovation",
                        Description = "The local park will be closed for renovations starting next week.",
                        Image = ConvertImageToByteArray("C:\\Users\\schoo\\Desktop\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\Assets\\istockphoto-1401607744-612x612.jpg"),
                        Category = "Public Notice",
                        Date = DateTime.Now
                    };

                    var announcement3 = new Announcement
                    {
                        Title = "New Library",
                        Description = "We are excited to announce the opening of the new community library.",
                        Image = ConvertImageToByteArray("C:\\Users\\schoo\\Desktop\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\Assets\\istockphoto-1401607744-612x612.jpg"),
                        Category = "Announcement",
                        Date = DateTime.Now
                    };

                    var announcement4 = new Announcement
                    {
                        Title = "Park",
                        Description = "The local park will be closed for renovations starting next week.",
                        Image = ConvertImageToByteArray("C:\\Users\\schoo\\Desktop\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\Assets\\istockphoto-1401607744-612x612.jpg"),
                        Category = "Public Notice",
                        Date = DateTime.Now
                    };

                    // Use transactions for consistency
                    using (var transaction = connection.BeginTransaction())
                    {
                        // Insert events if they don't exist
                        InsertEventIfNotExists(connection, event1);
                        InsertEventIfNotExists(connection, event2);
                        InsertEventIfNotExists(connection, event3);
                        InsertEventIfNotExists(connection, event4);

                        // Insert announcements if they don't exist
                        InsertAnnouncementIfNotExists(connection, announcement1);
                        InsertAnnouncementIfNotExists(connection, announcement2);
                        InsertAnnouncementIfNotExists(connection, announcement3);
                        InsertAnnouncementIfNotExists(connection, announcement4);

                        transaction.Commit();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new ApplicationException("An error occurred while seeding test data. Please try again.", ex);
            }
        }

        private void InsertEventIfNotExists(SQLiteConnection connection, Event evt)
        {
            var query = "INSERT INTO Events (Title, Description, Image, Category, Date) " +
                        "SELECT @Title, @Description, @Image, @Category, @Date " +
                        "WHERE NOT EXISTS (SELECT 1 FROM Events WHERE Title = @Title)";

            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", evt.Title);
                command.Parameters.AddWithValue("@Description", evt.Description);
                command.Parameters.AddWithValue("@Image", evt.Image);
                command.Parameters.AddWithValue("@Category", evt.Category);
                command.Parameters.AddWithValue("@Date", evt.Date);

                command.ExecuteNonQuery();
            }
        }

        private void InsertAnnouncementIfNotExists(SQLiteConnection connection, Announcement announcement)
        {
            var query = "INSERT INTO Announcements (Title, Description, Image, Category, Date) " +
                        "SELECT @Title, @Description, @Image, @Category, @Date " +
                        "WHERE NOT EXISTS (SELECT 1 FROM Announcements WHERE Title = @Title)";

            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", announcement.Title);
                command.Parameters.AddWithValue("@Description", announcement.Description);
                command.Parameters.AddWithValue("@Image", announcement.Image);
                command.Parameters.AddWithValue("@Category", announcement.Category);
                command.Parameters.AddWithValue("@Date", announcement.Date);

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Asynchronously retrieves all events from the SQLite database.
        /// </summary>
        /// <returns>A list of Event models.</returns>
        public async Task<List<Event>> GetAllEventsAsync()
        {
            Console.WriteLine(_connectionString);

            var events = new List<Event>();

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {

                    await connection.OpenAsync();

                    Console.WriteLine("Connection open");

                    var query = "SELECT Id, Title, Description, Image, Category, Date FROM Events";

                    Console.WriteLine("Query set");

                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        Console.WriteLine("Command set and executed");
                        while (await reader.ReadAsync())
                        {
                            Console.WriteLine("read success");
                            var evt = new Event
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.GetString(2),
                                Image = reader["Image"] as byte[], // Image can be null, so use 'as' for safe casting
                                Category = reader.GetString(4),
                                Date = reader.GetDateTime(5)
                            };

                            Console.WriteLine("model created");

                            events.Add(evt);

                            Console.WriteLine("event added");
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new ApplicationException("An error occurred while retrieving events. Please try again.", ex);
            }

            return events;
        }

        /// <summary>
        /// Asynchronously retrieves all announcements from the SQLite database.
        /// </summary>
        /// <returns>A list of Announcement models.</returns>
        public async Task<List<Announcement>> GetAllAnnouncementsAsync()
        {
            var announcements = new List<Announcement>();

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = "SELECT Id, Title, Description, Image, Category, Date FROM Announcements";

                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var announcement = new Announcement
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.GetString(2),
                                Image = reader["Image"] as byte[], // Image can be null, so use 'as' for safe casting
                                Category = reader.GetString(4),
                                Date = reader.GetDateTime(5)
                            };

                            announcements.Add(announcement);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new ApplicationException("An error occurred while retrieving announcements. Please try again.", ex);
            }

            return announcements;
        }

        public async Task LoadEventsAndAnnouncementsAsync()
        {
            var eventsFromDb = await GetAllEventsAsync();
            var announcementsFromDb = await GetAllAnnouncementsAsync();

            // Populate the sorted dictionaries
            foreach (var evt in eventsFromDb)
            {
                if (!SortedEvents.ContainsKey(evt.Category))
                    SortedEvents[evt.Category] = new List<Event>();

                SortedEvents[evt.Category].Add(evt);
            }

            foreach (var announcement in announcementsFromDb)
            {
                if (!SortedAnnouncements.ContainsKey(announcement.Category))
                    SortedAnnouncements[announcement.Category] = new List<Announcement>();

                SortedAnnouncements[announcement.Category].Add(announcement);
            }
        }

        public static byte[] ConvertImageToByteArray(string imagePath)
        {
            if (!File.Exists(imagePath))
                return null;

            return File.ReadAllBytes(imagePath);
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\
