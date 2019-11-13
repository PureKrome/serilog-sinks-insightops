using InsightCore.Net;

namespace Serilog.Sinks.InsightOps
{
    public class InsightOpsSettings : IAsyncLoggerConfig
    {
        /// <summary>
        /// The unique token GUID of the log to send messages to. This applies when using the newer token-based logging.
        /// </summary>
        public string Token { get; set ; }

        /// <summary>
        /// Supported Region's.
        /// </summary>
        /// <remarks>List: us, eu, ca, au, jp</remarks>
        /// <see cref="https://insightops.help.rapid7.com/docs/rest-api-overview#section-supported-regions"/>
        public string Region { get; set; }

        /// <summary>
        /// Set to true to use SSL to send data to InsightOps.
        /// </summary>
        /// <remarks>Using SSL secures your payload via HTTPS while insecure uses raw TCP over port 10,000. HTTPS adds a tiny performance hit compared to the raw TCP.</remarks>
        public bool UseSsl { get; set; }

        /// <summary>
        /// Ignored / not used.
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Ignored / not used.
        /// </summary>
        public bool IsUsingDataHub { get; set; }

        /// <summary>
        /// Ignored / not used.
        /// </summary>
        public string DataHubAddress { get; set; }

        /// <summary>
        /// Ignored / not used.
        /// </summary>
        public int DataHubPort { get; set; }

        /// <summary>
        /// Ignored / not used.
        /// </summary>
        public bool LogHostname { get; set; }

        /// <summary>
        /// Ignored / not used.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Ignored / not used.
        /// </summary>
        public string LogID { get; set; }
    }
}
