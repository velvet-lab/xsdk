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
using AspNetCore.Authentication.ApiKey;
using xSdk.Data;
using xSdk.Security.Claims;

namespace xSdk.Extensions.Authentication;

public class ApiKeyModelExtensionsTests
{
    private sealed class TestApiKeyModel : IApiKeyModel
    {
        public string? Id { get; set; }
        public string Key { get; set; } = "test-api-key-123";
        public string? User { get; set; } = "testuser";
        public IEnumerable<ClaimModel> Claims { get; set; } = [];
        public string? Description { get; set; } = "Test key";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ValidUntil { get; set; } = DateTime.UtcNow.AddYears(1);
    }

    [Fact]
    public void ToApiKey()
    {
        var model = new TestApiKeyModel
        {
            Key = "test-key",
            User = "test-owner",
            Claims =
            [
                new ClaimModel{ Type = "type1", Value = "value1" },
                new ClaimModel{ Type = "type2", Value = "value2" },
                new ClaimModel
                {
                    Type = "https://velvet-lab.net/centraldb/claims/workflow",
                    Value = "Read",
                    ValueType = "http://www.w3.org/2001/XMLSchema#string",
                    Issuer = "A Issuer",
                    OriginalIssuer = "A Original Issuer"
                }
            ]
        };

        IApiKey apiKey = model.ToApiKey();

        Assert.NotNull(apiKey);
        Assert.NotNull(apiKey.Claims);
        Assert.Equal("test-key", apiKey.Key);
        Assert.Equal("test-owner", apiKey.OwnerName);
        Assert.Equal(5, apiKey.Claims.Count);
    }

    [Fact]
    public void ToApiKey_WithValidModel_ReturnsIApiKey()
    {
        var model = new TestApiKeyModel();

        IApiKey result = model.ToApiKey();

        Assert.NotNull(result);
    }

    [Fact]
    public void ToApiKey_SetsKeyFromModel()
    {
        var model = new TestApiKeyModel { Key = "my-secret-key" };

        IApiKey result = model.ToApiKey();

        Assert.Equal("my-secret-key", result.Key);
    }

    [Fact]
    public void ToApiKey_SetsOwnerNameFromModelUser()
    {
        var model = new TestApiKeyModel { User = "john.doe" };

        IApiKey result = model.ToApiKey();

        Assert.Equal("john.doe", result.OwnerName);
    }

    [Fact]
    public void ToApiKey_AlwaysAddsSdkApiKeyClaims()
    {
        var model = new TestApiKeyModel();

        IApiKey result = model.ToApiKey();

        // Should have at least the 2 SDK claims (ApiKey.Name and ApiKey.Identifier)
        Assert.NotEmpty(result.Claims);
        Assert.True(result.Claims.Count >= 2);
    }

    [Fact]
    public void ToApiKey_WithUserClaims_IncludesThemInResult()
    {
        var userClaim = new ClaimModel
        {
            Type = ClaimTypes.Role,
            Value = "admin",
            ValueType = ClaimValueTypes.String,
            Issuer = "test",
            OriginalIssuer = "test"
        };
        var model = new TestApiKeyModel { Claims = [userClaim] };

        IApiKey result = model.ToApiKey();

        Assert.Contains(result.Claims, c => c.Type == ClaimTypes.Role && c.Value == "admin");
    }

    [Fact]
    public void ToApiKey_WithNullModel_ThrowsException()
    {
        IApiKeyModel? model = null;

        Assert.ThrowsAny<Exception>(model.ToApiKey);
    }
}
