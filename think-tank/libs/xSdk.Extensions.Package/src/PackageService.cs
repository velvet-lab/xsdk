// using Microsoft.Extensions.Logging;
// using NuGet.Versioning;
// using xSdk.Data;
// using xSdk.Extensions.Package.Stores.Artifactory;
// using xSdk.Extensions.Package.Stores.Path;

// namespace xSdk.Extensions.Package
// {
//     internal partial class PackageService(PackageSetup packageSetup, IPackageJsonHandler packageJsonHandler, CacheManager cacheMgr, ArtifactoryManager artifactoryMgr, EnvironmentSetup envSetup, ILogger<PackageService> logger) : IPackageService
//     {
//         private readonly IPackageJsonHandler _packageJsonHandler = packageJsonHandler;
//         private readonly CacheManager _cacheMgr = cacheMgr;
//         private readonly ArtifactoryManager _artifactoryMgr = artifactoryMgr;
//         private readonly EnvironmentSetup _envSetup = envSetup;
//         private readonly ILogger<PackageService> _logger = logger;

//         private IDictionary<string, IStore> _stores;

//         internal void BuildStores(IDictionary<string, IStore> stores)
//             => _stores = stores;

//         public async Task<PackageModel> DownloadPackageAsync(string name, string version, string destination, string repository, string location, CancellationToken token = default)
//         {
//             NuGetVersion nugetVersion = null;
//             if(!string.IsNullOrEmpty(version))
//             {
//                 try
//                 {
//                     nugetVersion = NuGetVersion.Parse(version);
//                 }
//                 catch
//                 {
//                     nugetVersion = await RetrieveHighestVersionAsync(name, version, destination, repository, location, token);
//                 }
//             }
//             else
//             {
//                 if(_envSetup.Stage != Stage.Production)
//                 {
//                     // All Stages because of Production highest Version will searched
//                     nugetVersion = await RetrieveHighestVersionAsync(name, version, destination, repository, location, token);
//                 }
//             }

//             if(nugetVersion == null)
//             {
//                 throw new AutomationException(string.Format("Can not determine version from current release for artefact {0}", name));
//             }

//             return await DownloadPackageAsync(name, nugetVersion, destination, repository, location, token);
//         }

//         public async Task<PackageModel> DownloadPackageAsync(string name, NuGetVersion version, string destination, string repository, string location, CancellationToken token = default)
//         {
//             // Logik: Zuerst wird im lokalen Path gesucht, sollte da das gewünschte Package nicht gefunden werden
//             // wird es aus dem Artifactory downgeloaded. Sollte aus dem Artifactory runtergeladen werden müssen, muss aus
//             // dem Path nochmal geladen werden

//             _logger.LogInformation("Try to download package {0}/{1}", name, version);
//             var cacheStore = _stores.Values.FirstOrDefault(x => x is CacheStore);

//             _logger.LogDebug("Try to download package from local cache");
//             var package = await cacheStore.DownloadPackageAsync(name, version, destination, repository, location, token);
//             if (package == null)
//             {
//                 var artifactoryStore = _stores.Values.FirstOrDefault(x => x is ArtifactoryStore);
//                 if (artifactoryStore != null)
//                 {
//                     _logger.LogDebug("Package was not found in local cache, download package from artifactory");
//                     var cacheDestination = _cacheMgr.GetStoreLocation(repository, location);
//                     await artifactoryStore.DownloadPackageAsync(name, version, cacheDestination, repository, location, token);

//                     _logger.LogDebug("Package found in artifacotry. Retry to download package from local cache");
//                     package = await cacheStore.DownloadPackageAsync(name, version, destination, repository, location, token);
//                     if (package == null)
//                     {
//                         _logger.LogWarning("Package {0}/{1} not found in Path and Artifactory. Is Package uploaded?", name, version);
//                     }
//                 }
//                 else
//                 {
//                     _logger.LogWarning("Package {0}/{1} not found in Path and Artifactory is not enabled, so we could not take a look to artifactory", name, version);
//                 }
//             }
//             return package;
//         }

//         public async Task UploadPackageAsync(PackageModel sourcePackage, string version, string destination, CancellationToken token = default)
//         {
//             foreach (var store in _stores.Values
//                 .OrderBy(x => x.Order))
//             {
//                 await store.UploadPackageAsync(sourcePackage, version, destination,  token);
//             }
//         }

//         private async Task<NuGetVersion> RetrieveHighestVersionAsync(string name, string version, string destination, string repository, string location, CancellationToken token = default)
//         {
//             var tempVersion = new SemVer(version);
//             NuGetVersion result = null;

//             // Angefordertes Artefakt zuerst versuchen aus dem Path zu laden
//             var releaseInfo = _cacheMgr.GetReleaseInfoFromStore(name, repository, location);
//             if (releaseInfo == null || !releaseInfo.Releases.Any())
//             {
//                 _logger.LogInformation("Requested Artefact {0} Version {1} can no be loaded from the local cache", name, version);
//                 _logger.LogInformation("Try to receive requested artefact now remotely from the global artifactory");

//                 releaseInfo = await _artifactoryMgr.GetReleaseInfoFromStoreAsync(name, destination, repository);
//                 if (releaseInfo == null || !releaseInfo.Releases.Any())
//                 {
//                     throw new AutomationException(string.Format("Requested Artefact {0} Version {1} can not be loaded from the global artifactory", name, version));
//                 }
//             }

//             // Höchste Version aus der Release.json ermitteln
//             var highestVersion = tempVersion.MaxSatisfying(releaseInfo.Releases.Select(x => new SemVer(x.Version)), _envSetup.Stage != Stage.Production);
//             if (highestVersion != null)
//             {
//                 _logger.LogInformation("Found artefact {0} in stores. Version info (Version={1}PreRelease={0})", name, highestVersion.Version, highestVersion.IsPreRelease);
//                 result = NuGetVersion.Parse(highestVersion.Version);
//             }

//             return result;
//         }
//     }
// }
