namespace xSdk.Data;

public interface IDatabase : IDisposable
{
    void Close();

    TConnection Open<TConnection>(bool persistConnection = false)
        where TConnection : class;
}
