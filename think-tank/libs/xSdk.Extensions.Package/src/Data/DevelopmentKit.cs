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
