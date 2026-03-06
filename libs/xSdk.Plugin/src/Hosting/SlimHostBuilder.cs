using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Hosting
{
    public sealed class SlimHostBuilder
    {
        private Action<IServiceCollection> _configureServicesDelegate;
        private SlimHostBase host;

        public static SlimHostBuilder CreateBuilder<TSlimHost>()
            where TSlimHost : ISlimHost, new()
        {
            // Create the builder
            var builder = new SlimHostBuilder();
#nullable disable
            // Create the host
            builder.host = new TSlimHost() as SlimHostBase;
#nullable restore

            // return the builder
            return builder;
        }

        public ISlimHost PreBuild()
        {
            // Return a not fully initialized host
            return host;
        }

        public ISlimHost Build()
        {
            // Prepare and build service collection
            var services = new ServiceCollection();
            _configureServicesDelegate?.Invoke(services);

            // Save the provider to the host
            var provider = services.BuildServiceProvider();
            host.Configure(provider);

            // Configure only the fake SlimHost in the Abstractions Library
            SlimHost.Configure(host);

            // Return the fully initialized host
            return host;
        }

        public SlimHostBuilder ConfigureServices(Action<IServiceCollection> configureDelegate)
        {
            this._configureServicesDelegate = configureDelegate;

            return this;
        }

        public SlimHostBuilder ValidateAppPrefix(string? appPrefix, string defaultValue)
        {
            if (string.IsNullOrEmpty(appPrefix))
            {
                appPrefix = defaultValue;
            }

            if (host != null)
            {
                host.AppPrefix = appPrefix;
            }

            return this;
        }

        public SlimHostBuilder ValidateAppName(string? appName, string defaultValue)
        {
            if (string.IsNullOrEmpty(appName))
            {
                appName = defaultValue;
            }

            if (host != null)
            {
                host.AppName = appName;
            }

            return this;
        }

        public SlimHostBuilder ValidateAppCompany(string? appCompany, string defaultValue)
        {
            if (string.IsNullOrEmpty(appCompany))
            {
                appCompany = defaultValue;
            }

            if (host != null)
            {
                host.AppCompany = appCompany;
            }

            return this;
        }

        public SlimHostBuilder ValidateAppVersion(string? appVersion)
        {
            if (!string.IsNullOrEmpty(appVersion) && host != null)
            {
                host.AppVersion = appVersion;
            }

            return this;
        }
    }
}
