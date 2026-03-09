namespace xSdk.Data;

public interface IConnectionBuilder
{
    object Create(IDatabaseSetup setup);
}
