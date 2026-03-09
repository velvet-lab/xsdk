using AspNetCore.Authentication.ApiKey;

namespace xSdk.Extensions.Authentication;

public interface IApiKeyHandler
{
    Task<IApiKey?> GetApiKeyAsync(string key);
}
