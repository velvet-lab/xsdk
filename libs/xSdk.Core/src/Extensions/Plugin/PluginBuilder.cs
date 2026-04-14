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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xSdk.Hosting;

namespace xSdk.Extensions.Plugin;

public abstract class PluginBuilder : PluginDescription, IPluginBuilder
{
    protected ILogger Logger { get => LogManager.GetCurrentClassLogger(); }

    internal protected IServiceProvider Services { get; internal set; }

    protected TPluginBuilder RetrievePluginBuilder<TPluginBuilder>()
        where TPluginBuilder : IPluginBuilder
    {
        if (Services == null)
            throw new InvalidOperationException("Services must be set before retrieving the plugin builder.");
        var pluginBuilder = Services.GetService<TPluginBuilder>();
        if (pluginBuilder == null)
            throw new InvalidOperationException($"Plugin builder of type {typeof(TPluginBuilder).FullName} not found in services.");

        return pluginBuilder;
    }

    protected TOptions? RetrieveOptions<TOptions>()
        where TOptions : class
    {
        IOptions<TOptions>? options = Services.GetService<IOptions<TOptions>>();
        if(options != null)
        {
            return options.Value;
        }
        return default;
    }
}
