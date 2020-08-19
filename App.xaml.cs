using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TimeTableManager.Models;

namespace TimeTableManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddDbContext<MyDbContext>(option => {
                option.UseSqlite("Data Source = TimeTableManager.db");

            });

            services.AddSingleton<TagWindow>();
            serviceProvider = services.BuildServiceProvider();
        }

        private void OnStarup(object s, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<TagWindow>();
            mainWindow.Show();

        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {

        }
    }
}
