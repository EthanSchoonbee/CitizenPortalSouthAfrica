//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     12/10/2024
 * Last Modified:    12/10/2024
 * 
 * Description:
 * This class represents an announcement with its details, including title, 
 * description, image, category, and date. It also provides a way to convert 
 * the image stored as a byte array (BLOB) into a BitmapImage for display purposes.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace CitizenPortalSouthAfrica.Models
{
    public class Announcement
    {
        /// <summary>
        /// Gets or sets the unique identifier for the announcement.
        /// </summary>
        public int Id { get; set; } // Unique identifier for the announcement

        /// <summary>
        /// Gets or sets the title of the announcement.
        /// </summary>
        public string Title { get; set; } // Title of the announcement

        /// <summary>
        /// Gets or sets the description of the announcement.
        /// </summary>
        public string Description { get; set; } // Description of the announcement

        /// <summary>
        /// Gets or sets the image of the announcement stored as a byte array (BLOB).
        /// </summary>
        public byte[] Image { get; set; } // Image stored as a byte array (BLOB)

        /// <summary>
        /// Gets or sets the category of the announcement.
        /// </summary>
        public string Category { get; set; } // Category of the announcement

        /// <summary>
        /// Gets or sets the date of the announcement.
        /// </summary>
        public DateTime Date { get; set; } // Date of the announcement

        /// <summary>
        /// Gets the image source as a BitmapImage for display purposes.
        /// Converts the byte array image to a BitmapImage.
        /// </summary>
        public BitmapImage ImageSource
        {
            get
            {
                // Return null if the Image is null or empty.
                if (Image == null || Image.Length == 0)
                    return null;

                // Create a MemoryStream from the byte array image.
                using (var stream = new MemoryStream(Image))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit(); // Begin initialization of the BitmapImage
                    bitmap.CacheOption = BitmapCacheOption.OnLoad; // Cache the image for quick access
                    bitmap.StreamSource = stream; // Set the stream source for the BitmapImage
                    bitmap.EndInit(); // End initialization
                    return bitmap; // Return the created BitmapImage
                }
            }
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\