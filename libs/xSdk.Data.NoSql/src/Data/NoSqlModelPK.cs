using xSdk.Data.Converters.Mapper;
using LiteDB;

namespace xSdk.Data
{
    internal class NoSqlModelPK : PrimaryKey<string>
    {
        private object syncObject = new();

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
                if (PKObjectIdToString.TryConvert(value, out string resultAsString))
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
                else if (PKStringToObjectId.TryConvert(value, out ObjectId result))
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
}
