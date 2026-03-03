using System.Threading;
using System.Threading.Tasks;
using xSdk.Data;

namespace xSdk.Extensions.Package
{
    public interface IPackageJsonHandler
    {
        Task<PackageModel> LoadAsync(string source, CancellationToken token = default) => LoadAsync(source, true, token);

        Task<PackageModel> LoadAsync(string source, bool deepSearch, CancellationToken token = default);

        Task<PackageModel> LoadFromAsync(string source, string name, string version, CancellationToken token = default) =>
            LoadFromAsync(source, name, version, false, token);

        Task<PackageModel> LoadFromAsync(string source, string name, string version, bool deepSearch, CancellationToken token = default);
    }
}
