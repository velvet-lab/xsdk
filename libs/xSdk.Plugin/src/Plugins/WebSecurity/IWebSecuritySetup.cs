using xSdk.Extensions.Variable;

namespace xSdk.Plugins.WebSecurity
{
    public interface IWebSecuritySetup : ISetup
    {
        string Origins { get; set; }
    }
}
