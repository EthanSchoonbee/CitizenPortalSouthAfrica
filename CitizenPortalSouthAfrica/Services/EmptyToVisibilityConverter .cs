//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     11/10/2024
 * Last Modified:    16/10/2024
 * 
 * Description:
 * This class implements the IValueConverter interface and is used to convert 
 * an integer value to a `Visibility` value. It determines whether an element 
 * in the UI should be visible or collapsed based on whether the integer value 
 * represents an empty state (e.g., a count of zero).
 * 
 * Usage:
 * Commonly used in WPF applications for data binding scenarios where the 
 * visibility of UI elements depends on a property value.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace CitizenPortalSouthAfrica.Services
{
    /// <summary>
    /// Converts an integer value to a <see cref="Visibility"/> value for UI binding purposes.
    /// Returns <see cref="Visibility.Visible"/> if the value is zero; otherwise, returns <see cref="Visibility.Collapsed"/>.
    /// </summary>
    public class EmptyToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts an integer value to a visibility state.
        /// </summary>
        /// <param name="value">The integer value to evaluate.</param>
        /// <param name="targetType">The target type (not used in this implementation).</param>
        /// <param name="parameter">Additional parameter (not used in this implementation).</param>
        /// <param name="culture">The culture information (not used in this implementation).</param>
        /// <returns>
        /// <see cref="Visibility.Visible"/> if the value is zero, 
        /// otherwise <see cref="Visibility.Collapsed"/>.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is an integer and evaluate its value.
            if (value is int count)
            {
                // Return Visible if count is zero; otherwise, return Collapsed.
                return count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            // Default to Collapsed for invalid or null values.
            return Visibility.Collapsed;
        }

        /// <summary>
        /// This method is not implemented as the conversion is one-way.
        /// </summary>
        /// <param name="value">The value to convert back (not used).</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">Additional parameter (not used).</param>
        /// <param name="culture">The culture information (not used).</param>
        /// <returns>Throws <see cref="NotImplementedException"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ConvertBack is not supported as this is a one-way converter.
            throw new NotImplementedException();
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\