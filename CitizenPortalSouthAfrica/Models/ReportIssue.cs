//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     10/09/2024
 * Last Modified:    17/09/2024
 * 
 * Description:
 * This class represents a report issue model in the CitizenPortalSouthAfrica application. 
 * It includes properties for capturing information about an issue report, such as location, category, and description.
 * The model also includes a list for storing attached files related to the issue.
 * 
 * Properties:
 * - Id: Primary key for the report issue.
 * - Location: The location where the issue was reported.
 * - Category: The category of the issue.
 * - Description: A detailed description of the issue.
 * - Files: A list of file data (byte arrays) associated with the issue.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using System;
using System.Collections.Generic;

namespace CitizenPortalSouthAfrica.Models
{
    /// <summary>
    /// Model for a report issue.
    /// Represents an issue report with details and associated files.
    /// </summary>
    public class ReportIssue
    {
        /// <summary>
        /// Gets or sets the primary key for the report issue.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the location where the issue was reported.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the category of the issue.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets a detailed description of the issue.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The status of a report.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The creation date of a report.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets a list of file data (byte arrays) associated with the issue.
        /// </summary>
        public List<byte[]> Files { get; set; } = new List<byte[]>();
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\
