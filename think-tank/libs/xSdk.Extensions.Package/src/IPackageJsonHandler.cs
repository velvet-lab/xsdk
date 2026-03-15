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

using System.Threading;
using System.Threading.Tasks;
using xSdk.Data;

namespace xSdk.Extensions.Package
{
    public interface IPackageJsonHandler
    {
        Task<PackageModel> LoadAsync(string source, CancellationToken token = default) => LoadAsync(source, true, token);

        Task<PackageModel> LoadAsync(string source, bool deepSearch, CancellationToken token = default);

        Task<PackageModel> LoadFromAsync(string source, string name, string version, CancellationToken token = default) =>
            LoadFromAsync(source, name, version, false, token);

        Task<PackageModel> LoadFromAsync(string source, string name, string version, bool deepSearch, CancellationToken token = default);
    }
}
