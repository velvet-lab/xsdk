using AutoMapper;

namespace xSdk.Data.Converters.Mapper
{
    public static class GuidConverter
    {
        public sealed class ToModelProperty : IValueConverter<string, Guid>
        {
            public Guid Convert(string sourceMember, ResolutionContext context)
            {
                if (TryConvert(sourceMember, out Guid result))
                {
                    return result;
                }

                return default;
            }
        }

        public sealed class ToEntityProperty : IValueConverter<Guid, string>
        {
            public string Convert(Guid sourceMember, ResolutionContext context)
            {
                if (TryConvert(sourceMember, out string result))
                {
                    return result;
                }
                return default;
            }
        }

        internal static bool TryConvert(object value, out Guid converted)
        {
            converted = default;
            if (value != null && value is string stringValue)
            {
                converted = Guid.Parse(stringValue);
                return true;
            }
            return false;
        }

        internal static bool TryConvert(object value, out string converted)
        {
            converted = default;
            if (value != null && value is Guid guidValue)
            {
                converted = guidValue.ToString();
                return true;
            }
            return false;
        }
    }
}
