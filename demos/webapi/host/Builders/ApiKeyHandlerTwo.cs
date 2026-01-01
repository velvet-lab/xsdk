using AspNetCore.Authentication.ApiKey;
using xSdk.Extensions.Authentication;
using xSdk.Security.Claims;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Security.Claims;

namespace xSdk.Demos.Builders
{
    internal class ApiKeyHandlerTwo: IApiKeyHandler
    {
        public ApiKeyHandlerTwo(ILogger<ApiKeyHandlerTwo> logger)
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
                Key = "1bee9671-13fa-489a-a35e-506fec8f74a1",
                OwnerName = "Only Read user",
                Claims = new ReadOnlyCollection<Claim>(new[] { ClaimCreator.CreateClaim(MyClaimTypes.MyTableA.Permission, MyClaimValues.Permissions.Read) }),
            },
            new ConcreteApiKey
            {
                Key = "0d3d3058-e59e-4178-ac23-0ed89168a925",
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
        private ILogger<ApiKeyHandlerTwo> logger;
    }
}
