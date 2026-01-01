using xSdk.Data;
using xSdk.Security.Claims;

namespace xSdk.Extensions.Authentication
{
    public sealed class ApiKeyModelExtensionsTests
    {
        class ApiKeyForTests : Model, IApiKeyModel
        {
            public string Key { get; set; }

            public IEnumerable<ClaimModel> Claims { get; set; }
            public string Description { get; set; }
            public string User { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime ValidUntil { get; set; }
        }

        [Fact]
        public void ToApiKey()
        {
            var model = new ApiKeyForTests
            {
                Key = "test-key",
                User = "test-owner",
                Claims = new List<ClaimModel>
                {
                    new ClaimModel{ Type = "type1", Value = "value1" },
                    new ClaimModel{ Type = "type2", Value = "value2" },
                    new ClaimModel{ Type = "https://datev.de/centraldb/claims/workflow", Value = "Read", ValueType = "http://www.w3.org/2001/XMLSchema#string", Issuer = "A Issuer", OriginalIssuer = "A Original Issuer" }
                }
            };

            var apiKey = model.ToApiKey();

            Assert.NotNull(apiKey);
            Assert.NotNull(apiKey.Claims);
            Assert.Equal("test-key", apiKey.Key);
            Assert.Equal("test-owner", apiKey.OwnerName);
            Assert.Equal(5, apiKey.Claims.Count);
        }
    }
}
