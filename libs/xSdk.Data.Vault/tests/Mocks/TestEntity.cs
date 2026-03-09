namespace xSdk.Data.Mocks;

internal class TestEntity : IEntity
{
    public string Key { get; set; }

    public string Value { get; set; }

    public object Id { get; set; }

    public IPrimaryKey PrimaryKey => null;
}
