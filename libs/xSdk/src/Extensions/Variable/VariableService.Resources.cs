/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using xSdk.Tools;

namespace xSdk.Extensions.Variable;

internal partial class VariableService
{
    public IDictionary<string, object> BuildResources()
    {
        IEnumerable<Variable> variablesWithAttributes = Variables.Cast<Variable>().Where(x => x.Attribute != null);

        Dictionary<string, object> resources = [];
        foreach (var variable in variablesWithAttributes)
        {
            object? value = ReadVariableValueInternal<object>(variable.Name, false, false);
            if ((value == null || TypeConverter.IsEmpty(value, variable.ValueType)) && variable.TelemetryResourceValue != null)
            {
                value = variable.TelemetryResourceValue();
            }

            if (value != null && !TypeConverter.IsEmpty(value, variable.ValueType))
            {
                if (variable.Attribute.ResourceNames != null && variable.Attribute.ResourceNames.Any())
                {
                    foreach (string resourceName in variable.Attribute.ResourceNames)
                    {
                        resources.AddOrNew(resourceName, value.ToString());
                    }
                }
            }
        }

        ReplaceVariableNames(resources);

        return resources;
    }

    private static void ReplaceVariableNames(Dictionary<string, object> resources)
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

            if (resources.TryGetValue(pattern, out object? value))
            {
                var newKey = oldKey.Replace(pattern, value.ToString());
                newKey = newKey.Replace("{{", "").Replace("}}", "");
                resources.AddOrNew(newKey, value);
                resources.Remove(oldKey);
            }
        }
    }
}
