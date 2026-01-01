using xSdk.Shared;

namespace xSdk.Extensions.Variable
{
    internal partial class VariableService
    {
        public IDictionary<string, object> BuildResources()
        {
            var variablesWithAttributes = Variables.Cast<Variable>().Where(x => x.Attribute != null);

            Dictionary<string, object> resources = new Dictionary<string, object>();
            foreach (var variable in variablesWithAttributes)
            {
                var value = ReadVariableValueInternal<object>(variable.Name, false, false);
                if ((value == null || TypeConverter.IsEmpty(value, variable.ValueType)) && variable.TelemetryResourceValue != null)
                {
                    value = variable.TelemetryResourceValue();
                }

                if (value != null && !TypeConverter.IsEmpty(value, variable.ValueType))
                {
                    if (variable.Attribute.ResourceNames != null && variable.Attribute.ResourceNames.Any())
                    {
                        foreach (var resourceName in variable.Attribute.ResourceNames)
                        {
                            resources.AddOrNew(resourceName, value.ToString());
                        }
                    }
                }
            }

            ReplaceVariableNames(resources);

            return resources;
        }

        private void ReplaceVariableNames(Dictionary<string, object> resources)
        {
            var sources = new List<string>();
            foreach (var item in resources)
            {
                if (item.Key.Contains("{{") && item.Key.Contains("}}"))
                {
                    sources.Add(item.Key);
                }
            }

            foreach (var oldKey in sources)
            {
                var pattern = oldKey.Substring(oldKey.IndexOf("{{") + 2);
                pattern = pattern.Substring(0, pattern.IndexOf("}}"));

                if (resources.TryGetValue(pattern, out object value))
                {
                    var newKey = oldKey.Replace(pattern, value.ToString());
                    newKey = newKey.Replace("{{", "").Replace("}}", "");
                    resources.AddOrNew(newKey, value);
                    resources.Remove(oldKey);
                }
            }
        }
    }
}
