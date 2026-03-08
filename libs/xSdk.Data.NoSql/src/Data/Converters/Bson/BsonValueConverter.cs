using LiteDB;

namespace xSdk.Data.Converters.Bson;

public static class BsonValueConverter
{
    public static BsonValue Convert(object value)
    {
        if (value is bool)
            return new BsonValue(System.Convert.ToBoolean(value));
        else if (value is int)
            return new BsonValue(System.Convert.ToInt32(value));
        else if (value is decimal)
            return new BsonValue(System.Convert.ToDecimal(value));
        else if (value is double)
            return new BsonValue(System.Convert.ToDouble(value));
        else if (value is Guid)
            return new BsonValue(Guid.Parse(value.ToString()));
        else if (value is long)
            return new BsonValue(System.Convert.ToInt64(value));
        else if (value is string)
            return new BsonValue(System.Convert.ToString(value));
        else if (value is ObjectId)
            return new BsonValue(value as ObjectId);
        else if (value is DateTime)
            return new BsonValue(DateTime.Parse(value.ToString()));
        else
            throw new SdkException("Value is not a convertable Bson Value");
    }
}
