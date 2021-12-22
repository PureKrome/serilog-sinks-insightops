using System;
using System.IO;
using InsightCore.Net;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace Serilog.Sinks.InsightOps
{
    public class InsightOpsSink : ILogEventSink, IDisposable
    {
        private readonly AsyncLogger _asyncLogger;
        private readonly ITextFormatter _textFormatter;

        /// <summary>
        /// The insightOps sink -> a service which sends log messages to insightOps.
        /// </summary>
        /// <param name="config">insightOps settings.</param>
        /// <param name="textFormatter">Formats log events.</param>
        public InsightOpsSink(InsightOpsSinkSettings config, ITextFormatter textFormatter)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            _textFormatter = textFormatter;

            ValidateToken(config.Token);

            _asyncLogger = new AsyncLogger();
            _asyncLogger.setToken(config.Token);
            _asyncLogger.setRegion(config.Region);
            _asyncLogger.setUseSsl(config.UseSsl);

            // These options are more or less, not used.
            _asyncLogger.setDebug(config.Debug);
            _asyncLogger.setIsUsingDataHub(config.IsUsingDataHub);
            _asyncLogger.setDataHubAddr(config.DataHubAddress);
            _asyncLogger.setDataHubPort(config.DataHubPort);
            _asyncLogger.setUseHostName(config.LogHostname);
            _asyncLogger.setHostName(config.HostName);
            _asyncLogger.setLogID(config.LogID);
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent is null)
            {
                throw new ArgumentNullException(nameof(logEvent));
            }

            var stringWriter = new StringWriter();
            _textFormatter.Format(logEvent, stringWriter);

            _asyncLogger.AddLine(stringWriter.ToString());
        }

        /// <summary>
        /// Dispose should automatically be called by Serilog when it Flushes.
        /// </summary>
        /// <remarks>REF: https://github.com/serilog/serilog/wiki/Developing-a-sink#releasing-resources </remarks>
        public void Dispose()
        {
            if (_asyncLogger is null)
            {
                return;
            }

            var numWaits = 3;
            while (!AsyncLogger.AreAllQueuesEmpty(TimeSpan.FromSeconds(2)) && 
                numWaits > 0)
            {
                numWaits--;
            }

            if (numWaits <= 0)
            {
                // Hmm... the queue still had/has some items in it and they probably won't be send downthe wire
                // to Insight Ops ... :/
                Console.WriteLine(" *** Failed to flush the Inisight Ops queue 100%");
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The Token should be a GUID. The InsightOps AsyncLogger does a validation check but quietly
        /// displays an error message to TRACE (which is crap). This can lead to the client NEVER
        /// logging .. and makes it hard to track down (why this client failed to log).
        /// So - lets be proactive and error this hard, fast and early.
        /// </summary>
        /// <param name="guid"></param>
        private static void ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new Exception("The InsightOps Token (which is a Guid) is required. Otherwise, how else are logs going to be sent?");
            }

            var isGuid = Guid.TryParse(token, out var _);
            if (!isGuid)
            {
                throw new Exception($"Provided Token '{token}' is not a valid Guid.");
            }
        }
    }
}
