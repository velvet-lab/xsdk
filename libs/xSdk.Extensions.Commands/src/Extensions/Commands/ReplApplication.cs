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
using System.Diagnostics.CodeAnalysis;
using xSdk.Tools;

namespace xSdk.Extensions.Commands;

[ExcludeFromCodeCoverage(Justification = "Interactive REPL application — requires live console I/O, not testable via unit tests.")]
internal class ReplApplication(RootCommand command, ReplConsoleBuilder builder) : IApplication
{
    public async Task<int> RunAsync(string[] args)
    {
        bool shouldRun = true;
        bool isCleared = false;

        var parser = SpecificCommandlineParser.Create(args);
        string[] replArgs = parser.Arguments;

        builder.CreateBannerAction?.Invoke();

        do
        {
            if (replArgs.Length > 0)
            {
                ParseResult parseResult = command.Parse(replArgs);

                Environment.ExitCode = await parseResult.InvokeAsync();
                if (isCleared)
                {
                    builder.CreateBannerAction?.Invoke();
                    isCleared = false;
                }
            }

            string? input = builder.CreateUserPromptAction?.Invoke();
            if (parser.Reparse(input).ContainsPattern(ExitCommand.Definitions.Name))
            {
                shouldRun = false;
            }
            else if (parser.Reparse(input).ContainsPattern(ClearCommand.Definitions.Name))
            {
                isCleared = true;
            }

            replArgs = CommandlineParser.Parse(input).Arguments;

        } while (shouldRun);

        builder.CreateLastWillAction?.Invoke();

        return Environment.ExitCode;
    }
}
