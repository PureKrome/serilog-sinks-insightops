using System;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace Serilog.Sinks.InsightOps
{
    public static class LoggerSinkConfigurationExtensions
    {
        const string DefaultOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// Writes events logs to insightOps.
        /// </summary>
        /// <param name="loggerConfiguration">Logger sink configuration.</param>
        /// <param name="config">Configuration settings.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for events passed through the sink. Ignored when levelSwitch is specified.</param>
        /// <param name="formatProvider">Optional: Supplies culture-specific formatting information, or null.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level to be changed at runtime.</param>
        /// <returns></returns>
        public static LoggerConfiguration InsightOps(this LoggerSinkConfiguration loggerConfiguration,
                                                     InsightOpsSinkSettings config,
                                                     LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose,
                                                     string outputTemplate = DefaultOutputTemplate,
                                                     IFormatProvider formatProvider = null,
                                                     LoggingLevelSwitch levelSwitch = null)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (string.IsNullOrWhiteSpace(outputTemplate))
            {
                throw new ArgumentException($"'{nameof(outputTemplate)}' cannot be null or whitespace", nameof(outputTemplate));
            }

            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
            var sink = new InsightOpsSink(config, formatProvider);
            
            return loggerConfiguration.Sink(sink, restrictedToMinimumLevel, levelSwitch);
        }
    }
}
