using xSdk.Data.Converters.Mapper;
using LiteDB;

namespace xSdk.Data
{
    internal class NoSqlEntityPK : PrimaryKey<ObjectId>
    {
        private object syncObject = new();

        public NoSqlEntityPK()
            : base(ObjectId.NewObjectId()) { }

        public NoSqlEntityPK(ObjectId initialValue)
            : base(initialValue) { }

        public NoSqlEntityPK(string intialValue)
            : base(new ObjectId(intialValue)) { }

        protected override TType Convert<TType>(object value)
        {
            lock (syncObject)
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
}
