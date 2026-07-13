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
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace xSdk.Extensions.Commands;

public sealed class CommandHandlerBuilder
{
    private readonly IList<CommandHandlerBuilder> _commandHandlerBuilders = [];

    private readonly CommandHandlerBuilder? _commandHandlerBuilder;
    private readonly ConsoleBuilder? _consoleBuilder;

    private Command? _command;

    private const string CommandPathSeparator = " / ";

    public CommandHandlerBuilder(CommandHandlerBuilder parent)
    {
        _commandHandlerBuilder = parent;
    }

    public CommandHandlerBuilder(ConsoleBuilder parent)
    {
        _consoleBuilder = parent;
    }

    public IList<CommandHandlerBuilder> Childs { get; } = [];

    public string Name { get; internal set; } = string.Empty;

    public string? Description { get; internal set; } = string.Empty;

    internal Command Command
        => _command ?? throw new InvalidOperationException("Command has not been initialized and builded.");

    internal Type? HandlerType { get; set; } = null!;

    private bool IsRoot => _consoleBuilder != null;

    public CommandHandlerBuilder AddCommand<THandler>(string name, string? description = null)
            where THandler : class, ICommandHandler
    {
        var builder = new CommandHandlerBuilder(this)
        {
            Name = name,
            Description = description,
            HandlerType = typeof(THandler)

        };
        _commandHandlerBuilders.Add(builder);

        return builder;
    }

    internal void Build(IServiceCollection services)
    {
        BuildCommand(services);

        foreach (CommandHandlerBuilder commandHandlerBuilder in _commandHandlerBuilders)
        {
            if (commandHandlerBuilder is CommandHandlerBuilder handlerBuilder)
            {
                handlerBuilder.Build(services);
            }
        }
    }

    private void BuildCommand(IServiceCollection services)
    {
        _command = new Command(Name, Description);

        ParseOptionsAndArguments(_command, HandlerType);

        AddHandler(_command, services);

        if (IsRoot && _consoleBuilder is ConsoleBuilder consoleBuilder)
        {
            // Parent is Root
            consoleBuilder.RootCommand?.Subcommands.Add(_command);
        }
        else if (!IsRoot && _commandHandlerBuilder is CommandHandlerBuilder parentBuilder)
        {
            // Parent is Command
            parentBuilder.Command?.Subcommands.Add(_command);
        }
    }

    private void AddHandler(Command command, IServiceCollection services)
    {
        if (HandlerType is not null)
        {
            command.SetAction((parseResult, cancellationToken) =>
            {
                string commandPath = BuildCommandPath();
                return CommandActivator.Instance.ActivateCommandHandlerAsync(commandPath, parseResult, cancellationToken);
            });

            services
                .TryAddKeyedSingleton<ICommandHandler>(
                    BuildCommandPath(),
                    (provider, key) => (ActivatorUtilities.CreateInstance(provider, HandlerType) as ICommandHandler)!
                );
        }
    }

    private string BuildCommandPath()
    {
        if (IsRoot)
        {
            return Name;
        }
        else if (_commandHandlerBuilder is CommandHandlerBuilder parentBuilder)
        {
            return $"{parentBuilder.BuildCommandPath()}{CommandPathSeparator}{Name}";
        }
        else
        {
            throw new InvalidOperationException("Parent builder is not set.");
        }
    }

    private static void ParseOptionsAndArguments(Command command, Type? handlerType)
    {
        if (handlerType is null)
        {
            return;
        }

        Option[] options = OptionManager.Build(handlerType);
        foreach (Option option in options)
        {
            command.Options.Add(option);
        }

        Argument[] arguments = ArgumentManager.Build(handlerType);
        foreach (Argument argument in arguments)
        {
            command.Arguments.Add(argument);
        }
    }
}
