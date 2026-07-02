// * Copyright 2026 Roland Breitschaft
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Spectre.Console.Cli;
//using xSdk.Extensions.Commands;
//using xSdk.Extensions.Plugin;
//using xSdk.Hosting;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Commands;
using xSdk.Hosting;


///*
namespace xSdk.Plugins.Commands;

public static class HostBuilderExtensions
{
    extension(IHostBuilder builder)
    {
        public IHostBuilder EnableReplConsole<TConsoleBuilder>()
            where TConsoleBuilder : class, IReplConsolePluginBuilder
            => builder
                .EnableReplConsole<TConsoleBuilder>(_ =>
                {
                    _.DisableDefaultHelp = true;
                });

        public IHostBuilder EnableReplConsole<TBuilder>(Action<PluginOptions> configure)
            where TBuilder : class, IReplConsolePluginBuilder
            => builder
                .RegisterPluginServices(services => services.AddSingleton<IApplicationBuilder, ApplicationBuilder<ReplApplication>>())
                .RegisterPluginBuilder<IReplConsolePluginBuilder, TBuilder>()
                .EnableConsole<PluginOptions>(configure);

        public IHostBuilder EnableDefaultConsole<TConsoleBuilder>()
            where TConsoleBuilder : class, IConsolePluginBuilder
            => builder.EnableDefaultConsole<TConsoleBuilder>(_ => { });

        public IHostBuilder EnableDefaultConsole<TBuilder>(Action<PluginOptions> configure)
            where TBuilder : class, IConsolePluginBuilder
            => builder
                .RegisterPluginServices(services => services.AddSingleton<IApplicationBuilder, ApplicationBuilder<ConsoleApplication>>())
                .RegisterPluginBuilder<IConsolePluginBuilder, TBuilder>()
                .EnableConsole<PluginOptions>(configure);

        public IHostBuilder EnableConsole<TConsolePluginOptions>()
            where TConsolePluginOptions : PluginOptions, new()
            => builder
                .EnableConsole<TConsolePluginOptions>(_ => { });

        public IHostBuilder EnableConsole<TConsolePluginOptions>(Action<TConsolePluginOptions> configure)
            where TConsolePluginOptions : PluginOptions, new()           
            => builder
                .RegisterPluginHost<PluginHost>()
                .RegisterPluginHostOptions<TConsolePluginOptions>(configure);
    }
}
