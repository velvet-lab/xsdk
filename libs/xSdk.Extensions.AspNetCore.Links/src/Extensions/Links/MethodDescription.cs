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

using System.Reflection;

namespace xSdk.Extensions.Links;

internal sealed class MethodDescription
{
    public required MethodInfo Action { get; internal set; }

    public required Type ControllerType { get; internal set; }

    public required HttpMethod HttpMethod { get; internal set; }

    public string? MethodName { get; internal set; }

    public string? PolicyName { get; internal set; }

    public string? RouteTemplate { get; internal set; }

    public string[] AuthRoles { get; internal set; } = Array.Empty<string>();

    public string? AuthPolicy { get; internal set; }

    internal bool ShouldAuthorize
    {
        get
        {
            if (AuthRoles.Any() || !string.IsNullOrEmpty(AuthPolicy))
            {
                return true;
            }
            return false;
        }
    }

    public override string ToString()
    {
        return MethodName ?? this.GetType().Name;
    }
}
