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

namespace xSdk.Hosting;

// Remarks: This SlimHost is only for Abstractions Library and should not be used in any other library.
// The Real SlimHost is implemented in the xSdk library and will constructed with the Builder Pattern
internal static class SlimHost
{
    private static ISlimHost _slimHost;

    internal static void Configure(ISlimHost host)
    {
        _slimHost = host;
    }

    internal static ISlimHost Instance => _slimHost ?? throw new InvalidOperationException("SlimHost has not been initialized.");
}
