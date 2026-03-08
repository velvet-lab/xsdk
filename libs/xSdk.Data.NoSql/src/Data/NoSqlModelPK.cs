using LiteDB;
using xSdk.Data.Converters.Mapper;

namespace xSdk.Data;

internal class NoSqlModelPK : PrimaryKey<string>
{
    private readonly object syncObject = new();

    public NoSqlModelPK()
        : base(ObjectId.NewObjectId().ToString()) { }

    public NoSqlModelPK(ObjectId initialValue)
        : base(initialValue.ToString()) { }

    public NoSqlModelPK(string intialValue)
        : base(new ObjectId(intialValue).ToString()) { }

    protected override TType Convert<TType>(object value)
    {
        lock (syncObject)
        {
            if (ObjectIdConverter.TryConvert(value, out string resultAsString))
            {
                if (typeof(TType) == typeof(ObjectId))
                {
                    return (TType)(object)new ObjectId(resultAsString);
                }
                else if (typeof(TType) == typeof(string))
                {
                    return (TType)(object)resultAsString;
                }
            }
            else if (ObjectIdConverter.TryConvert(value, out ObjectId result))
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
        }
        return default;
    }
}
