using KipCart.Database;
using KipCart.Services;
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
            _host = Host.CreateApplicationBuilder()
                .RegisterDb()
                .RegisterServices()
                .RegisterViewModels()
                .RegisterViews()
                .Build();

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

    public static class ServicesRegister
    {
        public static HostApplicationBuilder RegisterViewModels(this HostApplicationBuilder builder)
        {
            builder.Services.AddTransient<MainWindowViewModel>()
                .AddTransient<GoodsViewModel>()
                .AddTransient<CatalogWindowViewModel>()
                .AddTransient<PurchaseViewModel>();

            return builder;
        }

        public static HostApplicationBuilder RegisterViews(this HostApplicationBuilder builder)
        {
            builder.Services.AddTransient<GoodsView>()
                .AddTransient<PurchaseView>()
                .AddTransient<PurchaseHistoryView>()

                .AddSingleton<MainWindow>();

            return builder;
        }

        public static HostApplicationBuilder RegisterServices(this HostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IMessagesService, MessagesService>();

            return builder;
        }

        public static HostApplicationBuilder RegisterDb(this HostApplicationBuilder builder)
        {
            string connectionString = builder.Configuration.GetConnectionString("MySQL") ?? throw new ArgumentException("Не определена строка подключения БД");

            builder.Services.AddDbContext<KipCartContext>(options =>
            {
                options.UseMySQL(connectionString);
            });

            return builder;
        }
    }
}
