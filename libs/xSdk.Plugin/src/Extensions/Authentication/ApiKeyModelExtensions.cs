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
