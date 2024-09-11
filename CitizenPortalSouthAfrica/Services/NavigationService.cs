using CitizenPortalSouthAfrica.Resources;
using CitizenPortalSouthAfrica.ViewModels;
using CitizenPortalSouthAfrica.Views;
using System;
using System.Windows;

namespace CitizenPortalSouthAfrica.Services
{
    public class NavigationService : INavigationService
    {
        private static NavigationService _instance;
        private readonly MainWindowViewModel _mainWindowViewModel;

        private NavigationService(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        public static NavigationService GetInstance()
        {
            if (_instance == null)
            {
                throw new InvalidOperationException("NavigationService not initialized.");
            }
            return _instance;
        }

        public static void Initialize(MainWindowViewModel mainWindowViewModel)
        {
            if (_instance == null)
            {
                _instance = new NavigationService(mainWindowViewModel);
            }
        }

        public void NavigateTo(string viewName)
        {
            switch (viewName)
            {
                case "Home":
                    _mainWindowViewModel.CurrentView = new HomeView();
                    break;
                case "ReportIssues":
                    _mainWindowViewModel.CurrentView = new ReportIssuesView { DataContext = _mainWindowViewModel.ReportIssuesViewModel };
                    break;
                    // other cases
            }
        }

        public void ExitApplication()
        {
            Application.Current.Shutdown();
        }
    }

}
