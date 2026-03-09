namespace xSdk.Data;

public abstract class NoSqlModel : Model, IModel<NoSqlModelPK, string>
{
    public NoSqlModel()
    {
        this.PrimaryKey = new NoSqlModelPK();
    }

    public new string Id
    {
        get => PrimaryKey.GetValue<string>();
        set => PrimaryKey.SetValue(value);
    }
}
