using xSdk.Data.Converters.Mapper;
using xSdk.Shared;

namespace xSdk.Data
{
    public sealed class GuidPK : PrimaryKey<Guid>
    {
        private object syncObject = new();

        public GuidPK()
            : base(Guid.NewGuid()) { }

        public GuidPK(Guid initialValue)
            : base(initialValue) { }

        public GuidPK(string initialValue)
            : base(Guid.Parse(initialValue)) { }

        protected override TType Convert<TType>(object value)
        {
            lock (syncObject)
            {
                if (GuidConverter.TryConvert(value, out Guid result))
                {
                    return TypeConverter.ConvertTo<TType>(result);
                }
                else if (GuidConverter.TryConvert(value, out string resultString))
                {
                    return TypeConverter.ConvertTo<TType>(resultString);
                }
            }

            return default;
        }
    }
}
