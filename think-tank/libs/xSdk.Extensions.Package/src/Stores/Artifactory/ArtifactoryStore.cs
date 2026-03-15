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
//using System.Text;
//using System.Text.Json;
//using xSdk.Data;
//using xSdk.Extensions.Web;

//namespace xSdk.Extensions.Package.Stores.Artifactory
//{
//    internal class ArtifactoryStore : AbstractStore<ArtifactoryResolver>
//    {
//        private readonly IPackageJsonHandler _packageJsonHandler;
//        private readonly ArtifactoryManager _artifactoryMgr;
//        private readonly ILogger<ArtifactoryStore> _logger;

//        public ArtifactoryStore(IPackageJsonHandler packageJsonHandler,
//            ArtifactoryManager artifactoryMgr,
//            ArtifactoryResolver resolver,
//            ILogger<ArtifactoryStore> logger) : base(resolver, packageJsonHandler, logger)
//        {
//            _packageJsonHandler = packageJsonHandler;
//            _artifactoryMgr = artifactoryMgr;
//            _logger = logger;
//        }

//        public override int Order => Globals.ArtifactoryStoreOrderNumber;

//        public override async Task<PackageModel> DownloadPackageAsync(string name, NuGetVersion version, string destination, string repository, string location, CancellationToken token = default)
//        {
//            _logger.LogInformation("Try to download package '{0}/{1}'", name, version.OriginalVersion);
//            var artifact = await SearchNameByPropertyAsync(name, version, repository, token);
//            if (artifact != null)
//            {
//                var metaName = $"{name}.json";
//                var metaFilePath = Path.Combine(destination, metaName);

//                var packageName = $"{name}{version}.zip";
//                var packageFilePath = Path.Combine(destination, packageName);

//                using (var client = HttpClientBuilder.CreateHttpClient())
//                {
//                    using (var stream = await client.GetStreamAsync(artifact.DownLoadUri))
//                    {
//                        using (var fileStream = new FileStream(packageFilePath, FileMode.CreateNew))
//                        {
//                            await stream.CopyToAsync(fileStream);
//                        }
//                    }
//                }

//                var releaseInfo = await _artifactoryMgr.GetReleaseInfoFromStoreAsync(name, destination, repository, token);
//                if (releaseInfo != null)
//                {
//                    var releaseInfoContent = JsonSerializer.Serialize(releaseInfo, JsonHelper.GetSerializerOptions());
//                    await File.WriteAllTextAsync(metaFilePath, releaseInfoContent, Encoding.UTF8, token);
//                }
//            }

//            // Always return null, because after download from artifactory to cache, Cachedownloader has to start
//            return null;
//        }

//        public override async Task UploadPackageAsync(PackageModel sourcePackage, string version, string destination, CancellationToken token)
//        {
//            var destinationPath = Path.Combine(destination, sourcePackage.NativeName, version);
//            var outputPackage = await _packageJsonHandler.LoadAsync(destinationPath, token);

//            var (repository, location) = _artifactoryMgr.ExtractRepositoryAndLocation(sourcePackage);

//            var releaseInfo = await _artifactoryMgr.GetReleaseInfoFromStoreAsync(outputPackage, repository, token);
//            UpdateReleaseInfo(releaseInfo, outputPackage);
//            var releasInfoContent = JsonSerializer.Serialize(releaseInfo, JsonHelper.GetSerializerOptions());
//            File.WriteAllText(outputPackage.MetaFileName, releasInfoContent, Encoding.UTF8);

//            _logger.LogInformation("Upload release to artifactory");
//            _logger.LogTrace("Upload release information");
//            if (File.Exists(outputPackage.MetaFileName))
//            {
//                await _artifactoryMgr.UploadAsync(repository, location, outputPackage.MetaFileName, token);
//            }
//            else
//            {
//                _logger.LogWarning("Release meta information could not uploaded, because file not exists");
//            }

//            _logger.LogTrace("Upload binary release");
//            if (File.Exists(outputPackage.BuildFileName))
//            {
//                var properties = CollectUploadProperties(sourcePackage, outputPackage);
//                await _artifactoryMgr.UploadAsync(repository, location, outputPackage.BuildFileName, properties, token);
//            }
//            else
//            {
//                _logger.LogWarning("Release binary could not uploaded, because file not exists");
//            }
//        }

//        private Dictionary<string, string> CollectUploadProperties(PackageModel sourcePackage, PackageModel outputPackage)
//        {
//            var result = new Dictionary<string, string>
//            {
//                { "name", sourcePackage.Name },
//                { "native", sourcePackage.NativeName },
//                { "version", outputPackage.Version }
//            };

//            if (!string.IsNullOrEmpty(sourcePackage.Description))
//            {
//                result.Add("description", sourcePackage.Description);
//            }

//            if (sourcePackage.Os != null && sourcePackage.Os.Any())
//            {
//                result.Add("os", sourcePackage.Os.Aggregate((a, b) => a.ToLower() + ";" + b.ToLower()));
//            }

//            if (sourcePackage.DevelopmentKit != null)
//            {
//                var template = sourcePackage.DevelopmentKit.Template;
//                if (template != null)
//                {
//                    if (template.Language != TemplateLanguage.None)
//                    {
//                        result.Add("language", template.Language.ToString().ToLower());
//                    }

//                    if (template.Type != TemplateType.None)
//                    {
//                        result.Add("type", template.Type.ToString().ToLower());
//                    }

//                    if (!string.IsNullOrEmpty(template.Version))
//                    {
//                        result.Add("template", template.Version?.ToString());
//                    }
//                }

//                var artifactory = sourcePackage.DevelopmentKit.Artifactory;
//                if (artifactory != null)
//                {
//                    if (artifactory.Properties != null && artifactory.Properties.Any())
//                    {
//                        foreach (var kvp in artifactory.Properties)
//                        {
//                            result.Add(kvp.Key, kvp.Value);
//                        }
//                    }
//                }
//            }

//            if (sourcePackage.Keywords != null && sourcePackage.Keywords.Any())
//            {
//                result.Add("keywords", sourcePackage.Keywords.Aggregate((a, b) => a.ToLower() + ";" + b.ToLower()));
//            }

//            return result;
//        }

//        private async Task<ArtifactModel> SearchNameByPropertyAsync(string name, NuGetVersion version, string repository, CancellationToken token)
//        {
//            _logger.LogInformation("Try to find package '{0}' by name", name);
//            var properties = new Dictionary<string, string> {
//                { "name", name },
//                { "version", version.ToString() },
//            };

//            var list = await _artifactoryMgr.SearchByPropertiesAsync(repository, properties, token);
//            var artifact = ValidateSearchResult(list);
//            if (artifact == null)
//            {
//                _logger.LogInformation("Try to find package '{0}' by native name", name);
//                properties = new Dictionary<string, string> {
//                    { "native", name },
//                    { "version", version.ToString() },
//                };
//                list = await _artifactoryMgr.SearchByPropertiesAsync(repository, properties, token);
//                artifact = ValidateSearchResult(list);
//            }

//            return artifact;
//        }

//        private ArtifactModel ValidateSearchResult(ArtifactModelList list)
//        {
//            if (list != null)
//            {
//                if (list.Artifacts.Count > 1)
//                {
//                    throw new SdkException("More than one package found. Specify a filter to get a specific version");
//                }

//                var artifact = list.Artifacts.FirstOrDefault();

//                if (artifact != null)
//                {
//                    return artifact;
//                }
//            }
//            return null;
//        }
//    }
//}
