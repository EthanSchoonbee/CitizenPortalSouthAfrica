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
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\
