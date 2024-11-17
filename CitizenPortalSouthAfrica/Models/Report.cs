//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     11/10/2024
 * Last Modified:    16/11/2024
 * 
 * Description:
 * This class represents a report in the CitizenPortalSouthAfrica application. 
 * It encapsulates various properties of a report, such as its ID, name, creation 
 * date, status, and description. The class implements the INotifyPropertyChanged 
 * interface to support data binding with UI elements, allowing the user interface 
 * to automatically update when the properties of the report change. 
 * Additionally, the IsExpanded property is used to track whether the report's 
 * details are expanded in the UI, providing a better user experience.
 * 
 * Usage:
 * This class is designed to hold the data related to a report. It notifies the 
 * user interface whenever the IsExpanded property changes, enabling dynamic 
 * updates in the UI without requiring explicit code to refresh the interface.
 * 
 * The class is useful for scenarios where reports need to be displayed and 
 * interacted with dynamically, such as in list views or collapsible sections.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CitizenPortalSouthAfrica.Models
{
    /// <summary>
    /// Represents a report in the CitizenPortalSouthAfrica application.
    /// The report includes details such as ID, name, creation date, status, 
    /// description, and category. The class implements INotifyPropertyChanged 
    /// to notify the UI of any changes in property values.
    /// </summary>
    public class Report: INotifyPropertyChanged
    {
        private bool _isExpanded; // Private field to track if the report is expanded

        /// <summary>
        /// Gets or sets the ID of the report.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the report.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the report.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the location related to the report.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the status of the report (e.g., pending, completed).
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the description of the report.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the category of the report.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets whether the report is expanded in the UI.
        /// This property triggers property change notifications when changed.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded != value) // Only update if the value is different
                {
                    _isExpanded = value; // Update the internal field
                    OnPropertyChanged(); // Notify UI that the property value has changed
                }
            }
        }

        /// <summary>
        /// Event that is triggered when a property changes in the report.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Helper method to raise the PropertyChanged event, notifying that a property 
        /// has changed. Automatically includes the name of the property being changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Raise the PropertyChanged event
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\