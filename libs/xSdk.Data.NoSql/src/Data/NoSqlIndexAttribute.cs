namespace xSdk.Data;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class NoSqlIndexAttribute : Attribute
{
    private readonly bool _unique;
    private readonly string _expression;

    internal NoSqlIndex Index
    {
        get { return new NoSqlIndex { Expression = _expression, IsUnique = _unique }; }
    }

    public NoSqlIndexAttribute() { }

    public NoSqlIndexAttribute(bool unique)
    {
        this._unique = unique;
    }

    public NoSqlIndexAttribute(string expression, bool unique)
    {
        this._expression = expression;
        this._unique = unique;
    }
}
