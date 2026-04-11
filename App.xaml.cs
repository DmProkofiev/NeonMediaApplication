using Microsoft.Extensions.DependencyInjection;
using NeonMediaApplication.Engine;
using NeonMediaApplication.Factories;
using NeonMediaApplication.Interfaces;
using NeonMediaApplication.Services;
using NeonMediaApplication.ViewModels;
using NeonMediaApplication.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace NeonMediaApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            // Регистрация сервисов
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IMediaEngine, MediaEngine>();
            services.AddTransient<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
            ServiceProvider = services.BuildServiceProvider();
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindow.Show();
        }
    }

}
