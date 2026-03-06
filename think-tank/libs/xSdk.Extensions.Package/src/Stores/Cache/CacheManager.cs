using System.IO;
using System.Security.Permissions;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using xSdk.Data;
using xSdk.Shared;

namespace xSdk.Extensions.Package.Stores.Cache
{
    internal class CacheManager(PackageSetup setup, ILogger<CacheManager> logger)
    {
        private readonly PackageSetup _setup = setup;
        private readonly ILogger<CacheManager> _logger = logger;

        internal string GetStoreLocation(PackageModel package)
        {
            var repository = Globals.DefaultRepository;
            var location = Globals.DefaultLocation;
            if (package.DevelopmentKit.Artifactory != null)
            {
                repository = package.DevelopmentKit.Artifactory.Repository;
                location = package.DevelopmentKit.Artifactory.Location;
            }
            return GetStoreLocation(repository, location);
        }

        internal string GetStoreLocation(string repository, string location)
        {
            if (string.IsNullOrEmpty(repository))
            {
                repository = Globals.DefaultRepository;
            }

            if (string.IsNullOrEmpty(location))
            {
                location = Globals.DefaultLocation;
            }

            var rootLocation = Path.Combine(_setup.Cache, repository, location);
            if (!Directory.Exists(rootLocation))
            {
                Directory.CreateDirectory(rootLocation);
            }

            return rootLocation;
        }

        internal ReleaseInfo GetReleaseInfoFromStore(PackageModel sourcePackage)
        {
            var rootLocation = GetStoreLocation(sourcePackage);
            var metaFileName = Path.Combine(rootLocation, $"{sourcePackage.NativeName}.json");
            return GetReleaseInfoFromStore(metaFileName);
        }

        internal ReleaseInfo GetReleaseInfoFromStore(string packageName, string repository, string location)
        {
            var rootLocation = GetStoreLocation(repository, location);
            var metaFileName = Path.Combine(rootLocation, $"{packageName}.json");
            return GetReleaseInfoFromStore(metaFileName);
        }

        private ReleaseInfo GetReleaseInfoFromStore(string metaFileName)
        {
            var result = new ReleaseInfo();
            if (File.Exists(metaFileName))
            {
                result = JsonSerializer.Deserialize<ReleaseInfo>(File.ReadAllText(metaFileName), JsonHelper.GetSerializerOptions());
            }

            return result;
        }
    }
}
