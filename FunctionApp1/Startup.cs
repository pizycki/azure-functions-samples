using System.IO;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FunctionApp1.Startup))]

namespace FunctionApp1
{
    public class Startup : FunctionsStartup
    {
        IConfiguration _configuration;

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            System.Console.WriteLine("Invoking Startup.ConfigureAppConfiguration(IFunctionsConfigurationBuilder)");

            var context = builder.GetContext();

            string AbsolutePath(string path) => Path.Combine(context.ApplicationRootPath, path);

            _configuration = builder.ConfigurationBuilder
                .AddJsonFile(AbsolutePath("appSettings.json"), optional: false, reloadOnChange: false)
                .AddJsonFile(AbsolutePath($"appSettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            System.Console.WriteLine("Invoking Startup.Configure(IFunctionsHostBuilder)");

            var services = builder.Services;
            services.AddSingleton(_configuration);
        }

    }
}