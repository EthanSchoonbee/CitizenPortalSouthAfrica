//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     10/09/2024
 * Last Modified:    16/11/2024
 * 
 * Description:
 * This class defines the interaction logic for the MenuComponentView in the CitizenPortalSouthAfrica application.
 * The MenuComponentView represents a menu or navigation component that allows users to navigate between different 
 * sections of the application. It serves as a part of the application's user interface, enabling smooth navigation.
 * 
 * Dependencies:
 * - None: This view relies on standard controls from the System.Windows.Controls namespace.
 * 
 * Methods:
 * - MenuComponentView(): Constructor that initializes the MenuComponentView and its associated components.
 * 
 * Implementation Details:
 * - Uses the InitializeComponent method to load the XAML layout for the menu component.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using System.Windows.Controls;

namespace CitizenPortalSouthAfrica.Views
{
    /// <summary>
    /// Interaction logic for MenuComponentView.xaml
    /// </summary>
    public partial class MenuComponentView : UserControl
    {
        // Constructor: Initializes the MenuComponentView and sets up its UI components.
        /// <summary>
        /// Initializes the MenuComponentView and loads the associated XAML components.
        /// </summary>
        public MenuComponentView()
        {
            // Call to InitializeComponent to load the user interface elements defined in XAML
            InitializeComponent();
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\