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

using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using xSdk.Extensions.Commands;
using xSdk.Extensions.Logging;
using xSdk.Plugins.Commands;

namespace xSdk.Hosting;

[ExcludeFromCodeCoverage]
public static class HostExtensions
{
    public static int RunConsole(this IHost host, string[] args) => host.RunConsoleAsync(args).GetAwaiter().GetResult();

    public static async Task<int> RunConsoleAsync(this IHost host, string[] args)
    {
        Guard.IsNotNull(host);

        ICommandApp? app = host.Services.GetService<ICommandApp>() ?? throw new InvalidOperationException("Command application has not been configured.");
                
        await host.StartAsync();

        var parser = CommandlineParser.Parse(args);
        string[] defaultArgs = parser.BackupDefaultArgs();
        bool isReplConsole = parser.ContainsPattern(ConsoleCommand.Definitions.Name);
        string[] replArgs = parser.Arguments;

        ClearConsole();

        do
        {
            Environment.ExitCode = await app.RunAsync(replArgs);
            if (isReplConsole)
            {
                string? prompt = PromptFactory.Factory?.Invoke();
                if (prompt is not null)
                {
                    System.Console.Write(prompt);
                }
                else
                {
                    System.Console.Write("> ");
                }

                string? input = System.Console.ReadLine();
                if (CommandlineParser.Parse(input).AddDefaultArgs(defaultArgs).ContainsPattern(ExitCommand.Definitions.Name))
                {
                    isReplConsole = false;
                }
                else if(string.Compare(input, "help", true) == 0)
                {
                    input = "--help";
                }

                replArgs = CommandlineParser.Parse(input).Arguments;
            }
        } while (isReplConsole);

        return Environment.ExitCode;
    }

    private static void ClearConsole()
    {
        System.Console.Clear();
        AnsiConsole.Clear();
    }
}
