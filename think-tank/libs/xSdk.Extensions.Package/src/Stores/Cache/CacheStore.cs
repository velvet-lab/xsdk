//using Microsoft.Extensions.Logging;
//using System.Text;
//using System.Text.Json;
//using xSdk.Data;

//namespace xSdk.Extensions.Package.Stores.Path
//{
//    internal class CacheStore : AbstractStore<CacheResolver>
//    {
//        private readonly IPackageJsonHandler _packageJsonHandler;
//        private readonly CacheManager _cacheMgr;
//        private readonly ILogger<CacheStore> _logger;

//        public CacheStore(IPackageJsonHandler packageJsonHandler, PackageSetup setup, CacheResolver resolver, CacheManager cacheMgr, ILogger<CacheStore> logger)
//            : base(resolver, packageJsonHandler, logger)
//        {
//            _packageJsonHandler = packageJsonHandler;
//            _cacheMgr = cacheMgr;
//            _logger = logger;
//        }

//        public override int Order => 100;

//        public override async Task UploadPackageAsync(PackageModel sourcePackage, string version, string destination, CancellationToken token)
//        {
//            var outputPackage = await _packageJsonHandler.LoadFromAsync(destination, sourcePackage.NativeName, version, token);

//            var releaseInfo = _cacheMgr.GetReleaseInfoFromStore(sourcePackage);
//            UpdateReleaseInfo(releaseInfo, outputPackage);

//            var content = JsonSerializer.Serialize(releaseInfo, JsonHelper.GetSerializerOptions());
//            await File.WriteAllTextAsync(outputPackage.MetaFileName, content, Encoding.UTF8);

//            var storeRoot = _cacheMgr.GetStoreLocation(sourcePackage);

//            Deploy(outputPackage.MetaFileName, storeRoot);
//            Deploy(outputPackage.BuildFileName, storeRoot);
//        }


//        private void Deploy(string fileName, string destination)
//        {
//            FileInfo fi = new FileInfo(fileName);
//            if (fi.Exists)
//            {
//                var destinationFileName = Path.Combine(destination, fi.Name);
//                fi.CopyTo(destinationFileName, true);
//            }
//        }
//    }
//}
