using xSdk.Shared;
using System.Text.Json;

namespace xSdk.Data.Converters.Mapper
{
    public static class JsonElementConverter
    {
        public static JsonElement Convert(string sourceMember)
        {
            if (!string.IsNullOrEmpty(sourceMember))
            {
                var json = Base64Helper.ConvertFromBase64(sourceMember.Trim());
                return JsonDocument.Parse(json).RootElement;
            }

            return JsonDocument.Parse("{}").RootElement;
        }

        public static string Convert(JsonElement sourceMember)
        {
            var json = sourceMember.GetRawText();
            return Base64Helper.ConvertToBase64(json);
        }
    }
}
