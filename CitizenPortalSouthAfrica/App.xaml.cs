using CitizenPortalSouthAfrica.Models;
using CitizenPortalSouthAfrica.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CitizenPortalSouthAfrica
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                await DatabaseInitialiser.InitializeAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during database initialization: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
            
        }
    }
}
