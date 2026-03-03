using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Package.Providers.GitHub;
using xSdk.Extensions.Package.Providers.Local;
using xSdk.Extensions.Package.Providers.Nuget;
using xSdk.Extensions.Variable;

namespace xSdk.Extensions.Package
{
    public static class UpdateServiceBuilderExtensions
    {
        public static IUpdateServiceBuilder ValidateOnStartup(this IUpdateServiceBuilder builder)
        {
            return builder;
        }

        public static IUpdateServiceBuilder UseLocal(this IUpdateServiceBuilder builder) => builder.UseLocal(setup => { });

        public static IUpdateServiceBuilder UseLocal(this IUpdateServiceBuilder builder, Action<LocalSetup> setup)
        {
            if (builder is UpdateServiceBuilder concreteBuilder)
            {
                concreteBuilder.AddProvider(provider =>
                {
                    var setupImpl = provider.GetRequiredService<IVariableService>().GetSetup<LocalSetup>(true, true);

                    setup?.Invoke(setupImpl);

                    return new LocalProvider(setupImpl);
                });
            }

            return builder;
        }

        public static IUpdateServiceBuilder UseNuget(this IUpdateServiceBuilder builder) => builder.UseLocal(setup => { });

        public static IUpdateServiceBuilder UseNuget(this IUpdateServiceBuilder builder, Action<NugetSetup> setup)
        {
            if (builder is UpdateServiceBuilder concreteBuilder)
            {
                concreteBuilder.AddProvider(provider =>
                {
                    var nugetSetup = new NugetSetup();
                    setup?.Invoke(nugetSetup);

                    return new NugetPackageProvider(nugetSetup);
                });
            }
            return builder;
        }

        public static IUpdateServiceBuilder UseGitHub(this IUpdateServiceBuilder builder, Action<GitHubSetup> setup)
        {
            if (builder is UpdateServiceBuilder concreteBuilder)
            {
                concreteBuilder.AddProvider(provider =>
                {
                    var githubSetup = new GitHubSetup();
                    setup?.Invoke(githubSetup);

                    return new GitHubProvider(githubSetup);
                });
            }
            return builder;
        }
    }
}
