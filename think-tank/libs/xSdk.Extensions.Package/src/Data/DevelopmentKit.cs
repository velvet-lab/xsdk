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
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace xSdk.Data
{
    public sealed class DevelopmentKit
    {
        [JsonPropertyName("artifactory")]
        public Artifactory Artifactory { get; set; }

        [JsonPropertyName("template")]
        public Template Template { get; set; } = new Template();

        [JsonPropertyName("dependencies")]
        public Dependencies Dependencies { get; set; } = new Dependencies();

        [JsonPropertyName("excludes")]
        public IEnumerable<string> Excludes { get; set; }

        public bool IsEmpty()
        {
            if (Template == null)
            {
                return true;
            }
            else
            {
                if (Template.Type == TemplateType.None || Template.Language == TemplateLanguage.None)
                {
                    return true;
                }

                if (string.IsNullOrEmpty(Template.Version))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
