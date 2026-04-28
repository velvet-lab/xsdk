namespace xSdk.Data;

public interface IDatalayerMetadata
{
    string? DatalayerName { get; }

    IServiceProvider? Services { get; }
}
