using AutoMapper;
using xSdk.Shared;
using System.Text.Json;

namespace xSdk.Data.Converters.Mapper
{
    public static class JsonElementConverter
    {
        public sealed class ToModelProperty : IValueConverter<string, JsonElement>
        {
            public JsonElement Convert(string sourceMember, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(sourceMember))
                {
                    var json = Base64Helper.ConvertFromBase64(sourceMember.Trim());
                    return JsonDocument.Parse(json).RootElement;
                }

                return JsonDocument.Parse("{}").RootElement;
            }
        }

        public sealed class ToEntityProperty : IValueConverter<JsonElement, string>
        {
            public string Convert(JsonElement sourceMember, ResolutionContext context)
            {
                var json = sourceMember.GetRawText();
                return Base64Helper.ConvertToBase64(json);
            }
        }
    }
}
