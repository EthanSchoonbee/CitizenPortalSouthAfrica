//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     13/10/2024
 * Last Modified:    13/10/2024
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
        private readonly string _connectionString = $"Data Source={AppDomain.CurrentDomain.BaseDirectory}/{Constants.Database.DatabaseFileName}"; // Connection string for the SQLite database
        private readonly SQLiteConnection _connection; // SQLite connection object

        // Sorted dictionaries to hold events and announcements categorized by their categories
        public SortedDictionary<string, List<Event>> SortedEvents { get; private set; } // Holds sorted events
        public SortedDictionary<string, List<Announcement>> SortedAnnouncements { get; private set; } // Holds sorted announcements

        /// <summary>
        /// Initializes a new instance of the EventAndAnnouncementRepository class.
        /// Populates the sorted dictionaries with test data.
        /// </summary>
        public EventAndAnnouncementRepository()
        {
            SortedEvents = new SortedDictionary<string, List<Event>>(); // Initialize the sorted events dictionary
            SortedAnnouncements = new SortedDictionary<string, List<Announcement>>(); // Initialize the sorted announcements dictionary


            //SeedTestData() //Method for seeding the test data into the local SQLite database
        }

        // Example of how data was added to database fr events and annoucements
        //===============================================================================================================================================================================================
                                /*public void SeedTestData()
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
                                                Image = ConvertImageToByteArray("C:\\Users\\schoo\\Desktop\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\Assets\\event1.jpg"),
                                                Category = "Community",
                                                Date = DateTime.Now
                                            };


                                            // Example announcement data
                                            var announcement1 = new Announcement
                                            {
                                                Title = "New Library Opening",
                                                Description = "We are excited to announce the opening of the new community library.",
                                                Image = ConvertImageToByteArray("C:\\Users\\schoo\\Desktop\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\CitizenPortalSouthAfrica\\Assets\\announcement1.jpg"),
                                                Category = "Announcement",
                                                Date = DateTime.Now
                                            };


                                            // Use transactions for consistency
                                            using (var transaction = connection.BeginTransaction())
                                            {
                                                // Insert events if they don't exist
                                                InsertEventIfNotExists(connection, event1);

                                                // Insert announcements if they don't exist
                                                InsertAnnouncementIfNotExists(connection, announcement1);

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
                                }*/
        //===============================================================================================================================================================================================

        /// <summary>
        /// Asynchronously retrieves all events from the SQLite database.
        /// </summary>
        /// <returns>A list of Event models.</returns>
        public async Task<List<Event>> GetAllEventsAsync()
        {
            var events = new List<Event>(); // List to hold events retrieved from the database

            try
            {
                using (var connection = new SQLiteConnection(_connectionString)) // Create a new SQLite connection
                {
                    await connection.OpenAsync(); // Open the connection asynchronously

                    var query = Constants.Database.GetAllEventsQuery; // SQL query to select all events

                    using (var command = new SQLiteCommand(query, connection)) // Create a command for the SQL query
                    using (var reader = await command.ExecuteReaderAsync()) // Execute the command and get a data reader
                    {
                        while (await reader.ReadAsync()) // Read the results asynchronously
                        {
                            var evt = new Event
                            {
                                Id = reader.GetInt32(0), // Get the event ID
                                Title = reader.GetString(1), // Get the event title
                                Description = reader.GetString(2), // Get the event description
                                Image = reader["Image"] as byte[], // Get the event image (can be null)
                                Category = reader.GetString(4), // Get the event category
                                Date = reader.GetDateTime(5) // Get the event date
                            };

                            events.Add(evt); // Add the event to the list
                        }
                    }
                }
            }
            catch (SQLiteException ex) // Catch any SQLite exceptions
            {
                throw new ApplicationException(Constants.Database.FailedEventFetchError, ex); // Rethrow with a custom message
            }

            return events; // Return the list of events
        }

        /// <summary>
        /// Asynchronously retrieves all announcements from the SQLite database.
        /// </summary>
        /// <returns>A list of Announcement models.</returns>
        public async Task<List<Announcement>> GetAllAnnouncementsAsync()
        {
            var announcements = new List<Announcement>(); // List to hold announcements retrieved from the database

            try
            {
                using (var connection = new SQLiteConnection(_connectionString)) // Create a new SQLite connection
                {
                    await connection.OpenAsync(); // Open the connection asynchronously

                    var query = Constants.Database.GetAllAnnouncementsQuery; // SQL query to select all announcements

                    using (var command = new SQLiteCommand(query, connection)) // Create a command for the SQL query
                    using (var reader = await command.ExecuteReaderAsync()) // Execute the command and get a data reader
                    {
                        while (await reader.ReadAsync()) // Read the results asynchronously
                        {
                            var announcement = new Announcement
                            {
                                Id = reader.GetInt32(0), // Get the announcement ID
                                Title = reader.GetString(1), // Get the announcement title
                                Description = reader.GetString(2), // Get the announcement description
                                Image = reader["Image"] as byte[], // Get the announcement image (can be null)
                                Category = reader.GetString(4), // Get the announcement category
                                Date = reader.GetDateTime(5) // Get the announcement date
                            };

                            announcements.Add(announcement); // Add the announcement to the list
                        }
                    }
                }
            }
            catch (SQLiteException ex) // Catch any SQLite exceptions
            {
                throw new ApplicationException(Constants.Database.FailedAnnouncementFetchError, ex); // Rethrow with a custom message
            }

            return announcements; // Return the list of announcements
        }

        /// <summary>
        /// Asynchronously loads events and announcements into their respective sorted dictionaries.
        /// </summary>
        public async Task LoadEventsAndAnnouncementsAsync()
        {
            var eventsFromDb = await GetAllEventsAsync(); // Retrieve all events from the database
            var announcementsFromDb = await GetAllAnnouncementsAsync(); // Retrieve all announcements from the database

            // Populate the sorted dictionaries
            foreach (var evt in eventsFromDb)
            {
                if (!SortedEvents.ContainsKey(evt.Category)) // Check if the category exists
                    SortedEvents[evt.Category] = new List<Event>(); // If not, create a new list for the category

                SortedEvents[evt.Category].Add(evt); // Add the event to the corresponding category
            }

            foreach (var announcement in announcementsFromDb)
            {
                if (!SortedAnnouncements.ContainsKey(announcement.Category)) // Check if the category exists
                    SortedAnnouncements[announcement.Category] = new List<Announcement>(); // If not, create a new list for the category

                SortedAnnouncements[announcement.Category].Add(announcement); // Add the announcement to the corresponding category
            }
        }

        /// <summary>
        /// Converts an image file to a byte array.
        /// </summary>
        /// <param name="imagePath">The file path of the image.</param>
        /// <returns>A byte array representation of the image or null if the file does not exist.</returns>
        public static byte[] ConvertImageToByteArray(string imagePath)
        {
            if (!File.Exists(imagePath)) // Check if the file exists
                return null; // Return null if the file does not exist

            return File.ReadAllBytes(imagePath); // Read and return the image file as a byte array
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\