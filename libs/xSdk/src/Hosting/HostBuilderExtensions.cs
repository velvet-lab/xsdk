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

using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public static class HostBuilderExtensions
{
    public static IHostBuilder RegisterPluginHost<TPluginHost>(this IHostBuilder builder)
        where TPluginHost : class, IPluginHost
    {
        SlimHostInternal.Instance.PluginSystem.AddPlugin<TPluginHost>();
        return builder;
    }

    public static IHostBuilder RegisterPluginBuilder<TPluginHost>(this IHostBuilder builder)
        where TPluginHost : class, IPluginBuilder
    {
        SlimHostInternal.Instance.PluginSystem.AddPlugin<TPluginHost>();
        return builder;
    }

    //public static IHostBuilder EnablePlugin<TPluginBuilder>(this IHostBuilder builder)
    //{
    //    SlimHostInternal.Instance.PluginSystem.AddPlugin<TPluginBuilder>();
    //    return builder;
    //}

    //public static IHostBuilder EnablePlugin<TPluginBuilder, TPluginBuilder>(this IHostBuilder builder)
    //    where TPluginBuilder : IPlugin
    //    where TPluginBuilder : IPluginBuilder
    //{
    //    SlimHostInternal.Instance.PluginSystem.AddPlugin<TPluginBuilder>();
    //    SlimHostInternal.Instance.PluginSystem.AddPlugin<TPluginBuilder>();
    //    return builder;
    //}

    public static IHostBuilder RegisterSetup<TSetup>(this IHostBuilder builder)
        where TSetup : class, ISetup, new()
    {
        SlimHostInternal.Instance.VariableSystem.RegisterSetup<TSetup>();
        return builder;
    }

    public static IHostBuilder RegisterSetup<TSetup>(this IHostBuilder builder, Action<TSetup>? configure)
        where TSetup : class, ISetup, new()
    {
        SlimHostInternal.Instance.VariableSystem.RegisterSetup<TSetup>(configure);
        return builder;
    }

    public static IHostBuilder RegisterSetup<TSetup>(this IHostBuilder builder, TSetup implementation)
        where TSetup : class, ISetup, new()
    {
        SlimHostInternal.Instance.VariableSystem.RegisterSetup<TSetup>(implementation);
        return builder;
    }
}
