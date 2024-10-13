using CitizenPortalSouthAfrica.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CitizenPortalSouthAfrica.Views
{
    /// <summary>
    /// Interaction logic for EventsAndAnnouncementsView.xaml
    /// </summary>
    public partial class EventsAndAnnouncementsView : UserControl
    {
        public EventsAndAnnouncementsView()
        {
            InitializeComponent();
            this.Loaded += EventsAndAnnouncementsView_Loaded;
        }

        private void EventsAndAnnouncementsView_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is EventsAndAnoncementsViewModel viewModel)
            {
                // Only load data if events or announcements are empty
                if (!viewModel.Events.Any() && !viewModel.Announcements.Any())
                {
                    _ = viewModel.LoadEventAndAnnouncementDataAsync();
                }
            }
        }
    }
}
