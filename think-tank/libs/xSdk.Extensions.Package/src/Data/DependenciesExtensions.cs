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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xSdk.Data
{
    public static class DependenciesExtensions
    {
        public static void Merge(this Dependencies dependencies, Dependencies source)
        {
            Merge(source.NuGet, dependencies.NuGet);
            Merge(source.Module, dependencies.Module);
            Merge(source.Package, dependencies.Package);
        }

        public static void FixVersions(this Dependencies dependencies, string version)
        {
            FixVersions(dependencies.NuGet, version);
            FixVersions(dependencies.Module, version);
            FixVersions(dependencies.Package, version);
        }

        private static void FixVersions(Dictionary<string, string> dependencies, string version)
        {
            foreach (var kvp in dependencies)
            {
                if (string.Compare(kvp.Value, "monorepo", true) == 0)
                {
                    dependencies[kvp.Key] = version;
                }
            }
        }

        private static void Merge(Dictionary<string, string> source, Dictionary<string, string> target)
        {
            foreach (var item in source)
            {
                if (!target.ContainsKey(item.Key))
                {
                    target.Add(item.Key, item.Value);
                }
                else
                {
                    var existingVersion = new SemVer(target[item.Key]);
                    var newVersion = new SemVer(item.Value);

                    if (newVersion > existingVersion)
                    {
                        target[item.Key] = item.Value;
                    }
                }
            }
        }

        public static string CreateExportConfig(this Dependencies dependencies, PackageType type)
        {
            var sb = new StringBuilder();

            if (type == PackageType.Nuget)
            {
                if (dependencies.NuGet.Any())
                {
                    sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    sb.AppendLine("<packages>");
                    foreach (var item in dependencies.NuGet)
                    {
                        sb.AppendLine($"\t<package id=\"{item.Key}\" version=\"{item.Value}\" />");
                    }
                    sb.AppendLine("</packages>");
                }
            }
            else if (type == PackageType.Module)
            {
                if (dependencies.Module.Any())
                {
                    sb.AppendLine("@{");
                    sb.AppendLine("\tPSDependOptions = @{");
                    sb.AppendLine("\t\tTarget = \"$DependencyFolder\"");
                    sb.AppendLine("\t\tParameters = @{");
                    sb.AppendLine("\t\t\tRepository = \"PowerShellGallery\"");
                    sb.AppendLine("\t\t\tSkipPublisherCheck = $true");
                    sb.AppendLine("\t\t}");
                    sb.AppendLine("\t}");
                    foreach (var item in dependencies.Module)
                    {
                        sb.AppendLine($"\t\"{item.Key}\" = \"{item.Value}\"");
                    }
                    sb.AppendLine("}");
                }
            }
            else
            {
                throw new NotSupportedException($"Export for PackageType '{type}' is not supported");
            }

            return sb.ToString();
        }
    }
}
