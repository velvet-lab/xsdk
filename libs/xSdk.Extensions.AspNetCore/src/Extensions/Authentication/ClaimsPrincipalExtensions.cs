/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
