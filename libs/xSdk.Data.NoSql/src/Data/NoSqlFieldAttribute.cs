using LiteDB;

namespace xSdk.Data;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class NoSqlFieldAttribute : BsonFieldAttribute
{
    public NoSqlFieldAttribute()
        : base() { }

    public NoSqlFieldAttribute(string name)
        : base(name) { }
}
