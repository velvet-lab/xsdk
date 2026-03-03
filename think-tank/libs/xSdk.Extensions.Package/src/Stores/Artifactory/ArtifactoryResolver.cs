using NuGet.Versioning;
using Sewer56.Update.Packaging.Structures;

namespace xSdk.Extensions.Package.Stores.Artifactory
{
    // Is only a Dummy Resolver. Its only needed
    internal class ArtifactoryResolver : IStoreResolver
    {
        public ArtifactoryResolver() { }

        public void Configure(string repository, string location, string destination, string packageName) { }

        public async Task DownloadPackageAsync(
            NuGetVersion version,
            string destFilePath,
            ReleaseMetadataVerificationInfo verificationInfo,
            IProgress<double> progress = null,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public async Task<List<NuGetVersion>> GetPackageVersionsAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
