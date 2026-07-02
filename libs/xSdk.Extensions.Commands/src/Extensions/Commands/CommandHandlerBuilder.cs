using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace xSdk.Extensions.Commands;

internal class CommandHandlerBuilder : ICommandHandlerBuilder
{
    private readonly IList<ICommandHandlerBuilder> _commandHandlerBuilders = new List<ICommandHandlerBuilder>();

    private readonly ICommandHandlerBuilder? _commandHandlerBuilder;
    private readonly IApplicationBuilder? _consoleApplicationBuilder;

    private Command? _command;

    private const string CommandPathSeparator = " / ";

    public CommandHandlerBuilder(ICommandHandlerBuilder parent)
    {
        _commandHandlerBuilder = parent;
    }

    public CommandHandlerBuilder(IApplicationBuilder parent)
    {
        _consoleApplicationBuilder = parent;
    }

    public IList<ICommandHandlerBuilder> Childs { get; } = new List<ICommandHandlerBuilder>();

    public string Name { get; internal set; } = string.Empty;

    public string? Description { get; internal set; } = string.Empty;

    internal Command Command
        => _command ?? throw new InvalidOperationException("Command has not been initialized and builded.");

    internal Type? HandlerType { get; set; } = null!;

    private bool IsRoot => _consoleApplicationBuilder != null;

    public ICommandHandlerBuilder AddCommand<THandler>(string name, string? description = null)
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

        foreach (var commandHandlerBuilder in _commandHandlerBuilders)
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

        if (IsRoot && _consoleApplicationBuilder is ApplicationBuilder consoleBuilder)
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
                var commandPath = BuildCommandPath();
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

    private void ParseOptionsAndArguments(Command command, Type? handlerType)
    {
        if (handlerType is null)
        {
            return;
        }

        var options = OptionManager.Build(handlerType);
        foreach (var option in options)
        {
            command.Options.Add(option);
        }

        var arguments = ArgumentManager.Build(handlerType);
        foreach (var argument in arguments)
        {
            command.Arguments.Add(argument);
        }
    }
}
