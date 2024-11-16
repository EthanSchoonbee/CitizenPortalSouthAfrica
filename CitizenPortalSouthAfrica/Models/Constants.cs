//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     10/09/2024
 * Last Modified:    17/09/2024
 * 
 * Description:
 * This static class contains constant strings used throughout the CitizenPortalSouthAfrica application.
 * It includes guide text, error messages, and success messages for various user interactions.
 * 
 * Structure:
 * - GuideText: Provides instructional text to guide users through the reporting process.
 * - ErrorMessages: Contains error messages displayed when validation fails.
 * - SuccessMessages: Includes messages shown when operations complete successfully.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

namespace CitizenPortalSouthAfrica.Models
{
    /// <summary>
    /// Storage of constants for use within the application.
    /// Provides text for guides, error messages, and success notifications.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Text for database connection and queries
        /// </summary>
        public static class Database
        {
            /// <summary>
            /// Name of local database file 
            /// </summary>
            public const string DatabaseFileName = "IssueReports.db";

            /// <summary>
            /// Query for getting all events
            /// </summary>
            public const string GetAllEventsQuery = "SELECT Id, Title, Description, Image, Category, Date FROM Events";

            /// <summary>
            /// Error message for failing to retrieve events from database
            /// </summary>
            public const string FailedEventFetchError = "An error occurred while retrieving events. Please try again.";

            /// <summary>
            /// Query for getting all announcements
            /// </summary>
            public const string GetAllAnnouncementsQuery = "SELECT Id, Title, Description, Image, Category, Date FROM Announcements";

            /// <summary>
            /// Error message for failing to retrieve announcements from database
            /// </summary>
            public const string FailedAnnouncementFetchError = "An error occurred while retrieving announcements. Please try again.";
        }

        /// <summary>
        /// Text for guiding users through the reporting process.
        /// </summary>
        public static class GuideText
        {
            /// <summary>
            /// Initial guide message for users.
            /// </summary>
            public const string InitialGuide = "Hello! I'm Guru, your personal guide.\n" +
                                               "Please use the form below to report your concerns or problems.\n" +
                                               "Click on a field for guidance. Start by entering the Location.";

            /// <summary>
            /// Guidance text for the location field.
            /// </summary>
            public const string LocationGuide = "Please enter the location where the issue occurred.";

            /// <summary>
            /// Guidance text for the category field.
            /// </summary>
            public const string CategoryGuide = "Select the category that best describes the issue.";

            /// <summary>
            /// Guidance text for the description field.
            /// </summary>
            public const string DescriptionGuide = "Provide a detailed description of the issue.";

            /// <summary>
            /// Guidance text for completion of the form.
            /// </summary>
            public const string CompletionGuide = "Well done! You can now add relevant pictures or documents " +
                                                  "if needed and submit the form!";
        }

        /// <summary>
        /// Text for error messages displayed to users.
        /// </summary>
        public static class ErrorMessages
        {
            /// <summary>
            /// Title for validation error messages.
            /// </summary>
            public const string ValidationErrorTitle = "Error";

            /// <summary>
            /// General validation error message.
            /// </summary>
            public const string ValidationError = "Please ensure all fields are correctly filled.";

            /// <summary>
            /// Error message for missing location input.
            /// </summary>
            public const string LocationRequiredError = "Please ensure all fields are correctly filled.";

            /// <summary>
            /// Error message for missing category input.
            /// </summary>
            public const string CategoryRequiredError = "Please ensure all fields are correctly filled.";

            /// <summary>
            /// Error message for missing description input.
            /// </summary>
            public const string DescriptionRequiredError = "Please ensure all fields are correctly filled.";

            /// <summary>
            /// Error message header for feature not avaliable.
            /// </summary>
            public const string FeatureNotAvaliableHeader = "Feature Not Available";

            /// <summary>
            /// Error message for feature not avaliable.
            /// </summary>
            public const string FeatureNotAvaliableMessage = "This feature is currently under development. Please check back later!";

            /// <summary>
            /// Error message when loading events and annoucements from database
            /// </summary>
            public const string ErrorLoadingEventsAndAnnoucements = "Error loading events and announcements data: ";
        }

        /// <summary>
        /// Text for success messages displayed to users.
        /// </summary>
        public static class SuccessMessages
        {
            /// <summary>
            /// Success message displayed when an issue report is saved.
            /// </summary>
            public const string IssueSaved = "Issue report saved successfully!";

            /// <summary>
            /// Title for success messages.
            /// </summary>
            public const string SuccessTitle = "Success";
        }

        /// <summary>
        /// Text headers for navigation logic
        /// </summary>
        public static class NavigationHeaders
        {
            /// <summary>
            /// Home navigation header
            /// </summary>
            public const string Home = "Home";

            /// <summary>
            /// Home navigation header
            /// </summary>
            public const string ReportIssues = "ReportIssues";

            /// <summary>
            /// Home navigation header
            /// </summary>
            public const string EventsAndAnnouncements = "EventsAndAnnouncements";

            /// <summary>
            /// Home navigation header
            /// </summary>
            public const string RequestStatus = "RequestStatus";
        }

        /// <summary>
        /// Status
        /// </summary>
        public static class StatusValues
        {
            /// <summary>
            /// Open status
            /// </summary>
            public const string Open = "Open";

            /// <summary>
            /// Closed status
            /// </summary>
            public const string Closed = "Closed";

            /// <summary>
            /// In progress status
            /// </summary>
            public const string InProgress = "In Progress";
        }

        /// <summary>
        /// Red-Black BST Node colors
        /// </summary>
        public enum NodeColor
        {
            Red,
            Black
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\
