using NLog;

namespace xSdk.Extensions.Plugin
{
    public class PluginDescription : IPluginDescription
    {
        internal static int DefaultOrder => 99999;

        protected ILogger Logger { get; } = LogManager.GetCurrentClassLogger();

        protected internal virtual int Order { get; } = DefaultOrder;

        public string? Name { get; internal set; }

        public Version? Version { get; internal set; }

        public string? Description { get; internal set; }

        public string? ProductVersion { get; internal set; }

        public string? Tag { get; internal set; }

        public List<string> Tags { get; internal set; } = new List<string>();
    }
}
