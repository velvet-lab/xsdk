/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

//using Microsoft.Extensions.Logging;
//using NuGet.Versioning;
//using Sewer56.Update;
//using Sewer56.Update.Interfaces;
//using Sewer56.Update.Packaging.Extractors;
//using Sewer56.Update.Structures;
//using xSdk.Data;
//using xSdk.Extensions.Variable;

//namespace xSdk.Extensions.Package.Stores
//{
//    internal abstract class AbstractStore<TResolver>(TResolver resolver,
//                                                     IPackageJsonHandler packageJsonHandler,
//                                                     ILogger logger) : IStore
//        where TResolver : IStoreResolver
//    {
//        private readonly TResolver _resolver = resolver;
//        private readonly IPackageJsonHandler _packageJsonHandler = packageJsonHandler;
//        private readonly ILogger _logger = logger;

//        public abstract int Order { get; }

//        public virtual async Task<PackageModel> DownloadPackageAsync(string name, NuGetVersion version, string destination, string repository, string location, CancellationToken token = default)
//        {
//            _logger.LogInformation("Try to download package '{0}' with version '{1}'", name, version.OriginalVersion);

//            try
//            {
//                _consoleSvc
//                    .SetMessage("Try to download package '{0}' with version '{1}'", name, version.OriginalVersion)
//                    .WriteProgress();

//                var currentVersion = "0.0.0";
//                var application = new ItemMetadata(NuGetVersion.Parse(currentVersion), destination);

//                _resolver.Configure(repository, location, destination, name);

//                using var manager = await UpdateManager<PackageModel>.CreateAsync(application, _resolver as IPackageResolver, new ZipPackageExtractor());
//                var processOptions = new OutOfProcessOptions
//                {
//                    Restart = false
//                };

//                var updateOptions = new UpdateOptions
//                {
//                    CleanupAfterUpdate = false
//                };

//                await manager.PrepareUpdateAsync(version, CreateProgress($"Download package '{name}'"), token);
//                await manager.StartUpdateAsync(version, processOptions, updateOptions);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogWarning("Package {0}/{1} could not downloaded", name, version);
//            }
//            finally
//            {
//                _consoleSvc.StopProgress();
//            }

//            var package = await _packageJsonHandler.LoadAsync(destination, false, token);
//            return package;
//        }

//        public async Task<CheckForUpdatesResult> GetVersionsAsync(string name, string destination, string repository, string location, CancellationToken token = default)
//        {
//            var currentVersion = "0.0.0";
//            var application = new ItemMetadata(NuGetVersion.Parse(currentVersion), destination);

//            _resolver.Configure(repository, location, destination, name);

//            using var manager = await UpdateManager<PackageModel>.CreateAsync(application, _resolver as IPackageResolver, new ZipPackageExtractor());
//            return await manager.CheckForUpdatesAsync();
//        }




//        public abstract Task UploadPackageAsync(PackageModel sourcePackage, string version, string destination, CancellationToken token);


//        protected void UpdateReleaseInfo(ReleaseInfo info, PackageModel package)
//        {
//            if (File.Exists(package.BuildFileName))
//            {
//                var file = new FileInfo(package.BuildFileName);
//                if (!info.Releases.Any(x => x.FileName == file.Name))
//                {
//                    info.Releases.Add(new Release
//                    {
//                        FileName = file.Name,
//                        Version = package.Version
//                    });
//                }
//            }
//            else
//            {
//                throw new SdkException("Package build file could not found");
//            }
//        }

//        private IProgress<double> CreateProgress(string message)
//        {
//            var envSetup = VariableServiceFactory
//                .CreateService()
//                .EnableSetup<EnvironmentSetup>()
//                .Setup;

//            if ((envSetup.Mode == RunningMode.Tool
//               || envSetup.Mode == RunningMode.Console
//               || envSetup.Mode == RunningMode.PowerCli)
//               && !envSetup.IsConsoleDisabled)
//            {
//                if (string.IsNullOrEmpty(message))
//                {
//                    _consoleSvc
//                        .SetMessage(message)
//                        .WriteProgress();
//                    return new ConsoleProgress(_consoleSvc);
//                }
//            }
//            return null;
//        }
//    }
//}
