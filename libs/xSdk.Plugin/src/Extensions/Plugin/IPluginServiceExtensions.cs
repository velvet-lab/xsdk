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

namespace xSdk.Extensions.Plugin;

public static class IPluginServiceExtensions
{
    public static bool ConfigureHost<TPluginHost>(this IPluginService pluginService, Action<TPluginHost> factory)
        where TPluginHost : IPluginHost
    {
        var plugins = pluginService
            .GetPlugins<TPluginHost>()
            .Cast<PluginDescription>()
            .OrderBy(p => p.Order)
            .Cast<TPluginHost>()
            .ToList();

        foreach (var plugin in plugins)
        {
            factory?.Invoke(plugin);
        }
        return plugins.Any();
    }

    public static bool ConfigurePlugin<TPluginBuilder>(this IPluginService pluginService, Action<TPluginBuilder> factory)
        where TPluginBuilder : IPluginBuilder
    {
        var plugins = pluginService
            .GetPlugins<TPluginBuilder>()
            .Cast<PluginDescription>()
            .OrderBy(p => p.Order)
            .Cast<TPluginBuilder>()
            .ToList();

        foreach (TPluginBuilder plugin in plugins)
        {
            factory?.Invoke(plugin);
        }
        return plugins.Any();
    }
}
