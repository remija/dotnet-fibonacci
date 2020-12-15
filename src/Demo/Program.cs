using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Fibonacci;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables().AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{environmentName}.json", true, true).Build();


        var applicationSection = configuration.GetSection("Application");
        var applicationConfig = applicationSection.Get<ApplicationConfig>();

        var services = new ServiceCollection();
        services.AddDbContext<FibonacciDataContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("FiboConnection")));
        services.AddSingleton<Compute>();
        services.AddLogging();

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        using (var serviceProvider = services.BuildServiceProvider())
        {
            var logger = serviceProvider.GetService<ILogger<Program>>();
            logger.LogInformation($"Application Name : {applicationConfig.Name}");
            logger.LogInformation($"Application Message : {applicationConfig.Message}");
            var compute = serviceProvider.GetService<Compute>();
            var results = await compute.ExecuteAsync(args);

            foreach (var result in results)
            {
                logger.LogInformation($"Fibonacci result : {result}");
            }
        }

        stopwatch.Stop();
        Console.Write($"Elapsed seconds: {stopwatch.Elapsed.TotalSeconds}");
    }

    public class ApplicationConfig
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }
}