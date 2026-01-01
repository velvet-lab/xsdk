using AutoMapper;
using LiteDB;

namespace xSdk.Data.Converters.Mapper
{
    public sealed class PKObjectIdToString : IValueConverter<ObjectId, string>
    {
        public string Convert(ObjectId sourceMember, ResolutionContext context)
        {
            if (TryConvert(sourceMember, out string result))
            {
                return result;
            }
            return default;
        }

        internal static bool TryConvert(object value, out string converted)
        {
            converted = default;
            if (value != null && value is ObjectId objectIdValue)
            {
                converted = objectIdValue.ToString().Trim();
                return true;
            }
            return false;
        }
    }
}
