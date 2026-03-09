using System.Collections.ObjectModel;
using System.Security.Claims;
using AspNetCore.Authentication.ApiKey;
using CommunityToolkit.Diagnostics;
using xSdk.Security.Claims;

namespace xSdk.Extensions.Authentication;

public static class ApiKeyModelExtensions
{
    public static IApiKey ToApiKey(this IApiKeyModel? model)
    {
        Guard.IsNotNull(model);

        var claims = new List<Claim>();
        foreach (var claim in model.Claims)
        {
            claims.Add(ClaimCreator.CreateClaim(claim.Type, claim.Value, claim.ValueType, claim.Issuer, claim.OriginalIssuer));
        }

        claims.Add(
            ClaimCreator.CreateClaim(
                SdkClaimTypes.ApiKey.Name,
                ApiKeySignature.Name,
                ClaimValueTypes.String,
                Security.Claims.Authority.Name,
                Security.Claims.Authority.Name
            )
        );
        claims.Add(
            ClaimCreator.CreateClaim(
                SdkClaimTypes.ApiKey.Identifier,
                ApiKeySignature.Identifier,
                ClaimValueTypes.String,
                Security.Claims.Authority.Name,
                Authority.Name
            )
        );

        var result = new ConcreteApiKey
        {
            Key = model.Key,
            OwnerName = model.User,
            Claims = new ReadOnlyCollection<Claim>(claims)
        };

        return result;
    }
}
