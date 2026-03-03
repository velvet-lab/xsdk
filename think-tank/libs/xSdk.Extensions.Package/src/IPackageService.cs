using System.Threading;
using System.Threading.Tasks;
using NuGet.Versioning;
using xSdk.Data;

namespace xSdk.Extensions.Package
{
    public interface IPackageService
    {
        Task BuildPackageAsync(PackageModel package, string version, string destination, CancellationToken token = default);

        Task CompactPackageAsync(PackageModel package, string version, string destination, CancellationToken token = default);

        Task UploadPackageAsync(PackageModel package, string version, string destination, CancellationToken token = default);

        Task<PackageModel> DownloadPackageAsync(string name, string destination, CancellationToken token = default) =>
            DownloadPackageAsync(name, NuGetVersion.Parse("0.0.0"), destination, null, null, token);

        Task<PackageModel> DownloadPackageAsync(string name, string version, string destination, CancellationToken token = default) =>
            DownloadPackageAsync(name, version, destination, null, null, token);

        Task<PackageModel> DownloadPackageAsync(string name, NuGetVersion version, string destination, CancellationToken token = default) =>
            DownloadPackageAsync(name, version, destination, null, null, token);

        Task<PackageModel> DownloadPackageAsync(string name, string version, string destination, string repository, CancellationToken token = default) =>
            DownloadPackageAsync(name, version, destination, repository, null, token);

        Task<PackageModel> DownloadPackageAsync(string name, NuGetVersion version, string destination, string repository, CancellationToken token = default) =>
            DownloadPackageAsync(name, version, destination, repository, null, token);

        Task<PackageModel> DownloadPackageAsync(
            string name,
            string version,
            string destination,
            string repository,
            string location,
            CancellationToken token = default
        );

        Task<PackageModel> DownloadPackageAsync(
            string name,
            NuGetVersion version,
            string destination,
            string repository,
            string location,
            CancellationToken token = default
        );
    }
}
