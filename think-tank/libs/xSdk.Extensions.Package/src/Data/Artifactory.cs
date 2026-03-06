using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace xSdk.Data
{
    public sealed class Artifactory
    {
        public Artifactory()
        {
            Properties = new Dictionary<string, string>();
        }

        [JsonPropertyName("repository")]
        public string Repository { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("properties")]
        public Dictionary<string, string> Properties { get; set; }
    }
}
