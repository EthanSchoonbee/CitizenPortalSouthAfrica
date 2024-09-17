//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     10/09/2024
 * Last Modified:    17/09/2024
 * 
 * Description:
 * This interface defines the contract for a navigation service within the CitizenPortalSouthAfrica application. 
 * It provides methods to handle navigation between different views and to cleanly close the application.
 * 
 * Methods:
 * - NavigateTo: Navigates to the specified view by name.
 * - ExitApplication: Closes the application in a clean and orderly manner.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

namespace CitizenPortalSouthAfrica.Resources
{
    /// <summary>
    /// Interface for the navigation service.
    /// Defines methods for navigating between views and exiting the application.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Navigates to the specified view by name.
        /// </summary>
        /// <param name="viewName">The name of the view to navigate to.</param>
        void NavigateTo(string viewName);

        /// <summary>
        /// Closes the application cleanly.
        /// </summary>
        void ExitApplication();
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\
