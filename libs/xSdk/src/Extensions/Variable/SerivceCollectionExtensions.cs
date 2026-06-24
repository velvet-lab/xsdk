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

namespace xSdk.Extensions.Variable;

public static class SerivceCollectionExtensions
{
    public static IServiceCollection AddVariableServices(this IServiceCollection services)
    {
        // TryAddSingleton: when PostConfigure already registered a factory-based instance
        // (e.g. the main host bridging from SlimHost), this call becomes a no-op so the
        // factory registration is not overwritten by a plain new VariableService.
        services.TryAddSingleton<IVariableService, VariableService>();
        return services;
    }
}
