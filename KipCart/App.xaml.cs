using KipCart.Database;
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

        public App()
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();

            string connectionString = builder.Configuration.GetConnectionString("MySQL") ?? throw new ArgumentException("Не определена строка подключения БД");

            builder.Services.AddDbContext<KipCartContext>(options =>
            {
                options.UseMySQL(connectionString);
            });

            _host = builder.Build();
            _host.StartAsync();
        }

        private void OnExit(object sender, EventArgs e)
        {
            _host.StopAsync();
        }
    }
}
