using System;
using Serilog.Formatting.Compact;

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
            
            // NOTE: Please replace with your own settings.
            const string region = "au"; // au, eu, jp or us.
            const string token = "<some guid from your account>";
            const bool useSsl = false;

            // Create our logger.
            var log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.InsightOps(region, token, new RenderedCompactJsonFormatter(), useSsl)
                .WriteTo.Console(new CompactJsonFormatter())
                .WriteTo.Seq("http://localhost:5301")
                .CreateLogger();

            // Log some fake info.
            var position = new { Latitude = 25, Longitude = 134 };
            var elapsedMs = 34;
            log.Information("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);

            Console.WriteLine("Flushing and closing...");

            log.Dispose();

            Console.WriteLine("Finished.");
        }
    }
}
