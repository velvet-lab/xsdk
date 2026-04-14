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

using System.ComponentModel.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console.Cli;
using xSdk.Extensions.Commands;
using xSdk.Extensions.Plugin;

namespace xSdk.Hosting;

public static class HostExtensions
{
    public static int RunConsole(this IHost host, string[] args) => host.RunConsoleAsync(args).GetAwaiter().GetResult();

    public static async Task<int> RunConsoleAsync(this IHost host, string[] args)
    {
        if (host is null)
        {
            throw new ArgumentNullException(nameof(host));
        }

        var app = host.Services.GetService<ICommandApp>();
        if (app == null)
        {
            throw new InvalidOperationException("Command application has not been configured.");
        }

        // Start any hosted services before running the command application
        var hostedServices = host.Services.GetServices<IHostedService>();
        if (hostedServices.Any())
        {
            hostedServices.ToList().ForEach(hostedService => hostedService.StartAsync(CancellationToken.None).GetAwaiter().GetResult());
        }

        host.RemoveConsoleLoggers();

        var builder = host.Services.RetrievePluginBuilder<ICommandsPluginBuilder>();

        var lastResult = 0;
        var replArgs = args;
        var defaultArgs = CommandlineParser.Parse(args).BackupDefaultArgs();
        var isReplConsole = CommandlineParser.Parse().ContainsPattern(ConsoleCommand.Definitions.Name);

        do
        {
            var currentResult = await app.RunAsync(replArgs);
            if (isReplConsole)
            {
                System.Console.Write(builder.PromptFactory());
                var input = System.Console.ReadLine();

                if (CommandlineParser.Parse(input).AddDefaultArgs(defaultArgs).ContainsPattern(ExitCommand.Definitions.Name))
                {
                    isReplConsole = false;
                }
                else
                {
                    lastResult = currentResult;
                }
                replArgs = CommandlineParser.Parse(input).Arguments;
            }
            else
            {
                lastResult = currentResult;
            }
        } while (isReplConsole);

        return lastResult;
    }

    private static IHost RemoveConsoleLoggers(this IHost host)
    {
        System.Console.Clear();

        // Alle konfigurierten Provider (OTel, Console, ...) bleiben erhalten.
        // Nur der globale MinLevel wird auf Warning gesetzt, damit die Console
        // im REPL-Modus nicht mit Info-Meldungen überfüllt wird.
        var filterOptions = host.Services.GetRequiredService<IOptions<LoggerFilterOptions>>();
        filterOptions.Value.MinLevel = LogLevel.Warning;

        return host;
    }
}
