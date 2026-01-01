using LiteDB;

namespace xSdk.Data.Converters.Bson
{
    public abstract class BsonConverter<TValue>
    {
        public abstract BsonValue Serialize(TValue value);

        public abstract TValue Deserialize(BsonValue value);

        public void Register()
        {
            BsonMapper.Global.RegisterType<TValue>(serialize: Serialize, deserialize: Deserialize);
        }
    }
}
