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

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace xSdk.Data
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4004:Collection properties should be readonly", Justification = "<Pending>")]
    public class Dependencies
    {
        [JsonPropertyName("module")]
        public Dictionary<string, string> Module { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("nuget")]
        public Dictionary<string, string> NuGet { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("package")]
        public Dictionary<string, string> Package { get; set; } = new Dictionary<string, string>();
    }
}
