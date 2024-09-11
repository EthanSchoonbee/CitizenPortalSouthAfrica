using CitizenPortalSouthAfrica.Resources;
using CitizenPortalSouthAfrica.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;

namespace CitizenPortalSouthAfrica.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set => Set(ref _currentView, value);
        }

        public RelayCommand ExitCommand { get; private set; }
        public RelayCommand NavigateToHomeCommand { get; private set; }
        public RelayCommand NavigateToReportIssuesCommand { get; private set; }
        public RelayCommand NavigateToEventsAndAnnouncementsCommand { get; private set; }
        public RelayCommand NavigateToRequestStatusCommand { get; private set; }

        public ReportIssuesViewModel ReportIssuesViewModel { get; private set; }

        public MainWindowViewModel()
        {
            Services.NavigationService.Initialize(this);

            ExitCommand = new RelayCommand(() => Services.NavigationService.GetInstance().ExitApplication());
            NavigateToHomeCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo("Home"));
            NavigateToReportIssuesCommand = new RelayCommand(() => Services.NavigationService.GetInstance().NavigateTo("ReportIssues"));
            //NavigateToEventsAndAnnouncementsCommand = new RelayCommand(OnNavigateToEventsAndAnnouncements);
            //NavigateToRequestStatusCommand = new RelayCommand(OnNavigateToRequestStatus);

            ReportIssuesViewModel = new ReportIssuesViewModel();
            CurrentView = new HomeView();
        }
    }
}
