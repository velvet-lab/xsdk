namespace xSdk.Data
{
    public interface IModel
    {
        object Id { get; set; }

        IPrimaryKey PrimaryKey { get; }
    }

    public interface IModel<TPrimaryKey, TPrimaryKeyType> : IModel
        where TPrimaryKey : IPrimaryKey<TPrimaryKeyType>, new()
    {
        new TPrimaryKeyType Id { get; set; }
    }
}
