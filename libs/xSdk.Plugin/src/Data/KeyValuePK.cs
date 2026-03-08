namespace xSdk.Data;

public sealed class KeyValuePK : PrimaryKey<string>
{
    private readonly object _syncObject = new();

    public KeyValuePK()
        : base(string.Empty) { }

    public KeyValuePK(string initialValue)
        : base(initialValue) { }

    protected override TType Convert<TType>(object value)
    {
        lock (_syncObject)
        {
            return (TType)value;
        }
    }
}
