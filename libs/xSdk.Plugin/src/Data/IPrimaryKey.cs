namespace xSdk.Data
{
    public interface IPrimaryKey
    {
        object GetValue();

        TType GetValue<TType>();

        void SetValue(object value);
    }

    public interface IPrimaryKey<TPrimaryKeyType> : IPrimaryKey { }
}
