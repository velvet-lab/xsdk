using LiteDB;
using xSdk.Data.Converters.Mapper;

namespace xSdk.Data;

internal class NoSqlEntityPK : PrimaryKey<ObjectId>
{
    private readonly object _syncObject = new();

    public NoSqlEntityPK()
        : base(ObjectId.NewObjectId()) { }

    public NoSqlEntityPK(ObjectId initialValue)
        : base(initialValue) { }

    public NoSqlEntityPK(string intialValue)
        : base(new ObjectId(intialValue)) { }

    protected override TType Convert<TType>(object value)
    {
        lock (_syncObject)
        {
            if (ObjectIdConverter.TryConvert(value, out ObjectId result))
            {
                if (typeof(TType) == typeof(ObjectId))
                {
                    return (TType)(object)result;
                }
                else if (typeof(TType) == typeof(string))
                {
                    return (TType)(object)result.ToString();
                }
            }
            else if (ObjectIdConverter.TryConvert(value, out string resultString))
            {
                if (typeof(TType) == typeof(ObjectId))
                {
                    return (TType)(object)new ObjectId(resultString);
                }
                else if (typeof(TType) == typeof(string))
                {
                    return (TType)(object)resultString;
                }
            }
        }
        return default;
    }
}
