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

        public static LoggerConfiguration InsightOps(
            this LoggerSinkConfiguration sinkConfiguration,
            InsightOpsSinkSettings settings) => InsightOps(sinkConfiguration, settings, LevelAlias.Minimum, null);

        public static LoggerConfiguration InsightOps(
            this LoggerSinkConfiguration sinkConfiguration, 
            InsightOpsSinkSettings settings,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null)
        {
            var formatter = new MessageTemplateTextFormatter(DefaultOutputTemplate);

            var sink = new InsightOpsSink(settings, formatter);

            return sinkConfiguration.Sink(sink, restrictedToMinimumLevel, levelSwitch);
        }

        // NOTE: If you provide an ITextFormatter, you should also provide an output template inside that instance.
        //       This is why this constructor doesn't offer an ITextFormatter AND an output template + IFormatProvider.
        public static LoggerConfiguration InsightOps(
            this LoggerSinkConfiguration sinkConfiguration,
            InsightOpsSinkSettings settings,
            ITextFormatter formatter = null, 
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (formatter == null)
            {
                formatter = new MessageTemplateTextFormatter(DefaultOutputTemplate);
            }

            return sinkConfiguration.CreateSink(settings, formatter, restrictedToMinimumLevel, levelSwitch);
        }

        public static LoggerConfiguration InsightOps(
            this LoggerSinkConfiguration sinkConfiguration,
            InsightOpsSinkSettings settings,
            string outputTemplate = DefaultOutputTemplate,
            IFormatProvider formatProvider = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (string.IsNullOrWhiteSpace(outputTemplate))
            {
                outputTemplate = DefaultOutputTemplate;
            }

            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);

            return sinkConfiguration.CreateSink(settings, formatter, restrictedToMinimumLevel, levelSwitch);
        }

        private static LoggerConfiguration CreateSink(
            this LoggerSinkConfiguration sinkConfiguration, 
            InsightOpsSinkSettings settings, 
            ITextFormatter formatter,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null)
        {
            var sink = new InsightOpsSink(settings, formatter);

            return sinkConfiguration.Sink(sink, restrictedToMinimumLevel, levelSwitch);
        }

        /// <summary>
        /// Configuration file POCO class: writes events logs to insightOps.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="token">Secret token which links these logs to your logset/account.</param>
        /// <param name="region">Region to send data to. User: au, eu, us, ca, jp.</param>
        /// <param name="useSsl">Use SSL or not. (SSl -might- have a slight performance hit, but don't quote me on that).</param>
        /// <param name="debug">Sets the debug flag. Will print error messages to System.Diagnostics.Trace</param>
        /// <param name="isUsingDataHub">Set to true to use custom DataHub instance instead of Logentries service.</param>
        /// <param name="dataHubAddress">DataHub server address.</param>
        /// <param name="dataHubPort">DataHub server port.</param>
        /// <param name="hostName">User-defined host name. If empty, the library will try to obtain it automatically.</param>
        /// <param name="logHostname">Set to true to send HostName alongside with the log message.</param>
        /// <param name="logId">Log ID.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for events passed through the sink. Ignored when levelSwitch is specified.</param>
        /// <param name="formatter">The formatter to log events in a textual representation. E.G. a compact Json representation for structured logging.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level to be changed at runtime.</param>
        /// <returns></returns>
        public static LoggerConfiguration InsightOps(
            this LoggerSinkConfiguration sinkConfiguration,
            string token,
            string region,
            bool useSsl = true,
            bool debug = false,
            bool isUsingDataHub = false,
            string dataHubAddress = null,
            int dataHubPort = 0,
            string hostName = null, 
            bool logHostname = false,
            string logId = null,
            ITextFormatter formatter = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null)
        {
            var settings = new InsightOpsSinkSettings
            {
                Token = token,
                Region = region,
                UseSsl = useSsl,
                Debug = debug,

                // Rarely used but still available to the developer.
                IsUsingDataHub = isUsingDataHub,
                DataHubAddress = dataHubAddress,
                DataHubPort = dataHubPort,
                LogHostname = logHostname,
                HostName = hostName,
                LogID = logId
            };

            return InsightOps(sinkConfiguration, settings, formatter, restrictedToMinimumLevel, levelSwitch);
        }
    }
}
