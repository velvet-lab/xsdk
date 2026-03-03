//using Microsoft.Extensions.Logging;
//using System.Text.Json;
//using System.Text.RegularExpressions;
//using xSdk.Data;

//namespace xSdk.Extensions.Package
//{
//    internal partial class PackageService
//    {
//        public async Task BuildPackageAsync(PackageModel sourcePackage, string version, string destination, CancellationToken token = default)
//        {
//            // This source folders will builded
//            var sources = new string[]
//            {
//                "src",
//                "dist"
//            };

//            _logger.LogTrace("Prepare output directory");
//            var destinationPath = Path.Combine(destination, sourcePackage.NativeName, version);
//            if(Directory.Exists(destinationPath))
//            {
//                _logger.LogTrace("Destination already exists. Delete it");
//                Directory.Delete(destinationPath, true);
//            }

//            _logger.LogInformation("Build package '{0}/{1}'", sourcePackage.Name, version);
//            foreach (var source in sources)
//            {
//                var sourcePath = Path.Combine(sourcePackage.Root, source);
//                var destinationSourcePath = Path.Combine(destinationPath, source);

//                if (Directory.Exists(sourcePath))
//                {
//                    _logger.LogTrace("Package will deployed at '{0}'", destinationPath);
//                    FileSystemTasks.CopyDirectoryRecursively(sourcePath, destinationSourcePath, DirectoryExistsPolicy.Merge, FileExistsPolicy.Overwrite,
//                        dir =>
//                        {
//                            if (sourcePackage.DevelopmentKit.Excludes != null && sourcePackage.DevelopmentKit.Excludes.Any())
//                            {
//                                foreach (var exclude in sourcePackage.DevelopmentKit.Excludes)
//                                {
//                                    var regex = new Regex(exclude);
//                                    if (regex.IsMatch(dir.Name))
//                                    {
//                                        _logger.LogInformation("Directory '{0}' is excluded and will not builded", dir.FullName);
//                                        return true;
//                                    }
//                                }
//                            }
//                            return false;
//                        },
//                        file =>
//                        {
//                            if (sourcePackage.DevelopmentKit.Excludes != null && sourcePackage.DevelopmentKit.Excludes.Any())
//                            {
//                                foreach (var exclude in sourcePackage.DevelopmentKit.Excludes)
//                                {
//                                    var regex = new Regex(exclude);
//                                    if (regex.IsMatch(file.Name))
//                                    {
//                                        _logger.LogInformation("File '{0}' is excluded and will not builded", file.FullName);
//                                        return true;
//                                    }
//                                }
//                            }
//                            return false;
//                        });
//                }
//                else
//                {
//                    _logger.LogInformation("Folder '{0}' will not builded because it does not exist", sourcePath);
//                }
//            }

//            _logger.LogInformation("Build package.json");
//            var newPackage = PackageJsonHandler.DeepClone(sourcePackage);

//            _logger.LogTrace("Create checksums");
//            var checksums = CreateChecksums(newPackage, destinationPath);
//            newPackage.Checksums = checksums;

//            _logger.LogTrace("Fix monorepo versions in dependencies");
//            newPackage.AutomationHub.Dependencies.FixVersions(version);

//            _logger.LogTrace("Set version and creation date");
//            newPackage.Version = version;
//            newPackage.Created = DateTime.Now;
//            newPackage.PrimaryKey = Random.Shared.Next(1, Int32.MaxValue);

//            _logger.LogTrace("Delete some package.json entries");
//            newPackage.DevelopmentKit = null;
//            newPackage.DevDependencies = null;

//            _logger.LogInformation("Write package.json");
//            var jsonText = JsonSerializer.Serialize(newPackage, JsonHelper.GetSerializerOptions());
//            var packageJsonFile = Path.Combine(destinationPath, "package.json");
//            await File.WriteAllTextAsync(packageJsonFile, jsonText, token);

//            _logger.LogInformation("Build for package '{0}/{1}' is done.", newPackage.Name, version);
//        }

//        private Dictionary<string, string> CreateChecksums(PackageModel package, string source)
//        {
//            var files = Directory.GetFiles(source, "*", SearchOption.AllDirectories);

//            var result = new Dictionary<string, string>();
//            foreach (var file in files)
//            {
//                var relativePath = Path.GetRelativePath(source, file);
//                if (string.Compare(relativePath, "package.json", true) != -1)
//                {
//                    var index = relativePath.Replace(@"\", "/");
//                    using (var stream = File.OpenRead(file))
//                    {
//                        var hash = HashTools.CreateHash(stream);
//                        result[index] = hash;
//                    }
//                }
//            }

//            return result;
//        }
//    }
//}
