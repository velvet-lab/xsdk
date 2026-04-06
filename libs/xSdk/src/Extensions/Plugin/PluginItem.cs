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

using Microsoft.Extensions.Logging;
using xSdk.Hosting;
using xSdk.Shared;

namespace xSdk.Extensions.Plugin;

internal class PluginItem(Weikio.PluginFramework.Abstractions.Plugin weikioPlugin)
{
    private static readonly ILogger _logger = LogManager.CreateLogger<PluginItem>();

    private object? _concretePlugin;
    public int Order { get; set; } = PluginDescription.DefaultOrder;

    public IPluginDescription Description { get; private set; }

    public string Key { get; private set; }

    public object? Plugin
    {
        get
        {
            if (_concretePlugin == null)
            {
                _concretePlugin = Activator.CreateInstance(weikioPlugin);
                Initialize();
            }
            return _concretePlugin;
        }
    }
    public Weikio.PluginFramework.Abstractions.Plugin WeikioPlugin => weikioPlugin;

    public override string ToString()
    {
        return string.Format("{0} v{1}", Description.Name, Description.Version);
    }

    private void Initialize()
    {
        if (weikioPlugin != null && _concretePlugin is PluginDescription description)
        {
            _logger.LogInformation("Initializing plugin {0} v{1}", weikioPlugin.Name, weikioPlugin.Version);

            description.Name = weikioPlugin.Name;
            description.Version = weikioPlugin.Version;
            description.ProductVersion = weikioPlugin.ProductVersion;
            description.Description = weikioPlugin.Description;
            description.Tag = weikioPlugin.Tag;
            description.Tags = weikioPlugin.Tags;

            Order = description.Order;

            var key = string.Format("{0} v{1}", description.Name, description.ProductVersion);
            Key = HashTools.GetHashString(key);
            Description = description;
        }
    }
}
