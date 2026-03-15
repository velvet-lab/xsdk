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

using xSdk.Shared;

namespace xSdk.Data;

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
