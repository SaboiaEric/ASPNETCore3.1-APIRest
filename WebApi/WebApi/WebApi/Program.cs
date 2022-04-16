using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace WebApi
{
    public class Program
    {
        public static void Main()
        {
            CreateHostBuilder(Array.Empty<string>()).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
                webBuilder.ConfigureAppConfiguration(config =>
                {
                    var settings = config.Build();
                    var connection = settings.GetConnectionString("AppConfigurationAzure");
                    config.AddAzureAppConfiguration(connection);
            }).UseStartup<Startup>());
    }
}