//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     13/10/2024
 * Last Modified:    13/10/2024
 * 
 * Description:
 * This class implements the IValueConverter interface to convert 
 * boolean values to WPF Visibility values. It returns 
 * Visibility.Collapsed for a true boolean value and 
 * Visibility.Visible for a false boolean value, allowing 
 * for dynamic control of UI element visibility based on boolean 
 * states. The ConvertBack method is not implemented and will 
 * throw a NotImplementedException if invoked.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace CitizenPortalSouthAfrica.Services
{
    /// <summary>
    /// Converter that transforms a boolean value to a <see cref="Visibility"/> value.
    /// If the boolean value is true, it returns <see cref="Visibility.Collapsed"/>,
    /// otherwise it returns <see cref="Visibility.Visible"/>.
    /// </summary>
    public class InvertedBooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a <see cref="Visibility"/> value.
        /// </summary>
        /// <param name="value">The input boolean value to be converted.</param>
        /// <param name="targetType">The type of the target property (not used in this implementation).</param>
        /// <param name="parameter">An optional parameter (not used in this implementation).</param>
        /// <param name="culture">The culture information (not used in this implementation).</param>
        /// <returns>
        /// <see cref="Visibility.Collapsed"/> if the input value is true; otherwise, 
        /// <see cref="Visibility.Visible"/>. If the input value is not a boolean, 
        /// <see cref="Visibility.Visible"/> is returned by default.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is a boolean
            if (value is bool booleanValue)
            {
                // Return Collapsed if true, Visible if false
                return booleanValue ? Visibility.Collapsed : Visibility.Visible;
            }

            // Default to Visible if the value is not a boolean
            return Visibility.Visible;
        }

        /// <summary>
        /// Converts a <see cref="Visibility"/> value back to a boolean value.
        /// This method is not implemented and will throw a <see cref="NotImplementedException"/>.
        /// </summary>
        /// <param name="value">The input <see cref="Visibility"/> value to convert back.</param>
        /// <param name="targetType">The type of the target property (not used in this implementation).</param>
        /// <param name="parameter">An optional parameter (not used in this implementation).</param>
        /// <param name="culture">The culture information (not used in this implementation).</param>
        /// <returns>
        /// This method always throws <see cref="NotImplementedException"/> as it is not implemented.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // ConvertBack is not implemented
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\