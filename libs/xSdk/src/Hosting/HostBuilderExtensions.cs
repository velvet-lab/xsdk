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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public static class HostBuilderExtensions
{
    private const string SlimHostKey = "xSdk.Hosting.SlimHost";

    extension(IHostBuilder builder)
    {
        public IHostBuilder AddHost<THostedService>()
            where THostedService : class, IHostedService
            => builder.AddHost<THostedService>(null);

        public IHostBuilder AddHost<THostedService>(Func<IServiceProvider, THostedService>? implementationFactory)
            where THostedService : class, IHostedService
        {
            builder
                .ConfigureServices(services =>
                {
                    if (implementationFactory is not null)
                    {
                        services.AddHostedService<THostedService>(implementationFactory);
                    }
                    else
                    {
                        services.AddHostedService<THostedService>();
                    }
                });

            return builder;
        }

        /// <summary>
        /// Retrieves the SlimHost instance bound to this builder.
        /// </summary>
        public SlimHost GetSlimHost()
        {
            if (builder.Properties.TryGetValue(SlimHostKey, out object? value) && value is SlimHost slimHost)
            {
                return slimHost;
            }

            throw new SdkException("No SlimHost found on this IHostBuilder. Ensure InitializeSlimHost was called first.");
        }

        public IHostBuilder RegisterPluginHost<TPluginHost>()
            where TPluginHost : class, IPluginHost
        {
            builder
                .GetSlimHost()
                .RegisterPluginHost<IPluginHost, TPluginHost>();

            return builder;
        }

        public IHostBuilder RegisterPluginHostOptions<TOptions>()
            where TOptions : class, IVariableSetup
        {
            builder
                .GetSlimHost()
                .RegisterPluginHostOptions<TOptions>(null);

            return builder;
        }

        public IHostBuilder RegisterPluginHostOptions<TOptions>(Action<TOptions> configureOptions)
            where TOptions : class, IVariableSetup
        {
            builder
                .GetSlimHost()
                .RegisterPluginHostOptions<TOptions>(configureOptions);

            return builder;
        }

        public IHostBuilder RegisterPluginBuilder<TPluginBuilder, TPluginBuilderImplementation>()
            where TPluginBuilder : class, IPluginBuilder
            where TPluginBuilderImplementation : class, TPluginBuilder
        {
            builder
                .GetSlimHost()
                .RegisterPluginBuilder<TPluginBuilder, TPluginBuilderImplementation>();

            return builder;
        }

        public IHostBuilder RegisterServices(Action<IServiceCollection> configureServices)
        {
            var slimHost = builder.GetSlimHost();

            slimHost.RegisterPluginServices(configureServices);
            slimHost.RegisterHostServices(configureServices);

            return builder;
        }


        public IHostBuilder RegisterPluginServices(Action<IServiceCollection> configureServices)
        {
            builder
                .GetSlimHost()
                .RegisterPluginServices(configureServices);

            return builder;
        }

        public IHostBuilder RegisterHostServices(Action<IServiceCollection> configureServices)
        {
            builder
                .GetSlimHost()
                .RegisterHostServices(configureServices);

            return builder;
        }

        /// <summary>
        /// Stores the SlimHost instance in the builder's Properties dictionary,
        /// binding it to this specific builder so parallel builds stay isolated.
        /// </summary>
        internal IHostBuilder SetSlimHost(SlimHost slimHost)
        {
            builder.Properties[SlimHostKey] = slimHost;
            return builder;
        }
    }
}
