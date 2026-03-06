using Sewer56.Update.Interfaces;
using Sewer56.Update.Resolvers;
using Sewer56.Update.Structures;

namespace xSdk.Extensions.Package.Providers.Local
{
    internal class LocalProvider(LocalSetup setup) : IPackageProvider
    {
        public IPackageResolver GetResolver()
        {
            return new LocalPackageResolver(setup.Path, new CommonPackageResolverSettings { AllowPrereleases = true });
        }
    }
}
