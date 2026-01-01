using xSdk.Shared;

namespace xSdk.Data
{
    public abstract class ConnectionBuilder : IConnectionBuilder
    {
        private Dictionary<string, string> _connectionProperties;

        public ConnectionBuilder()
        {
            _connectionProperties = new Dictionary<string, string>();
        }

        protected void AddConnectionProperty(string name, string value)
        {
            _connectionProperties.AddOrNew(name, value);
        }

        protected void RemoveConnectionProperty(string name)
        {
            _connectionProperties.Remove(name);
        }

        protected string ResolvePlaceholders(string content)
        {
            // string name, string fileName
            foreach (var kvp in _connectionProperties)
            {
                var placeholder = kvp.Key;
                var value = kvp.Value;

                if (!placeholder.StartsWith("{"))
                    placeholder = "{" + kvp.Key + "}";

                if (content.IndexOf(placeholder, StringComparison.InvariantCultureIgnoreCase) > -1)
                {
                    content = content.Replace(placeholder, value, StringComparison.InvariantCultureIgnoreCase);
                }

                if (content.IndexOf("{") > -1 && content.IndexOf("}") > -1)
                {
                    var leftIndex = content.IndexOf("{");
                    if (leftIndex > -1)
                    {
                        var rightIndex = content.IndexOf("}", leftIndex);
                        if (rightIndex > -1)
                        {
                            var left = content.Substring(0, leftIndex + 1);
                            var right = content.Substring(rightIndex + 1);

                            content = $"{left}{value}{right}";
                        }
                    }
                }
            }
            return content;
        }

        protected void ResetConnectionProperties()
        {
            _connectionProperties = new Dictionary<string, string>();
        }

        internal object InitializeConnection(IDatabaseSetup setup)
        {
            foreach (var kvp in setup.Properties)
                AddConnectionProperty(kvp.Key, kvp.Value);

            return Create(setup);
        }

        public abstract object Create(IDatabaseSetup setup);
    }
}
