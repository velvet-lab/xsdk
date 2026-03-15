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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using xSdk.Data;

namespace xSdk.Data
{
    public class PackageModel : Model
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public string NativeName => CleanPackageName(Name);

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("keywords")]
        public IEnumerable<string> Keywords { get; set; }

        [JsonPropertyName("os")]
        public IEnumerable<string> Os { get; set; }

        [JsonPropertyName("private")]
        public bool Private { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("checksums")]
        public Dictionary<string, string> Checksums { get; set; }

        [JsonPropertyName("vah")]
        public AutomationHub AutomationHub { get; set; } = new AutomationHub();

        [JsonPropertyName("vdk")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual DevelopmentKit DevelopmentKit { get; set; } = new DevelopmentKit();

        [JsonPropertyName("dependencies")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4004:Collection properties should be readonly", Justification = "<Pending>")]
        public Dictionary<string, string> Dependencies { get; set; }

        [JsonPropertyName("devDependencies")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4004:Collection properties should be readonly", Justification = "<Pending>")]
        public Dictionary<string, string> DevDependencies { get; set; }

        [JsonIgnore]
        public string Root { get; internal set; }

        [JsonIgnore]
        public string MetaFileName => Path.Combine(Path.GetFullPath(Path.Combine(Root, "..")), $"{NativeName}.json");

        [JsonIgnore]
        public string BuildFileName => Path.Combine(Path.GetFullPath(Path.Combine(Root, "..")), $"{NativeName}{Version}.zip");

        public override string ToString() => Name;

        private string CleanPackageName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var patterns = new Dictionary<string, string> { { "/", "." }, { "-", "." } };

                foreach (var kvp in patterns)
                {
                    if (name.IndexOf(kvp.Key, StringComparison.InvariantCultureIgnoreCase) != -1)
                    {
                        name = name.Replace(kvp.Key, kvp.Value, StringComparison.InvariantCultureIgnoreCase);
                    }
                }
            }

            if (!string.IsNullOrEmpty(name))
            {
                name = CreateCamelCase(name);
                name = name.Trim();
            }
            return name;
        }

        private string CreateCamelCase(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var result = new List<string>();

                var splitted = name.Split(".", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var split in splitted)
                {
                    var left = split.Substring(0, 1);
                    var right = split.Substring(1);
                    result.Add($"{left.ToUpper()}{right}");
                }
                name = result.Aggregate((a, b) => a + "." + b);
            }
            return name;
        }
    }
}
