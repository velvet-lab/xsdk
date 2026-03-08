using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using xSdk.Shared;

namespace xSdk.Data.Converters.Json;

public sealed class SemVerConverter : JsonConverter<SemVer>
{
    public override SemVer Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (Base64Helper.IsBase64(value))
        {
            var converted = Base64Helper.ConvertFromBase64(value);
            var splitted = converted.Split(";", StringSplitOptions.RemoveEmptyEntries);

            return new SemVer(splitted[0], splitted[1]);
        }
        else
        {
            return new SemVer(value);
        }
    }

    public override void Write(Utf8JsonWriter writer, SemVer value, JsonSerializerOptions options)
    {
        var tmp = $"{value.Version};{value.Range}";
        writer.WriteBase64StringValue(Encoding.UTF8.GetBytes(tmp));
    }
}
