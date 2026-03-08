using System.Security.Claims;
using AspNetCore.Authentication.ApiKey;
using xSdk.Data;

namespace xSdk.Extensions.Authentication;

public sealed class ConcreteApiKey : IApiKey
{
    private readonly IReadOnlyCollection<Claim> _claims;

    public ConcreteApiKey()
    {
        _claims = new List<Claim>();
    }

    public string Key { get; set; }

    public string OwnerName { get; set; }

    public IReadOnlyCollection<Claim> Claims { get; set; }
}
