//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     10/09/2024
 * Last Modified:    16/11/2024
 * 
 * Description:
 * This class defines the interaction logic for the HomeView in the CitizenPortalSouthAfrica application.
 * The HomeView serves as the main landing page for the application and is responsible for initializing 
 * and setting up the user interface components. It acts as a container for other views or controls 
 * that may be displayed as part of the application's navigation flow.
 * 
 * Dependencies:
 * - None: This view relies on standard controls from the System.Windows.Controls namespace.
 * 
 * Methods:
 * - HomeView(): Constructor that initializes the HomeView and its associated components.
 * 
 * Implementation Details:
 * - Uses the InitializeComponent method to load the XAML layout for the view.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using System.Windows.Controls;

namespace CitizenPortalSouthAfrica.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary> 
    public partial class HomeView : UserControl
    {
        // Constructor: Initializes the HomeView and sets up its UI components.
        /// <summary>
        /// Initializes the HomeView and loads the associated XAML components.
        /// </summary>
        public HomeView()
        {
            // Call to InitializeComponent to load the user interface elements defined in XAML
            InitializeComponent();
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\