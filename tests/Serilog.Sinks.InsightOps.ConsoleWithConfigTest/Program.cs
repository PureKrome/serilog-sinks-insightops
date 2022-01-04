using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Serilog.Sinks.InsightOps.ConsoleTest
{
    public class Program
    {
        static void Main()
        {
            // If something is wrong with our Serilog setup, 
            // lets make sure we can see what the problem is.
            Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));
            Debugging.SelfLog.Enable(Console.Error);

            Console.WriteLine("Starting.");

            // Create our logger.
            var log = new LoggerConfiguration()
                .ReadFrom.Configuration(GetConfigurationBuilder())
                .Enrich.FromLogContext()
                .CreateLogger();

            // Log some fake info.
            var position = new { Latitude = 25, Longitude = 134 };
            var elapsedMs = 34;
            log.Information("Processed {@Position} in {Elapsed:000} ms. Current Time: {TimeAndDate}", position, elapsedMs, DateTime.Now);

            Console.WriteLine("Flushing and closing...");

            log.Dispose();

            Console.WriteLine("Finished.");
        }

        // This is only to help load the SERILOG information.
        // - appsettings.json
        // Strongly based off/from: https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.host.createdefaultbuilder?view=dotnet-plat-ext-5.0
        private static IConfiguration GetConfigurationBuilder() => 
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
    }
}
