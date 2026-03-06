using CommunityToolkit.Diagnostics;
using xSdk.Security.Claims;
using System.Security.Claims;

namespace xSdk.Extensions.Authentication
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsApiKeyPrincipal(this ClaimsPrincipal principal)
        {
            Guard.IsNotNull(principal);

            return principal.HasClaim(c => c.Type == SdkClaimTypes.ApiKey.Name &&
                                           c.Value == ApiKeySignature.Name) &&
                   principal.HasClaim(c => c.Type == SdkClaimTypes.ApiKey.Identifier &&
                                           c.Value == ApiKeySignature.Identifier);
        }
    }
}
