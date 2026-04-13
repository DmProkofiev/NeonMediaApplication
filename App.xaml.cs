using Microsoft.Extensions.DependencyInjection;
using NeonMediaApplication.Engine;
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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);
            var provider = services.BuildServiceProvider();
            var mainWindow = provider.GetRequiredService<MainWindow>();
            var viewModel = provider.GetRequiredService<MainWindowViewModel>();
            //var mediaEngine = provider.GetRequiredService<MediaEngine>();
            mainWindow.DataContext = viewModel;
            //mainWindow.DataContext = mediaEngine;
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IMediaEngine, MediaEngine>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<MainWindow>();
        }
    }

}
