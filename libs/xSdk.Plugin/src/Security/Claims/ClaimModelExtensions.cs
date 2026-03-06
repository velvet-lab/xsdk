using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace xSdk.Security.Claims
{
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
}
