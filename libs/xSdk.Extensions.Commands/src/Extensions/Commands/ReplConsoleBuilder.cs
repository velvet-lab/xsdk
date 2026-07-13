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

using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace xSdk.Extensions.Commands;

public class ReplConsoleBuilder : ConsoleBuilder
{
    public virtual Action? CreateBannerAction { get => field ?? CreateBanner; set; }

    public virtual Action<IList<Command>>? CreateHelpAction { get => field ?? CreateHelp; set; }

    public virtual Action? CreateLastWillAction { get => field ?? CreateLastWill; set; }

    public virtual Func<string>? CreateUserPromptAction { get => field ?? CreateUserPrompt; set; }

    protected override IApplication BuildApplication(IServiceProvider provider)
        => ActivatorUtilities.CreateInstance<ReplApplication>(provider);


    private static void CreateBanner()
    {
        AnsiConsole.Write(
            new FigletText("xSDK REPL Console")
                .Color(Color.Green)
                .Centered());
    }

    private static string CreateUserPrompt()
        => AnsiConsole.Ask<string>("REPL> ");

    private static void CreateLastWill()
        => AnsiConsole.WriteLine("REPL console is shutting down. Goodbye!");

    private static void CreateHelp(IList<Command> commands)
    {
        foreach (Command command in commands)
        {
            AnsiConsole.WriteLine($"Command: {command.Name}");
            AnsiConsole.WriteLine($"Description: {command.Description}");
            AnsiConsole.WriteLine();
        }
    }
}
