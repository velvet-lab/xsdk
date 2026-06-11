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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using xSdk.Extensions.Commands;
using xSdk.Hosting;

namespace xSdk.Plugins.Commands;

public sealed class CommandPluginHost(ICommandsPluginBuilder builder) : PluginHost
{
    public override void ConfigureLogging(ILoggingBuilder builder)
    {
        //// CLI-Kontext: Nur Warnings, Errors und Critical anzeigen
        //builder.SetMinimumLevel(LogLevel.Information);

        //// Microsoft.Agents.* komplett unterdrücken (spezifischer Filter)
        //builder.AddFilter("Microsoft.Agents", LogLevel.Debug);
        //builder.AddFilter("Microsoft.Agents.AI", LogLevel.Debug);
        //builder.AddFilter("Microsoft.Extensions.AI", LogLevel.Debug);

        //builder.AddFilter((provider, category, level) =>
        //{
        //    Console.WriteLine(provider);
        //    Console.WriteLine(category);
        //    return true;
        //});
    }

    public override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services
            .TryAddSingleton(provider =>
            {
                ICommandApp app = builder.CreateApplication(services);
                app.Configure(config =>
                {
                    builder.Configure(config);
                });

                PromptFactory.Factory = builder.PromptFactory;

                return app;
            });
    }
}
