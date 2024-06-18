using Accessibility;
using KipCart.Database;
using KipCart.ViewModels;
using KipCart.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace KipCart
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IHost _host;
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();

            string connectionString = builder.Configuration.GetConnectionString("MySQL") ?? throw new ArgumentException("Не определена строка подключения БД");

            builder.Services.AddDbContext<KipCartContext>(options =>
            {
                options.UseMySQL(connectionString);
            });

            builder.RegisterViewModels();
            builder.RegisterViews();

            _host = builder.Build();
            _serviceProvider = _host.Services;                
            _host.StartAsync();
        }

        private void OnStartup(object sender, EventArgs e)
        {
            MainWindow? mainWindow = _serviceProvider.GetService<MainWindow>();
            if (mainWindow is null)
                throw new ArgumentNullException(nameof(mainWindow), "MainWindow не зарегистрировано в контейнере сервисов");

            mainWindow.Show();
        }

        private void OnExit(object sender, EventArgs e)
        {
            _host.StopAsync();
        }
    }

    public static class Services
    {
        public static HostApplicationBuilder RegisterViewModels(this HostApplicationBuilder builder)
        {
            builder.Services.AddTransient<MainWindowViewModel>();

            return builder;
        }

        public static HostApplicationBuilder RegisterViews(this HostApplicationBuilder builder)
        {
            builder.Services.AddTransient<GoodsView>();
            builder.Services.AddTransient<PurchaseView>();
            builder.Services.AddTransient<PurchaseHistoryView>();

            builder.Services.AddSingleton<MainWindow>();

            return builder;
        }
    }
}
