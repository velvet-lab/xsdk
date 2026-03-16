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

namespace xSdk.Extensions.Package
{
    internal class UpdateServiceBuilder(IServiceProvider provider) : IUpdateServiceBuilder
    {
        internal IList<Func<IServiceProvider, IPackageProvider>> registrations = new List<Func<IServiceProvider, IPackageProvider>>();

        internal void AddProvider(Func<IServiceProvider, IPackageProvider> action) => registrations.Add(action);

        public IUpdateService Build()
        {
            var packageProviders = new List<IPackageProvider>();
            foreach (var registration in registrations)
            {
                var packageProvider = registration(provider);
                if (packageProvider != null)
                {
                    packageProviders.Add(packageProvider);
                }
            }

            return new UpdateService(packageProviders);
        }
    }
}
