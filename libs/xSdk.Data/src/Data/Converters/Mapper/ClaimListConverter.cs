using System.Security.Claims;
using xSdk.Security.Claims;

namespace xSdk.Data.Converters.Mapper;

public static class ClaimListConverter
{
    public static IEnumerable<ClaimModel> Convert(IEnumerable<Claim> sourceMember)
    {
        if (sourceMember.Any())
        {
            return sourceMember.ToClaimModels();
        }
        return new List<ClaimModel>();
    }

    public static IEnumerable<Claim> Convert(IEnumerable<ClaimModel> sourceMember)
    {
        if (sourceMember.Any())
        {
            return sourceMember.ToClaims();
        }
        return new List<Claim>();
    }
}
