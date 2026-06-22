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
using Spectre.Console.Cli;
using xSdk.Extensions.Commands;
using xSdk.Hosting;

namespace xSdk.Plugins.Commands;

public static class HostBuilderExtensions
{
    extension(IHostBuilder builder)
    {
        public IHostBuilder EnableReplConsole<TConsoleBuilder, TDefaultCommand>()
            where TConsoleBuilder : class, IConsolePluginBuilder
            where TDefaultCommand : class, ICommand
            => builder
                .EnableReplConsole<TConsoleBuilder, TDefaultCommand>(_ => { });

        public IHostBuilder EnableReplConsole<TConsoleBuilder, TDefaultCommand>(Action<ConsolePluginOptions> configure)
            where TConsoleBuilder : class, IConsolePluginBuilder
            where TDefaultCommand : class, ICommand
            => builder
                .RegisterServices(services => services.AddSingleton<IConsole, ReplConsole>())
                .EnableConsole<TConsoleBuilder, ConsolePluginOptions, TDefaultCommand>(configure);






        public IHostBuilder EnableDefaultConsole()
            => builder.EnableDefaultConsole<DefaultConsolePluginBuilder, DefaultConsoleCommand>(_ => { });

        public IHostBuilder EnableDefaultConsole(Action<ConsolePluginOptions> configure)
            => builder.EnableDefaultConsole<DefaultConsolePluginBuilder, DefaultConsoleCommand>(configure);

        public IHostBuilder EnableDefaultConsole<TConsoleBuilder>()
            where TConsoleBuilder : class, IConsolePluginBuilder
            => builder.EnableDefaultConsole<TConsoleBuilder, DefaultConsoleCommand>(_ => { });

        public IHostBuilder EnableDefaultConsole<TConsoleBuilder>(Action<ConsolePluginOptions> configure)
            where TConsoleBuilder : class, IConsolePluginBuilder
            => builder.EnableDefaultConsole<TConsoleBuilder, DefaultConsoleCommand>(configure);

        public IHostBuilder EnableDefaultConsole<TConsoleBuilder, TDefaultCommand>()
            where TConsoleBuilder : class, IConsolePluginBuilder
            where TDefaultCommand : class, ICommand
            => builder.EnableDefaultConsole<TConsoleBuilder, TDefaultCommand>(_ => { });

        public IHostBuilder EnableDefaultConsole<TConsoleBuilder, TDefaultCommand>(Action<ConsolePluginOptions> configure)
            where TConsoleBuilder : class, IConsolePluginBuilder
            where TDefaultCommand : class, ICommand
            => builder
                .RegisterServices(services => services.AddSingleton<IConsole, DefaultConsole>())
                .EnableConsole<TConsoleBuilder, ConsolePluginOptions, TDefaultCommand>(configure);







        public IHostBuilder EnableConsole<TConsoleBuilder, TConsolePluginOptions, TDefaultCommand>()
            where TConsoleBuilder : class, IConsolePluginBuilder
            where TConsolePluginOptions : ConsolePluginOptions, new()
            where TDefaultCommand : class, ICommand
            => builder
                .EnableConsole<TConsoleBuilder, TConsolePluginOptions, TDefaultCommand>(_ => { });


        public IHostBuilder EnableConsole<TConsoleBuilder, TConsolePluginOptions, TDefaultCommand>(Action<TConsolePluginOptions> configure)
            where TConsoleBuilder : class, IConsolePluginBuilder
            where TConsolePluginOptions : ConsolePluginOptions, new()
            where TDefaultCommand : class, ICommand
            => builder
                .RegisterPluginServices(services =>
                {
                    services
                        .AddSingleton<ICommandAppBuilder, CommandAppBuilder<TDefaultCommand>>();
                })
                .RegisterPluginHost<ConsolePluginHost>()
                .RegisterPluginHostOptions<TConsolePluginOptions>(configure)
                .RegisterPluginBuilder<IConsolePluginBuilder, TConsoleBuilder>();
    }
}
