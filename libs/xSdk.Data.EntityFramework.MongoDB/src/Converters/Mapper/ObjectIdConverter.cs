using AutoMapper;
using MongoDB.Bson;

namespace xSdk.Data.Converters.Mapper
{
    public static class ObjectIdConverter
    {
        public sealed class ToModelProperty : IValueConverter<ObjectId, string>
        {
            public string Convert(ObjectId sourceMember, ResolutionContext context)
            {
                if (TryConvert(sourceMember, out string result))
                {
                    return result;
                }
                return default;
            }
        }

        public sealed class ToEntityProperty : IValueConverter<string, ObjectId>
        {
            public ObjectId Convert(string sourceMember, ResolutionContext context)
            {
                if (TryConvert(sourceMember, out ObjectId result))
                {
                    return result;
                }

                return default;
            }
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

        internal static bool TryConvert(object value, out ObjectId converted)
        {
            converted = default;
            if (value != null && value is string stringValue)
            {
                converted = ObjectId.Parse(stringValue.Trim());
                return true;
            }
            return false;
        }
    }
}
