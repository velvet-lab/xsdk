using System.Security.Claims;
using CommunityToolkit.Diagnostics;
using xSdk.Security.Claims;

namespace xSdk.Extensions.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static bool IsApiKeyPrincipal(this ClaimsPrincipal principal)
    {
        Guard.IsNotNull(principal);

        return principal.HasClaim(
                   claim => claim.Type == SdkClaimTypes.ApiKey.Name
                       && claim.Value == ApiKeySignature.Name
               )
               && principal.HasClaim(
                   claim => claim.Type == SdkClaimTypes.ApiKey.Identifier
                       && claim.Value == ApiKeySignature.Identifier
               );
    }
}
