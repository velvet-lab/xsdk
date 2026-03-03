using Sewer56.Update.Interfaces;

namespace xSdk.Extensions.Package
{
    public interface IStoreResolver : IPackageResolver
    {
        void Configure(string repository, string location, string destination, string name);
    }
}
