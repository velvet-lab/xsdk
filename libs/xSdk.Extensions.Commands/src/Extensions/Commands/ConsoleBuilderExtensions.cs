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

namespace xSdk.Extensions.Commands;

public static class ConsoleBuilderExtensions
{
    extension(ConsoleBuilder builder)
    {
        public ConsoleBuilder AddDefaultCommands()
        {
            builder
                .AddCommand<ExitCommand>(ExitCommand.Definitions.Name, ExitCommand.Definitions.HelpText)
                .AddCommand<ClearCommand>(ClearCommand.Definitions.Name, ClearCommand.Definitions.HelpText)
                .AddCommand<HelpCommand>(HelpCommand.Definitions.Name, HelpCommand.Definitions.HelpText);

            return builder;
        }

        public ConsoleBuilder WithDescription(string description)
        {
            builder.Description = description;
            return builder;
        }

        public ConsoleBuilder AddCommand<THandler>(string name, string? description = default)
            where THandler : class, ICommandHandler
            => builder.AddCommand<THandler>(name, description);

        public CommandHandlerBuilder AddBranch(string name, string? description = default)
            => builder.AddBranch(name, description);
    }
}
