using NuGet.Versioning;
using Sewer56.Update.Structures;
using xSdk.Data;

namespace xSdk.Extensions.Package
{
    public interface IStore
    {
        int Order { get; }

        Task<CheckForUpdatesResult> GetVersionsAsync(string name, string destination, CancellationToken token = default) =>
            GetVersionsAsync(name, destination, null, null, token);

        Task<CheckForUpdatesResult> GetVersionsAsync(string name, string destination, string repository, CancellationToken token = default) =>
            GetVersionsAsync(name, destination, null, null, token);

        Task<CheckForUpdatesResult> GetVersionsAsync(string name, string destination, string repository, string location, CancellationToken token = default);

        Task<PackageModel> DownloadPackageAsync(string name, string destination, CancellationToken token = default) =>
            DownloadPackageAsync(name, NuGetVersion.Parse("0.0.0"), destination, null, null, token);

        Task<PackageModel> DownloadPackageAsync(string name, NuGetVersion version, string destination, CancellationToken token = default) =>
            DownloadPackageAsync(name, version, destination, null, null, token);

        Task<PackageModel> DownloadPackageAsync(string name, NuGetVersion version, string destination, string repository, CancellationToken token = default) =>
            DownloadPackageAsync(name, version, destination, repository, null, token);

        Task<PackageModel> DownloadPackageAsync(
            string name,
            NuGetVersion version,
            string destination,
            string repository,
            string location,
            CancellationToken token = default
        );

        Task UploadPackageAsync(PackageModel sourcePackage, string version, string destination, CancellationToken token);
    }
}
