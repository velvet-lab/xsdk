using System.Text.Json;
using System.Text.Json.Serialization;

namespace xSdk.Data.Converters.Json;

public sealed class StageConverter : JsonConverter<Stage>
{
    public override Stage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var result = Stage.None;
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();
            if (!string.IsNullOrEmpty(value))
            {
                result = Enum.Parse<Stage>(value, true);
            }
        }
        return result;
    }

    public override void Write(Utf8JsonWriter writer, Stage value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
