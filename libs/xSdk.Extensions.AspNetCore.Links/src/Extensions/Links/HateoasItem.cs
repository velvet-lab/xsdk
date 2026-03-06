using System.Text.Json.Serialization;

namespace xSdk.Extensions.Links
{
    internal class HateoasItem : IHateoasItem
    {
        [JsonPropertyName("rel")]
        public string Rel { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("method")]
        public string Method { get; set; }
    }
}
