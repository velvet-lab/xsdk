namespace xSdk.Data.Mocks;

internal static class TestEntityExtensions
{
    internal static KeyValuePair<string, object> ConverToDictionary(this TestEntity entity)
    {
        return new KeyValuePair<string, object>(entity.Key, entity.Value);
    }
}
