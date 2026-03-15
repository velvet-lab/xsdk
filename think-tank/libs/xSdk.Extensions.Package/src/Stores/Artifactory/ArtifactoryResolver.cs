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
using Sewer56.Update.Packaging.Structures;

namespace xSdk.Extensions.Package.Stores.Artifactory
{
    // Is only a Dummy Resolver. Its only needed
    internal class ArtifactoryResolver : IStoreResolver
    {
        public ArtifactoryResolver() { }

        public void Configure(string repository, string location, string destination, string packageName) { }

        public async Task DownloadPackageAsync(
            NuGetVersion version,
            string destFilePath,
            ReleaseMetadataVerificationInfo verificationInfo,
            IProgress<double> progress = null,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public async Task<List<NuGetVersion>> GetPackageVersionsAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
