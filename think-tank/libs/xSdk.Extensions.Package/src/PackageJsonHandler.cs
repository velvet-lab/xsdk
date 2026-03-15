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

// using Microsoft.Extensions.Logging;
// using System.Text.Json;
// using xSdk.Data;
// using xSdk.Data.Converters.Json;

// namespace xSdk.Extensions.Package
// {
//     internal class PackageJsonHandler(ILogger<PackageJsonHandler> logger) : IPackageJsonHandler
//     {
//         private readonly ILogger<PackageJsonHandler> _logger = logger;

//         public async Task<PackageModel> LoadAsync(string source, bool deepSearch, CancellationToken token = default)
//         {
//             _logger.LogInformation("Try to load package.json from source '{0}'", source);
//             var packageJsonFile = TryToFindPackageJson(source, deepSearch);
//             if (File.Exists(packageJsonFile))
//             {
//                 _logger.LogTrace("Package.json found. Process loading");
//                 var packageJsonContent = await File.ReadAllTextAsync(packageJsonFile, token);

//                 var package = Parse(packageJsonContent);
//                 package.Root = AbsolutePath.Create(source);
//                 return package;
//             }
//             else
//             {
//                 _logger.LogWarning("Package.json could not found at '{0}'", source);
//             }

//             return null;
//         }

//         public async Task<PackageModel> LoadFromAsync(string source, string name, string version, bool deepSearch, CancellationToken token = default)
//         {
//             var packagePath = Path.Combine(source, name);
//             var packageJsonPath = Path.Combine(packagePath, version);
//             return await LoadAsync(packageJsonPath, deepSearch, token);
//         }

//         internal static PackageModel DeepClone(PackageModel package)
//         {
//             var options = JsonHelper.GetSerializerOptions();
//             var content = JsonSerializer.Serialize(package, options);
//             return Parse(content);
//         }

//         private static PackageModel Parse(string content)
//         {
//             var options = JsonHelper.GetSerializerOptions();
//             options.Converters.Add(new TemplateLanguageConverter());
//             options.Converters.Add(new TemplateTypeConverter());

//             return JsonSerializer.Deserialize<PackageModel>(content, options);
//         }

//         private string TryToFindPackageJson(string source, bool deepSearch)
//         {
//             var packageJsonFile = Path.Combine(source, "package.json");
//             if (File.Exists(packageJsonFile))
//             {
//                 return packageJsonFile;
//             }

//             if (deepSearch)
//             {
//                 var files = Directory.GetFiles(source, "package.json", SearchOption.AllDirectories);
//                 if (files.Any() && files.Count() == 1)
//                 {
//                     return files.FirstOrDefault();
//                 }
//             }

//             return null;
//         }
//     }
// }
