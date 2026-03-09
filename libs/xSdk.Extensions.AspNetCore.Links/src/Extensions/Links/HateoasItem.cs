using System.Text.Json.Serialization;

namespace xSdk.Extensions.Links;

internal class HateoasItem : IHateoasItem
{
    [JsonPropertyName("rel")]
    public string Rel { get; set; } = string.Empty;

    [JsonPropertyName("href")]
    public string Href { get; set; } = string.Empty;

    [JsonPropertyName("method")]
    public string Method { get; set; } = string.Empty;
}
