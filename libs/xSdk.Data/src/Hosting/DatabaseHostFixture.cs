using xSdk.Data;

namespace xSdk.Hosting;

public abstract class DatabaseHostFixture : TestHostFixture
{
    public DatabaseHostFixture() : base(true)
    { }

    public IDatalayerFactory Factory => GetService<IDatalayerFactory>();

    protected abstract override void Initialize();
}
