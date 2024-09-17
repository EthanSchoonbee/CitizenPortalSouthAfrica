//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     10/09/2024
 * Last Modified:    17/09/2024
 * 
 * Description:
 * This service class provides validation logic for inputs in the CitizenPortalSouthAfrica
 * application. It includes methods to validate location, category, and description fields,
 * ensuring that these fields are not left empty or invalid when users submit forms. 
 * It also includes a method to validate an entire form and return any validation errors.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using CitizenPortalSouthAfrica.Models;

namespace CitizenPortalSouthAfrica.Services
{
    /// <summary>
    /// Service for handling form input validations.
    /// Provides methods to validate individual fields and the overall form for report issues.
    /// </summary>
    public class ValidationService
    {
        /// <summary>
        /// Validates the location input.
        /// Ensures the location is not null or empty.
        /// </summary>
        /// <param name="location">The location string to validate.</param>
        /// <returns>An error message if validation fails, otherwise an empty string.</returns>
        public string ValidateLocation(string location)
        {
            return string.IsNullOrWhiteSpace(location) ? Constants.ErrorMessages.LocationRequiredError : string.Empty;
        }

        /// <summary>
        /// Validates the category input.
        /// Ensures the category is not null or empty.
        /// </summary>
        /// <param name="category">The category string to validate.</param>
        /// <returns>An error message if validation fails, otherwise an empty string.</returns>
        public string ValidateCategory(string category)
        {
            return string.IsNullOrWhiteSpace(category) ? Constants.ErrorMessages.CategoryRequiredError : string.Empty;
        }

        /// <summary>
        /// Validates the description input.
        /// Ensures the description is not null or empty.
        /// </summary>
        /// <param name="description">The description string to validate.</param>
        /// <returns>An error message if validation fails, otherwise an empty string.</returns>
        public string ValidateDescription(string description)
        {
            return string.IsNullOrWhiteSpace(description) ? Constants.ErrorMessages.DescriptionRequiredError : string.Empty;
        }

        /// <summary>
        /// Validates the entire form by checking the location, category, and description inputs.
        /// Populates the corresponding error messages if validation fails.
        /// </summary>
        /// <param name="location">The location input to validate.</param>
        /// <param name="category">The category input to validate.</param>
        /// <param name="description">The description input to validate.</param>
        /// <param name="locationError">Outputs the location validation error message, if any.</param>
        /// <param name="categoryError">Outputs the category validation error message, if any.</param>
        /// <param name="descriptionError">Outputs the description validation error message, if any.</param>
        /// <returns>True if the form passes validation, false if there are any errors.</returns>
        public bool ValidateForm(string location, string category, string description, out string locationError, out string categoryError, out string descriptionError)
        {
            locationError = ValidateLocation(location);
            categoryError = ValidateCategory(category);
            descriptionError = ValidateDescription(description);

            // Return true if there are no validation errors
            return string.IsNullOrWhiteSpace(locationError) &&
                   string.IsNullOrWhiteSpace(categoryError) &&
                   string.IsNullOrWhiteSpace(descriptionError);
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\
