using MongoDB.Bson;

namespace xSdk.Data.Converters.Mapper;

public static class ObjectIdConverter
{
    public static string Convert(ObjectId sourceMember)
    {
        if (TryConvert(sourceMember, out string result))
        {
            return result;
        }
        return default;
    }
    public static ObjectId Convert(string sourceMember)
    {
        if (TryConvert(sourceMember, out ObjectId result))
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
