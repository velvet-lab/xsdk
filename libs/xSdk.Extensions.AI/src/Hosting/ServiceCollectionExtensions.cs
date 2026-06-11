using CommunityToolkit.Diagnostics;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace xSdk.Hosting;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Registers a custom workflow using a factory delegate.
        /// </summary>
        /// <param name="builder">The <see cref="IHostApplicationBuilder"/> to configure.</param>
        /// <param name="name">The unique name for the workflow.</param>
        /// <param name="factory">A factory function that creates the <see cref="Workflow"/> instance. The function receives the service provider and workflow name as parameters.</param>
        /// <param name="lifetime">The DI service lifetime for the workflow registration. Defaults to <see cref="ServiceLifetime.Singleton"/>.</param>
        /// <returns>An <see cref="IHostedWorkflowBuilder"/> that can be used to further configure the workflow.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/>, <paramref name="name"/>, or <paramref name="factory"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is empty.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the factory delegate returns null or a workflow with a name that doesn't match the expected name.
        /// </exception>
        public IHostedWorkflowBuilder AddWorkflow(string name, Func<IServiceProvider, string, Workflow> factory, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            Guard.IsNotNull(name);
            Guard.IsNotNull(factory);
            
            services.AddKeyedService(name, (sp, key) =>
            {
                Guard.IsNotNull(key);
                string? keyString = key as string;
                Guard.IsNotNullOrEmpty(keyString);

                Workflow workflow = factory(sp, keyString) ?? throw new InvalidOperationException($"The workflow factory did not return a valid {nameof(Workflow)} instance for key '{keyString}'.");
                if (!string.Equals(workflow.Name, keyString, StringComparison.Ordinal))
                {
                    throw new InvalidOperationException($"The workflow factory returned workflow with name '{workflow.Name}', but the expected name is '{keyString}'.");
                }

                return workflow;
            }, lifetime);

            return new HostedWorkflowBuilder(name, services, lifetime);
        }

        /// <summary>
        /// Registers a keyed service with the specified lifetime.
        /// </summary>
        internal void AddKeyedService<T>(object? serviceKey, Func<IServiceProvider, object?, T> factory, ServiceLifetime lifetime)
            where T : class
        {
            var descriptor = new ServiceDescriptor(typeof(T), serviceKey, (sp, key) => factory(sp, key), lifetime);
            services.Add(descriptor);
        }
    }
}
