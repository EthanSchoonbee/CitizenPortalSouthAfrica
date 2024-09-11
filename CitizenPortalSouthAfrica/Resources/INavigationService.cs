namespace CitizenPortalSouthAfrica.Resources
{
    public interface INavigationService
    {
        void NavigateTo(string viewName);

        void ExitApplication();
    }
}