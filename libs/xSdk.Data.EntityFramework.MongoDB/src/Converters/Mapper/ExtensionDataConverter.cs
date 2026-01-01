using AutoMapper;
using System.Text.Json;

namespace xSdk.Data.Converters.Mapper
{
    public static class ExtensionDataConverter
    {
        public sealed class ToModelProperty : IValueConverter<string, IDictionary<string, object>>
        {
            public IDictionary<string, object> Convert(string sourceMember, ResolutionContext context)
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
        }

        public sealed class ToEntityProperty : IValueConverter<IDictionary<string, object>, string>
        {
            public string Convert(IDictionary<string, object> sourceMember, ResolutionContext context)
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
    }
}
