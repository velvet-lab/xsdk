using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Extensions.Commands;

public interface IApplicationBuilder
{
    string? Description { get; }

    IApplicationBuilder AddCommand<THandler>(string name, string? description = default)
        where THandler : class, ICommandHandler;

    ICommandHandlerBuilder AddBranch(string name, string? description = default);

    IApplicationBuilder SetDescription(string description);

    void Build(IServiceCollection services);
}

