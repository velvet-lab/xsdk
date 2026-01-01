using AutoMapper;
using xSdk.Security.Claims;
using xSdk.Shared;
using System.Security.Claims;

namespace xSdk.Data.Converters.Mapper
{
    public static class ClaimListConverter
    {
        public sealed class ToModelProperty : IValueConverter<IEnumerable<Claim>, IEnumerable<ClaimModel>>
        {
            public IEnumerable<ClaimModel> Convert(IEnumerable<Claim> sourceMember, ResolutionContext context)
            {
                if (sourceMember.Any())
                {
                    return sourceMember.ToClaimModels();
                }
                return new List<ClaimModel>();
            }
        }

        public sealed class ToEntityProperty : IValueConverter<IEnumerable<ClaimModel>, IEnumerable<Claim>>
        {
            public IEnumerable<Claim> Convert(IEnumerable<ClaimModel> sourceMember, ResolutionContext context)
            {
                if (sourceMember.Any())
                {
                    return sourceMember.ToClaims();
                }
                return new List<Claim>();
            }
        }
    }
}
