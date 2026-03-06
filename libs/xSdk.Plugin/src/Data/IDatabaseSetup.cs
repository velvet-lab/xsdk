using xSdk.Extensions.Variable;

namespace xSdk.Data
{
    public interface IDatabaseSetup : ISetup
    {
        IDictionary<string, string> Properties { get; }
    }
}
