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

namespace xSdk.Hosting;

/// <summary>
/// Represents a builder for configuring AI agents within a hosting environment.
/// </summary>
public interface IHostedWorkflowBuilder
{
    /// <summary>
    /// Gets the name of the agent being configured.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the service collection for configuration.
    /// </summary>
    IServiceCollection ServiceCollection { get; }

    /// <summary>
    /// Gets the DI service lifetime used for the agent registration.
    /// </summary>
    ServiceLifetime Lifetime { get; }
}
