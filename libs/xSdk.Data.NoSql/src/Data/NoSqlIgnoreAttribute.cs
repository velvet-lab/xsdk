using LiteDB;

namespace xSdk.Data;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class NoSqlIgnoreAttribute : BsonIgnoreAttribute
{
    public NoSqlIgnoreAttribute()
        : base() { }
}
