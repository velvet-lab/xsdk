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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace xSdk.Security.Claims;

public static class ClaimModelExtensions
{
    public static Claim ToClaim(this ClaimModel model)
        => new Claim(model.Type, model.Value, model.ValueType, model.Issuer, model.OriginalIssuer);

    public static ClaimModel ToClaimModel(this Claim claim)
        => new ClaimModel
        {
            ValueType = claim.ValueType,
            Type = claim.Type,
            Value = claim.Value,
            Issuer = claim.Issuer,
            OriginalIssuer = claim.OriginalIssuer
        };

    public static IEnumerable<Claim> ToClaims(this IEnumerable<ClaimModel> models)
        => models.Select(x => x.ToClaim()).ToList();

    public static IEnumerable<ClaimModel> ToClaimModels(this IEnumerable<Claim> claims)
        => claims.Select(x => x.ToClaimModel()).ToList();
}
