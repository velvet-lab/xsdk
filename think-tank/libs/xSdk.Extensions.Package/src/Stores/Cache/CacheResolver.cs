using Microsoft.Extensions.Logging;
using NuGet.Versioning;
using Sewer56.Update.Packaging.Structures;

namespace xSdk.Extensions.Package.Stores.Cache
{
    internal class CacheResolver(CacheManager cacheMgr, ILogger<CacheResolver> logger) : IStoreResolver
    {
        private readonly CacheManager _cacheMgr = cacheMgr;
        private readonly ILogger<CacheResolver> _logger = logger;
        private string _packageName;
        private string _packageDestination;
        private string _repository;
        private string _location;

        public void Configure(string repository, string location, string destination, string name)
        {
            _repository = repository;
            _location = location;
            _packageName = name;
            _packageDestination = destination;
        }

        public Task DownloadPackageAsync(
            NuGetVersion version,
            string destFilePath,
            ReleaseMetadataVerificationInfo verificationInfo,
            IProgress<double> progress = null,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation("Try to download package '{0}/{1}'", _packageName, version.OriginalVersion);

            var releaseInfo = _cacheMgr.GetReleaseInfoFromStore(_packageName, _repository, _location);
            if (releaseInfo != null)
            {
                var release = releaseInfo.Releases.FirstOrDefault(x => NuGetVersion.Parse(x.Version) == version);
                if (release != null)
                {
                    var storeRoot = _cacheMgr.GetStoreLocation(_repository, _location);
                    var file2download = Path.Combine(storeRoot, release.FileName);
                    if (File.Exists(file2download))
                    {
                        File.Copy(file2download, destFilePath, true);
                    }
                }
            }

            return Task.CompletedTask;
        }

        public Task<List<NuGetVersion>> GetPackageVersionsAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Try to download package versions for package '{0}'", _packageName);

            if (string.IsNullOrWhiteSpace(_packageName))
            {
                _logger.LogWarning("Missing package name to resolve. Empty version list will returned");
            }

            var result = new List<NuGetVersion>();

            var releaseInfo = _cacheMgr.GetReleaseInfoFromStore(_packageName, _repository, _location);
            if (releaseInfo != null)
            {
                foreach (var release in releaseInfo.Releases)
                {
                    result.Add(NuGetVersion.Parse(release.Version));
                }
            }
            return Task.FromResult(result);
        }
    }
}
