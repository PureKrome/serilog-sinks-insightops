using InsightCore.Net;
using Serilog.Core;
using Serilog.Events;
using System;

namespace Serilog.Sinks.InsightOps
{
    public class InsightOps : ILogEventSink, IDisposable
    {
        private readonly IFormatProvider _formatProvider;
        private readonly AsyncLogger _asyncLogger;
        
        public InsightOps(InsightOpsSettings config,
                          IFormatProvider formatProvider = null)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            _formatProvider = formatProvider; // Optional.

            _asyncLogger = new AsyncLogger();
            _asyncLogger.setToken(config.Token);
            _asyncLogger.setRegion(config.Region);
            _asyncLogger.setUseSsl(config.UseSsl);
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent is null)
            {
                throw new ArgumentNullException(nameof(logEvent));
            }

            var message = logEvent.RenderMessage(_formatProvider);

            _asyncLogger.AddLine(message);
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

            var isQueueEmpty = _asyncLogger.FlushQueue(TimeSpan.FromSeconds(5));

            if (!isQueueEmpty)
            {
                // Hmm... the queue still had/has some items in it and they probably won't be send downthe wire
                // to Insight Ops ... :/
                Console.WriteLine(" *** Failed to flush the Inisight Ops queue 100%");
            }
        }
    }
}
