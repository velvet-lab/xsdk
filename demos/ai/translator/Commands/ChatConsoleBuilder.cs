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

using Spectre.Console;
using xSdk.Plugins.Commands;

namespace xSdk.Demos.Commands;

internal class ChatConsoleBuilder : ChatConsolePluginBuilder
{
    public override void CreateBanner()
    {
        AnsiConsole.Write(
            new FigletText("xSDK Chat Console")
                .Color(Color.Green)
                .Centered());
    }

    public override string CreateUserPrompt()
        => AnsiConsole.Ask<string>("Type message to translate: ");

    public override void CreateLastWill()
        => AnsiConsole.WriteLine("Chat console is shutting down. Goodbye!");
}
