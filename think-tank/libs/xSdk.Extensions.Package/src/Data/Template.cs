using System.Text.Json.Serialization;

namespace xSdk.Data
{
    public class Template
    {
        [JsonPropertyName("language")]
        public TemplateLanguage Language { get; set; }

        [JsonPropertyName("type")]
        public TemplateType Type { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
}
