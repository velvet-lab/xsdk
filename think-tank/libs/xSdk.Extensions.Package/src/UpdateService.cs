using Sewer56.Update;
using Sewer56.Update.Packaging.Extractors;
using Sewer56.Update.Packaging.Structures;

namespace xSdk.Extensions.Package
{
    internal class UpdateService(IList<IPackageProvider> providers) : IUpdateService
    {
        public async Task<bool> CheckForUpdatesAsync<TComponent>(CancellationToken token = default)
            where TComponent : class
        {
            var shouldUpdate = false;

            var assembly = typeof(TComponent).Assembly;
            var version = assembly.GetName().Version;

            foreach (var provider in providers)
            {
                var resolver = provider.GetResolver();

                using var manager = await UpdateManager<Empty>.CreateAsync(resolver, new ZipPackageExtractor());

                var result = await manager.CheckForUpdatesAsync(token);
                if (result.CanUpdate)
                {
                    shouldUpdate = true;
                    break;
                }
            }

            return shouldUpdate;
        }
    }
}
