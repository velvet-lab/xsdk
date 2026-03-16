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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Spectre.Console.Cli;
using xSdk.Extensions.Plugin;
using xSdk.Hosting;


namespace xSdk.Extensions.Commands;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandServices(this IServiceCollection services, Action<IConfigurator> configureDelegate)
    {
        services.TryAddSingleton<ICommandApp>(provider =>
        {
            var app = new CommandApp<DefaultCommand>(new ServiceRegistrar(services));
            return AddCommandServicesInternal(provider, app, configureDelegate);
        });

        return services;
    }

    public static IServiceCollection AddCommandServices<TDefaultCommand>(this IServiceCollection services, Action<IConfigurator> configureDelegate)
        where TDefaultCommand : class, ICommand
    {
        services.TryAddSingleton<ICommandApp>(provider =>
        {
            var app = new CommandApp<TDefaultCommand>(new ServiceRegistrar(services));
            return AddCommandServicesInternal(provider, app, configureDelegate);
        });

        return services;
    }

    private static ICommandApp AddCommandServicesInternal(IServiceProvider provider, ICommandApp app, Action<IConfigurator> configureDelegate)
    {
        app.Configure(config =>
        {
            configureDelegate?.Invoke(config);

            var plugins = SlimHost.Instance.PluginSystem.Invoke<ICommandLinePluginBuilder>(x => x.ConfigureCommandLine(config));
        });

        return app;
    }

    public static IConfigurator AddReplConsole(this IConfigurator config, Action<IReplBuilder> builderDelegate)
    {
        config.AddCommand<ConsoleCommand>(ConsoleCommand.Definitions.Name);
        config.AddCommand<ClearCommand>(ClearCommand.Definitions.Name);
        config.AddCommand<ExitCommand>(ExitCommand.Definitions.Name);

        HostExtensions.ReplBuilderDelegate = builderDelegate;

        return config;
    }
}
