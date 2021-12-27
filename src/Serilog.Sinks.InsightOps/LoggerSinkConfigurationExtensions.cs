using System;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;

namespace Serilog.Sinks.InsightOps
{
    public static class LoggerSinkConfigurationExtensions
    {
        const string DefaultOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// Writes events logs to insightOps.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="token">Secret token which links these logs to your logset/account.</param>
        /// <param name="region">Region to send data to. User: au, eu, us, ca, jp.</param>
        /// <param name="useSsl">Use SSL or not. (SSl -might- have a slight performance hit, but don't quote me on that).</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for events passed through the sink. Ignored when levelSwitch is specified.</param>
        /// <param name="outputTemplate">Custom output template. If not provided, will default to <code>[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}</code></param>
        /// <param name="formatProvider">Optional: Supplies culture-specific formatting information, or null.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level to be changed at runtime.</param>
        /// <returns></returns>
        public static LoggerConfiguration InsightOps(
            this LoggerSinkConfiguration sinkConfiguration,
            string token,
            string region,
            bool useSsl = true,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultOutputTemplate,
            IFormatProvider formatProvider = null,
            LoggingLevelSwitch levelSwitch = null) => InsightOps(
                sinkConfiguration,
                new InsightOpsSinkSettings
                {
                    Token = token,
                    Region = region,
                    UseSsl = useSsl
                },
                restrictedToMinimumLevel,
                outputTemplate,
                formatProvider,
                levelSwitch);

        /// <summary>
        /// Writes events logs to insightOps.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="config">Configuration settings.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for events passed through the sink. Ignored when levelSwitch is specified.</param>
        /// <param name="outputTemplate">Custom output template. If not provided, will default to <code>[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}</code></param>
        /// <param name="formatProvider">Optional: Supplies culture-specific formatting information, or null.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level to be changed at runtime.</param>
        /// <returns></returns>
        public static LoggerConfiguration InsightOps(
            this LoggerSinkConfiguration sinkConfiguration,
            InsightOpsSinkSettings config,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
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

            return sinkConfiguration.InsightOps(config, formatter, restrictedToMinimumLevel, levelSwitch);
        }

        public static LoggerConfiguration InsightOps(
            this LoggerSinkConfiguration sinkConfiguration,
            InsightOpsSinkSettings config,
            ITextFormatter formatter,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (formatter is null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var sink = new InsightOpsSink(config, formatter);

            return sinkConfiguration.Sink(sink, restrictedToMinimumLevel, levelSwitch);
        }
    }
}
