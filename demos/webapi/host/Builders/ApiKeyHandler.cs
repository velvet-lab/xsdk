using AspNetCore.Authentication.ApiKey;
using xSdk.Extensions.Authentication;
using xSdk.Security.Claims;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Security.Claims;

namespace xSdk.Demos.Builders
{
    internal class ApiKeyHandler : IApiKeyHandler
    {
        public ApiKeyHandler(ILogger<ApiKeyHandler> logger)
        {
            this.logger = logger;
        }


        public Task<IApiKey?> GetApiKeyAsync(string key)
        {
            return Task.FromResult(ApiKeys.SingleOrDefault(x => string.Compare(x.Key, key) == 0));
        }

        // List of ApiKeys only for Demo Purposes. Its highly recommended to host this in a external service
        private List<IApiKey> ApiKeys = new List<IApiKey>
        {
            new ConcreteApiKey
            {
                Key = "338879db-73e4-4f50-93fa-45f70d35ac15",
                OwnerName = "Only Read user",
                Claims = new ReadOnlyCollection<Claim>(new[] { ClaimCreator.CreateClaim(MyClaimTypes.MyTableA.Permission, MyClaimValues.Permissions.Read) }),
            },
            new ConcreteApiKey
            {
                Key = "47cf5874-4c73-45ae-82dd-f7f2e66b79c6",
                OwnerName = "Write User",
                Claims = new ReadOnlyCollection<Claim>(
                    new[]
                    {
                        ClaimCreator.CreateClaim(MyClaimTypes.MyTableA.Permission, MyClaimValues.Permissions.Read),
                        ClaimCreator.CreateClaim(MyClaimTypes.MyTableA.Permission, MyClaimValues.Permissions.Write),
                    }
                ),
            },
        };
        private ILogger<ApiKeyHandler> logger;
    }
}
