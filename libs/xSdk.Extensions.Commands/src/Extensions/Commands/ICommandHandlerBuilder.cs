namespace xSdk.Extensions.Commands;

public interface ICommandHandlerBuilder
{
    string Name { get; }

    string? Description { get; }

    IList<ICommandHandlerBuilder> Childs { get; }

    ICommandHandlerBuilder AddCommand<THandler>(string name, string? description = null)
        where THandler : class, ICommandHandler;
}
