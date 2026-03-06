using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using xSdk.Extensions.Package.Providers.GitHub;
using xSdk.Extensions.Package.Providers.Local;
using xSdk.Extensions.Package.Providers.Nuget;
using xSdk.Extensions.Package.Stores.Artifactory;
using xSdk.Extensions.Package.Stores.Cache;
using xSdk.Extensions.Variable;

namespace xSdk.Extensions.Package
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUpdateServices(this IServiceCollection services, Action<IUpdateServiceBuilder> configureDelegate)
        {
            services.AddVariableServices();

            services.TryAddSingleton<IUpdateService>(provider =>
            {
                var variableService = provider.GetRequiredService<IVariableService>();
                variableService.RegisterSetup<LocalSetup>();
                variableService.RegisterSetup<GitHubSetup>();
                variableService.RegisterSetup<NugetSetup>();

                var builder = new UpdateServiceBuilder(provider);
                configureDelegate?.Invoke(builder);

                return builder.Build();
            });
            return services;
        }

        //public static IServiceCollection AddPackageServices(this IServiceCollection services)
        //    => services.AddPackageServices(null);

        //public static IServiceCollection AddPackageServices(this IServiceCollection services, Action<PackageSetup> configure)
        //{
        //    services
        //        .AddVariableServices(builder =>
        //        {
        //            builder.RegisterSetup(configure);
        //        });

        //    services.TryAddSingleton<IPackageRegistry, Registry>();

        //    services.TryAddSingleton<IPackageJsonHandler, PackageJsonHandler>();

        //    services.TryAddSingleton<ArtifactoryManager>();
        //    services.TryAddSingleton<ArtifactoryResolver>();
        //    services.TryAddKeyedSingleton<IStore, ArtifactoryStore>(nameof(ArtifactoryStore));

        //    services.TryAddSingleton<CacheResolver>();
        //    services.TryAddSingleton<CacheManager>();
        //    services.TryAddKeyedSingleton<IStore, CacheStore>(nameof(CacheStore));

        //    services.TryAddSingleton<IPackageService>(provider =>
        //    {
        //        var packageService = ActivatorUtilities.CreateInstance<PackageService>(provider);

        //        var setup = provider.GetRequiredService<IVariableService>().EnableSetup<PackageSetup>().Setup;
        //        var stores = new Dictionary<string, IStore>();

        //        if (!setup.DisableArtifactory)
        //        {
        //            stores.Add(nameof(ArtifactoryStore), provider.GetRequiredKeyedService<IStore>(nameof(ArtifactoryStore)));
        //        }

        //        if (!setup.DisableCache)
        //        {
        //            stores.Add(nameof(CacheStore), provider.GetRequiredKeyedService<IStore>(nameof(CacheStore)));
        //        }

        //        packageService.BuildStores(stores);

        //        return packageService;
        //    });

        //    return services;
        //}
    }
}
