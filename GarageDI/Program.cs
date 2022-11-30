using GarageDI.Constants;
using GarageDI.Handler;
using GarageDI.UI;
using GarageDI.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using VehicleCollection;
using VehicleCollection.Interfaces;

namespace GarageDI
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            try
            {
                serviceProvider.GetService<GarageManager>().Run();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error with following message: {e.Message}");
                throw;
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            IConfiguration configuration = GetConfig();

            services.AddSingleton(configuration);
            services.AddSingleton<ISettings>(configuration.GetSection(StaticStringHelper.GetSettingsFromConfig).Get<Settings>());
            services.AddTransient<GarageManager>();
            services.AddTransient<IGarage<IVehicle>, InMemoryGarage<IVehicle>>();
            services.AddTransient<IGarageHandler, GarageHandler>();
            services.AddTransient<IUI, ConsoleUI>();
            services.AddSingleton<IUtil, Util>();
        }

        private static IConfigurationRoot GetConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(StaticStringHelper.AppSettingsFileName, optional: true, reloadOnChange: true)
                .Build();
        }
    }
}
