using xSdk.Shared;
using NLog;

namespace xSdk.Extensions.Plugin
{
    internal class PluginItem(Weikio.PluginFramework.Abstractions.Plugin weikioPlugin)
    {
        private Logger Logger = LogManager.GetCurrentClassLogger();

        private object? concretePlugin;
        public int Order { get; set; } = PluginDescription.DefaultOrder;

        public IPluginDescription Description { get; private set; }

        public string Key { get; private set; }

        public object? Plugin
        {
            get
            {
                if (concretePlugin == null)
                {
                    concretePlugin = Activator.CreateInstance(weikioPlugin);
                    Initialize();
                }
                return concretePlugin;
            }
        }
        public Weikio.PluginFramework.Abstractions.Plugin WeikioPlugin => weikioPlugin;

        public override string ToString()
        {
            return string.Format("{0} v{1}", Description.Name, Description.Version);
        }

        private void Initialize()
        {
            if (weikioPlugin != null && concretePlugin is PluginDescription description)
            {
                Logger.Info("Initializing plugin {0} v{1}", weikioPlugin.Name, weikioPlugin.Version);

                description.Name = weikioPlugin.Name;
                description.Version = weikioPlugin.Version;
                description.ProductVersion = weikioPlugin.ProductVersion;
                description.Description = weikioPlugin.Description;
                description.Tag = weikioPlugin.Tag;
                description.Tags = weikioPlugin.Tags;

                Order = description.Order;

                var key = string.Format("{0} v{1}", description.Name, description.ProductVersion);
                Key = HashTools.GetHashString(key);
                this.Description = description;
            }
        }
    }
}
