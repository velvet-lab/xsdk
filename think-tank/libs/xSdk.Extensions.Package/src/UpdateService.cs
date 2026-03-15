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

using Sewer56.Update;
using Sewer56.Update.Packaging.Extractors;
using Sewer56.Update.Packaging.Structures;

namespace xSdk.Extensions.Package
{
    internal class UpdateService(IList<IPackageProvider> providers) : IUpdateService
    {
        public async Task<bool> CheckForUpdatesAsync<TComponent>(CancellationToken token = default)
            where TComponent : class
        {
            var shouldUpdate = false;

            var assembly = typeof(TComponent).Assembly;
            var version = assembly.GetName().Version;

            foreach (var provider in providers)
            {
                var resolver = provider.GetResolver();

                using var manager = await UpdateManager<Empty>.CreateAsync(resolver, new ZipPackageExtractor());

                var result = await manager.CheckForUpdatesAsync(token);
                if (result.CanUpdate)
                {
                    shouldUpdate = true;
                    break;
                }
            }

            return shouldUpdate;
        }
    }
}
