//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     10/09/2024
 * Last Modified:    17/09/2024
 * 
 * Description:
 * This interface defines the contract for a file management service in the CitizenPortalSouthAfrica application. 
 * It provides methods for handling file attachments to issue reports.
 * 
 * Methods:
 * - AttachFilesAsync: Asynchronously allows users to attach files to an issue report. Returns a tuple containing 
 *   lists of valid files (as byte arrays) and their names.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CitizenPortalSouthAfrica.Resources
{
    /// <summary>
    /// Interface for the file management service.
    /// Defines methods for attaching files to issue reports.
    /// </summary>
    internal interface IFileManagementService
    {
        /// <summary>
        /// Asynchronously allows users to attach files to an issue report.
        /// The method returns a tuple containing:
        /// - List of valid files as byte arrays.
        /// - List of valid file names.
        /// </summary>
        /// <returns>A tuple with two lists:
        /// - List of byte arrays representing the files.
        /// - List of file names.
        /// </returns>
        Task<(List<byte[]> Files, List<string> FileNames)> AttachFilesAsync();
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\
