using NuGet.Versioning;
using Sewer56.Update.Interfaces;
using Sewer56.Update.Packaging.Structures;

namespace xSdk.Extensions.Package.Resolvers.Artifactory
{
    internal class ArtifactoryResolver : IPackageResolver
    {
        public Task DownloadPackageAsync(
            NuGetVersion version,
            string destFilePath,
            ReleaseMetadataVerificationInfo verificationInfo,
            IProgress<double>? progress = null,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<List<NuGetVersion>> GetPackageVersionsAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
