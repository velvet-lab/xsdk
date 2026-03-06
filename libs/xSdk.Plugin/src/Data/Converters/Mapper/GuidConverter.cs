using System.ComponentModel;

namespace xSdk.Data.Converters.Mapper
{
    public static class GuidConverter
    {
        public static Guid Convert(string value)
        {
            if (TryConvert(value, out Guid result))
            {
                return result;
            }

            return default;
        }

        public static string Convert(Guid value)
        {
            if (TryConvert(value, out string result))
            {
                return result;
            }

            return default;
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
