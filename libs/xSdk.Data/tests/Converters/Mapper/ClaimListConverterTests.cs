using xSdk.Security.Claims;
using System.Security.Claims;

namespace xSdk.Data.Converters.Mapper
{
    public class ClaimListConverterTests
    {
        private readonly IEnumerable<Claim> Claims = new List<Claim>
        {
            new Claim("Type1", "Value1"),
            new Claim("Type2", "Value2")
        };

        private readonly IEnumerable<ClaimModel> ClaimModels = new List<ClaimModel>
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
            var converter = new ClaimListConverter.ToModelProperty();
            var actual = converter.Convert(Claims, default);

            Assert.NotNull(actual);
            Assert.IsType<List<ClaimModel>>(actual);
        }

        [Fact]
        public void ConvertStringToClaims()
        {
            var converter = new ClaimListConverter.ToEntityProperty();
            var actual = converter.Convert(ClaimModels, default);

            Assert.NotNull(actual);
            Assert.IsType<List<Claim>>(actual);
        }
    }
}
