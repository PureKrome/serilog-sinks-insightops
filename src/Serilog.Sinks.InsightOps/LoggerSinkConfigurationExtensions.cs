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
        /// Configuration file POCO class: writes events logs to insightOps.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="token">Secret token which links these logs to your logset/account.</param>
        /// <param name="region">Region to send data to. User: au, eu, us, ca, jp.</param>
        /// <param name="useSsl">Use SSL or not. (SSl -might- have a slight performance hit, but don't quote me on that).</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for events passed through the sink. Ignored when levelSwitch is specified.</param>
        /// <param name="formatter">The formatter to log events in a textual representation. E.G. a compact Json representation for structured logging.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level to be changed at runtime.</param>
        /// <returns></returns>
        public static LoggerConfiguration InsightOps(
            this LoggerSinkConfiguration sinkConfiguration,
            string token,
            string region,
            ITextFormatter formatter = null,
            bool useSsl = true,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null)
        {
            // We need a formatter, so lets use the other constructor.
            if (formatter == null)
            {
                return InsightOps(sinkConfiguration, token, region, useSsl, restrictedToMinimumLevel, DefaultOutputTemplate, null, levelSwitch);
            }

            // We have a custom formatter, so we need to use the custom output template.
            return InsightOps(sinkConfiguration, token, region, useSsl, formatter, restrictedToMinimumLevel, levelSwitch);
        }

        /// <summary>
        /// Configuration file POCO class: writes events logs to insightOps.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="settings">Configuration settings.</param>
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
            LoggingLevelSwitch levelSwitch = null)
        {
            if (string.IsNullOrWhiteSpace(outputTemplate))
            {
                throw new ArgumentException($"'{nameof(outputTemplate)}' cannot be null or whitespace", nameof(outputTemplate));
            }

            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);

            return sinkConfiguration.InsightOps(token, region, formatter, useSsl, restrictedToMinimumLevel, levelSwitch);
        }

        /// <summary>
        /// Configuration file POCO class: writes events logs to insightOps.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="settings">Configuration settings.</param>
        /// <param name="formatter">The formatter to log events in a textual representation. E.G. a compact Json representation for structured logging.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for events passed through the sink. Ignored when levelSwitch is specified.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level to be changed at runtime.</param>
        /// <returns></returns>
        public static LoggerConfiguration InsightOps(
            this LoggerSinkConfiguration sinkConfiguration,
            string token,
            string region,
            bool useSsl = true,
            ITextFormatter formatter = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null)
        {
            var settings = new InsightOpsSinkSettings
            {
                Token = token,
                Region = region,
                UseSsl = useSsl
            };

            if (formatter is null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var sink = new InsightOpsSink(settings, formatter);

            return sinkConfiguration.Sink(sink, restrictedToMinimumLevel, levelSwitch);
        }
    }
}
