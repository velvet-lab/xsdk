using xSdk.Extensions.Plugin;
using xSdk.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Spectre.Console.Cli;


namespace xSdk.Extensions.Commands
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandServices(this IServiceCollection services, Action<IConfigurator> configureDelegate)
        {
            services.TryAddSingleton<ICommandApp>(provider =>
            {
                var app = new CommandApp<DefaultCommand>(new ServiceRegistrar(services));
                return AddCommandServicesInternal(provider, app, configureDelegate);
            });

            return services;
        }

        public static IServiceCollection AddCommandServices<TDefaultCommand>(this IServiceCollection services, Action<IConfigurator> configureDelegate)
            where TDefaultCommand : class, ICommand
        {
            services.TryAddSingleton<ICommandApp>(provider =>
            {
                var app = new CommandApp<TDefaultCommand>(new ServiceRegistrar(services));
                return AddCommandServicesInternal(provider, app, configureDelegate);
            });

            return services;
        }

        private static ICommandApp AddCommandServicesInternal(IServiceProvider provider, ICommandApp app, Action<IConfigurator> configureDelegate)
        {
            app.Configure(config =>
            {
                configureDelegate?.Invoke(config);

                var plugins = SlimHost.Instance.PluginSystem.Invoke<ICommandLinePluginBuilder>(x => x.ConfigureCommandLine(config));
            });

            return app;
        }

        public static IConfigurator AddReplConsole(this IConfigurator config, Action<IReplBuilder> builderDelegate)
        {
            config.AddCommand<ConsoleCommand>(ConsoleCommand.Definitions.Name);
            config.AddCommand<ClearCommand>(ClearCommand.Definitions.Name);
            config.AddCommand<ExitCommand>(ExitCommand.Definitions.Name);

            HostExtensions.ReplBuilderDelegate = builderDelegate;

            return config;
        }
    }
}
