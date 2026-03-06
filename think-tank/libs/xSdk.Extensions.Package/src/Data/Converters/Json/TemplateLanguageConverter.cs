using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using xSdk.Data;

namespace xSdk.Data.Converters.Json
{
    internal sealed class TemplateLanguageConverter : JsonConverter<TemplateLanguage>
    {
        public override TemplateLanguage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = TemplateLanguage.None;
            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();
                if (!string.IsNullOrEmpty(value))
                {
                    result = Enum.Parse<TemplateLanguage>(value, true);
                }
            }
            return result;
        }

        public override void Write(Utf8JsonWriter writer, TemplateLanguage value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
