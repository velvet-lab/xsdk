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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Plugin;

namespace xSdk.Hosting;

public abstract class PluginHost : PluginDescription, IPluginHost
{
    protected ILogger Logger => LogManager.GetCurrentClassLogger();

    public IServiceProvider Services { get; internal set; }

    public virtual void ConfigureServices(IServiceCollection services) { }

    public virtual void ConfigureServices(HostBuilderContext context, IServiceCollection services) { }


    protected bool InvokeBuilder<TPluginBuilder>(Action<TPluginBuilder> action)
        where TPluginBuilder : IPluginBuilder
    {
        if (Services == null)
            throw new InvalidOperationException("Services must be set before invoking the plugin builder.");

        var builder = Services.GetService<TPluginBuilder>();
        if (builder != null)
        {
            action?.Invoke(builder);
            return true;
        }
        return false;        
    }

    protected bool InvokeBuilders<TPluginBuilder>(Action<TPluginBuilder> action)
        where TPluginBuilder : IPluginBuilder
    {
        if (Services == null)
            throw new InvalidOperationException("Services must be set before invoking the plugin builders.");

        var builders = Services.GetServices<TPluginBuilder>();
        foreach (var builder in builders)
        {
            action?.Invoke(builder);
        }
        return builders.Any();
    }

    protected TBuilder? GetBuilder<TBuilder>()
        where TBuilder : IPluginBuilder
    {
        if (Services == null)
            throw new InvalidOperationException("Services must be set before getting the plugin builder.");

        return Services.GetService<TBuilder>();
    }
}
