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

using NuGet.Versioning;
using Sewer56.Update.Structures;
using xSdk.Data;

namespace xSdk.Extensions.Package
{
    public interface IStore
    {
        int Order { get; }

        Task<CheckForUpdatesResult> GetVersionsAsync(string name, string destination, CancellationToken token = default) =>
            GetVersionsAsync(name, destination, null, null, token);

        Task<CheckForUpdatesResult> GetVersionsAsync(string name, string destination, string repository, CancellationToken token = default) =>
            GetVersionsAsync(name, destination, null, null, token);

        Task<CheckForUpdatesResult> GetVersionsAsync(string name, string destination, string repository, string location, CancellationToken token = default);

        Task<PackageModel> DownloadPackageAsync(string name, string destination, CancellationToken token = default) =>
            DownloadPackageAsync(name, NuGetVersion.Parse("0.0.0"), destination, null, null, token);

        Task<PackageModel> DownloadPackageAsync(string name, NuGetVersion version, string destination, CancellationToken token = default) =>
            DownloadPackageAsync(name, version, destination, null, null, token);

        Task<PackageModel> DownloadPackageAsync(string name, NuGetVersion version, string destination, string repository, CancellationToken token = default) =>
            DownloadPackageAsync(name, version, destination, repository, null, token);

        Task<PackageModel> DownloadPackageAsync(
            string name,
            NuGetVersion version,
            string destination,
            string repository,
            string location,
            CancellationToken token = default
        );

        Task UploadPackageAsync(PackageModel sourcePackage, string version, string destination, CancellationToken token);
    }
}
