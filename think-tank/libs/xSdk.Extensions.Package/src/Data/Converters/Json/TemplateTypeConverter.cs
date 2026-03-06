using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using xSdk.Data;

namespace xSdk.Data.Converters.Json
{
    internal sealed class TemplateTypeConverter : JsonConverter<TemplateType>
    {
        public override TemplateType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = TemplateType.None;
            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();
                if (!string.IsNullOrEmpty(value))
                {
                    result = Enum.Parse<TemplateType>(value, true);
                }
            }
            return result;
        }

        public override void Write(Utf8JsonWriter writer, TemplateType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
