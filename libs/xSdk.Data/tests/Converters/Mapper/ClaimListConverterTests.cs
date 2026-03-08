using System.Security.Claims;
using xSdk.Security.Claims;

namespace xSdk.Data.Converters.Mapper;

public class ClaimListConverterTests
{
    private readonly IEnumerable<Claim> _claims = new List<Claim>
    {
        new Claim("Type1", "Value1"),
        new Claim("Type2", "Value2")
    };

    private readonly IEnumerable<ClaimModel> _claimModels = new List<ClaimModel>
    {
        new ClaimModel{
            Type = "Type1",
            Value = "Value1"
        },
        new ClaimModel{
            Type= "Type2",
            Value = "Value2"
        }
    };

    [Fact]
    public void ConvertClaimsToString()
    {
        var actual = ClaimListConverter.Convert(_claims);

        Assert.NotNull(actual);
        Assert.IsType<List<ClaimModel>>(actual);
    }

    [Fact]
    public void ConvertStringToClaims()
    {
        var actual = ClaimListConverter.Convert(_claimModels);

        Assert.NotNull(actual);
        Assert.IsType<List<Claim>>(actual);
    }
}
