using System.Text.Json;

namespace xSdk.Data.Converters.Mapper;

public static class ExtensionDataConverter
{
    public static IDictionary<string, object> Convert(string sourceMember)
    {
        if (!string.IsNullOrEmpty(sourceMember))
        {
            var options = JsonHelper.GetSerializerOptions();
            options.WriteIndented = false;

            var result = JsonSerializer.Deserialize<IDictionary<string, object>>(sourceMember, options);
            return result;
        }
        return null;
    }

    public static string Convert(IDictionary<string, object> sourceMember)
    {
        if (sourceMember != null)
        {
            var options = JsonHelper.GetSerializerOptions();
            options.WriteIndented = false;

            var json = JsonSerializer.Serialize(sourceMember, options);
            return json;
        }
        return null;
    }
}
