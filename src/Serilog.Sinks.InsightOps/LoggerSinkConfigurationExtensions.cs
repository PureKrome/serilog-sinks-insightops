using System;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.InsightOps
{
    public static class LoggerSinkConfigurationExtensions
    {
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
                                                     IFormatProvider formatProvider = null,
                                                     LoggingLevelSwitch levelSwitch = null)
        {
            return loggerConfiguration.Sink(new InsightOpsSink(config, formatProvider), restrictedToMinimumLevel, levelSwitch);
        }
    }
}
