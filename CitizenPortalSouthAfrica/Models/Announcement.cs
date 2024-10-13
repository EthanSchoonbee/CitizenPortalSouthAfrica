using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace CitizenPortalSouthAfrica.Models
{
    public class Announcement
    {
        public int Id { get; set; } // Unique identifier for the announcement
        public string Title { get; set; } // Title of the announcement
        public string Description { get; set; } // Description of the announcement
        public byte[] Image { get; set; } // Image stored as a byte array (BLOB)
        public string Category { get; set; } // Category of the announcement
        public DateTime Date { get; set; } // Date of the announcement

        public BitmapImage ImageSource
        {
            get
            {
                if (Image == null || Image.Length == 0)
                    return null;

                using (var stream = new MemoryStream(Image))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                    return bitmap;
                }
            }
        }
    }
}
