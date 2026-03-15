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

// using xSdk.Data;
// using Microsoft.Extensions.Logging;
// using Sewer56.Update.Packaging;
// using Sewer56.Update.Packaging.Structures.ReleaseBuilder;
// using System.IO;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;

// namespace xSdk.Extensions.Package
// {
//     internal partial class PackageService
//     {
//         public async Task CompactPackageAsync(PackageModel sourcePackage, string version, string destination, CancellationToken token = default)
//         {
//             _logger.LogInformation("Create new package");

//             var destinationPath = Path.Combine(destination, sourcePackage.NativeName);
//             var destinationSourcePath = Path.Combine(destinationPath, version);
//             var outputPackage = await _packageJsonHandler.LoadAsync(destinationSourcePath, true);

//             _logger.LogInformation("Create new package");

//             if (File.Exists(outputPackage.MetaFileName))
//             {
//                 _logger.LogDebug("Remove existing meta file");
//                 File.Delete(outputPackage.MetaFileName);
//             }

//             if (File.Exists(outputPackage.BuildFileName))
//             {
//                 _logger.LogDebug("Remove existing compressed file");
//                 File.Delete(outputPackage.BuildFileName);
//             }

//             _logger.LogTrace("Copy all sources for the release");
//             var builder = new ReleaseBuilder<PackageModel>();
//             builder.AddCopyPackage(new CopyBuilderItem<PackageModel>
//             {
//                 FolderPath = destinationSourcePath,
//                 Version = outputPackage.Version,
//                 IgnoreRegexes = sourcePackage.DevelopmentKit.Excludes?.ToList()
//             });

//             _logger.LogTrace("Build the release");
//             await builder.BuildAsync(new BuildArgs
//             {
//                 FileName = $"{outputPackage.NativeName}",
//                 OutputFolder = destinationPath,
//                 JsonCompressionMode = Sewer56.Update.Packaging.IO.JsonCompression.None,
//                 MetadataFileName = $"{outputPackage.NativeName}.json"
//             });
//         }
//     }
// }
