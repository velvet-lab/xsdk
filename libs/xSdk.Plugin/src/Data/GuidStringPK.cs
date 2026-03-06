using xSdk.Data.Converters.Mapper;
using xSdk.Shared;

namespace xSdk.Data
{
    public sealed class GuidStringPK : PrimaryKey<string>
    {
        private static object syncObject = new();

        public GuidStringPK()
            : base(Guid.NewGuid().ToString()) { }

        public GuidStringPK(Guid initialValue)
            : base(initialValue.ToString()) { }

        public GuidStringPK(string initialValue)
            : base(Guid.Parse(initialValue).ToString()) { }

        protected override TType Convert<TType>(object value)
        {
            lock (syncObject)
            {
                if (GuidConverter.TryConvert(value, out string stringResult))
                {
                    return TypeConverter.ConvertTo<TType>(stringResult);
                }
                else if (GuidConverter.TryConvert(value, out Guid guidResult))
                {
                    return TypeConverter.ConvertTo<TType>(guidResult);
                }
            }

            return default;
        }
    }
}
