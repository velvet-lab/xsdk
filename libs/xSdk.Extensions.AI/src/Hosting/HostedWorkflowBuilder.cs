using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Hosting;

internal sealed class HostedWorkflowBuilder(string name, IServiceCollection serviceCollection, ServiceLifetime lifetime = ServiceLifetime.Singleton) : IHostedWorkflowBuilder
{
    public string Name { get; } = name;

    public IServiceCollection ServiceCollection { get; } = serviceCollection;

    public ServiceLifetime Lifetime { get; } = lifetime;
}
