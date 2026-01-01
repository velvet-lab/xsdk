namespace xSdk.Data
{
    public interface IEntity
    {
        object Id { get; set; }

        IPrimaryKey PrimaryKey { get; }
    }

    public interface IEntity<TPrimaryKey, TPrimaryKeyType> : IEntity
        where TPrimaryKey : IPrimaryKey<TPrimaryKeyType>, new()
    {
        new TPrimaryKeyType Id { get; set; }
    }
}
