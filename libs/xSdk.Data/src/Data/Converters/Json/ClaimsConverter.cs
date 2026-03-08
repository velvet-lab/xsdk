using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace xSdk.Data.Converters.Json;

public sealed class ClaimsConverter : JsonConverter<IEnumerable<Claim>>
{
    public override IEnumerable<Claim> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var result = new List<Claim>();

        if (reader.TokenType != JsonTokenType.StartArray)
            return result;

        Dictionary<string, string?> tmp = null;
        string propertyName = default;
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
                tmp = new Dictionary<string, string?>();

            if (reader.TokenType == JsonTokenType.EndObject)
            {
                if (tmp != null && tmp.Any())
                {
                    var claim = ConvertToClaim(tmp);
                    if (claim != null)
                        result.Add(claim);
                }
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
                propertyName = reader.GetString();

            if (reader.TokenType == JsonTokenType.String && !string.IsNullOrEmpty(propertyName))
            {
                tmp.Add(propertyName, reader.GetString());
                propertyName = default;
            }
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<Claim> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
        {
            writer.WriteStartObject();
            writer.WriteString("type", item.Type);
            writer.WriteString("value", item.Value);
            writer.WriteString("valueType", item.ValueType);
            writer.WriteString("originalIssuer", item.OriginalIssuer);
            writer.WriteString("issuer", item.Issuer);
            writer.WriteEndObject();
        }
        writer.WriteEndArray();
    }

    private Claim? ConvertToClaim(Dictionary<string, string?> values)
    {
        values.TryGetValue("type", out string? type);
        values.TryGetValue("value", out string value);
        values.TryGetValue("valueType", out string valueType);
        values.TryGetValue("issuer", out string issuer);
        values.TryGetValue("originalIssuer", out string originalIssuer);

        if (string.IsNullOrEmpty(type))
            return null;

        if (string.IsNullOrEmpty(value))
            return null;

        if (string.IsNullOrEmpty(valueType) && string.IsNullOrEmpty(issuer) && string.IsNullOrEmpty(originalIssuer))
            return new Claim(type, value);

        if (!string.IsNullOrEmpty(valueType) && string.IsNullOrEmpty(issuer) && string.IsNullOrEmpty(originalIssuer))
            return new Claim(type, value, valueType);

        if (!string.IsNullOrEmpty(valueType) && !string.IsNullOrEmpty(issuer) && string.IsNullOrEmpty(originalIssuer))
            return new Claim(type, value, valueType, issuer);

        if (!string.IsNullOrEmpty(valueType) && !string.IsNullOrEmpty(issuer) && !string.IsNullOrEmpty(originalIssuer))
            return new Claim(type, value, valueType, issuer, originalIssuer);

        return null;
    }
}
