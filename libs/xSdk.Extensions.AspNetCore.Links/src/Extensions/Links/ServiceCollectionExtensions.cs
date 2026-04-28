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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace xSdk.Extensions.Links;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLinksService(this IServiceCollection services, Func<LinksOptions> optionsFactory)
    {
        services.AddHttpContextAccessor();
        services.TryAddSingleton<ILinksService>(provider =>
        {
            var options = optionsFactory?.Invoke();
            var service = ActivatorUtilities.CreateInstance<LinksService>(provider, options);

            return service;
        });

        return services;
    }
}
