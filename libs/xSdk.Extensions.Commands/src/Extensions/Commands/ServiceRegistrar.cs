using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace xSdk.Extensions.Commands
{
    internal class ServiceRegistrar(IServiceCollection builder) : ITypeRegistrar
    {
        public ITypeResolver Build()
        {
            return new ServiceResolver(builder.BuildServiceProvider());
        }

        public void Register(Type service, Type implementation)
        {
            builder.AddSingleton(service, implementation);
        }

        public void RegisterInstance(Type service, object implementation)
        {
            builder.AddSingleton(service, implementation);
        }

        public void RegisterLazy(Type service, Func<object> factory)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            builder.AddSingleton(service, _ => factory());
        }
    }
}
