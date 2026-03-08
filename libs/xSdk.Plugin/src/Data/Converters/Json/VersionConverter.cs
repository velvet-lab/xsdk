using System.Text.Json;
using System.Text.Json.Serialization;

namespace xSdk.Data.Converters.Json;

public sealed class VersionConverter : JsonConverter<Version>
{
    public override Version Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Version result = default;
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();
            if (!string.IsNullOrEmpty(value))
            {
                result = Version.Parse(value);
            }
        }
        return result;
    }

    public override void Write(Utf8JsonWriter writer, Version value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
