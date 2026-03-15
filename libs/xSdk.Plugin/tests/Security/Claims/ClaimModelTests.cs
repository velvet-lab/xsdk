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
using xSdk.Security.Claims;

namespace xSdk.Plugin.Tests.Security.Claims;

public class ClaimModelTests
{
    [Fact]
    public void DefaultValues_AreEmptyStrings()
    {
        var model = new ClaimModel();

        Assert.Equal(string.Empty, model.Issuer);
        Assert.Equal(string.Empty, model.OriginalIssuer);
        Assert.Equal(string.Empty, model.Type);
        Assert.Equal(string.Empty, model.Value);
        Assert.Equal(string.Empty, model.ValueType);
    }

    [Fact]
    public void Properties_CanBeSetAndRead()
    {
        var model = new ClaimModel
        {
            Issuer = "issuer",
            OriginalIssuer = "original-issuer",
            Type = "claim-type",
            Value = "claim-value",
            ValueType = "value-type",
        };

        Assert.Equal("issuer", model.Issuer);
        Assert.Equal("original-issuer", model.OriginalIssuer);
        Assert.Equal("claim-type", model.Type);
        Assert.Equal("claim-value", model.Value);
        Assert.Equal("value-type", model.ValueType);
    }
}

public class ClaimModelExtensionsTests
{
    [Fact]
    public void ToClaim_ConvertsToClaim_WithCorrectProperties()
    {
        var model = new ClaimModel
        {
            Type = "test-type",
            Value = "test-value",
            ValueType = ClaimValueTypes.String,
            Issuer = "test-issuer",
            OriginalIssuer = "original-issuer",
        };

        var claim = model.ToClaim();

        Assert.Equal("test-type", claim.Type);
        Assert.Equal("test-value", claim.Value);
        Assert.Equal(ClaimValueTypes.String, claim.ValueType);
        Assert.Equal("test-issuer", claim.Issuer);
        Assert.Equal("original-issuer", claim.OriginalIssuer);
    }

    [Fact]
    public void ToClaimModel_ConvertsToClaimModel_WithCorrectProperties()
    {
        var claim = new Claim("test-type", "test-value", ClaimValueTypes.String, "test-issuer", "original-issuer");

        var model = claim.ToClaimModel();

        Assert.Equal("test-type", model.Type);
        Assert.Equal("test-value", model.Value);
        Assert.Equal(ClaimValueTypes.String, model.ValueType);
        Assert.Equal("test-issuer", model.Issuer);
        Assert.Equal("original-issuer", model.OriginalIssuer);
    }

    [Fact]
    public void ToClaims_ConvertsCollectionOfModels()
    {
        var models = new List<ClaimModel>
        {
            new() { Type = "type1", Value = "value1" },
            new() { Type = "type2", Value = "value2" },
        };

        var claims = models.ToClaims().ToList();

        Assert.Equal(2, claims.Count);
        Assert.Equal("type1", claims[0].Type);
        Assert.Equal("type2", claims[1].Type);
    }

    [Fact]
    public void ToClaimModels_ConvertsCollectionOfClaims()
    {
        var claims = new List<Claim>
        {
            new("type1", "value1"),
            new("type2", "value2"),
        };

        var models = claims.ToClaimModels().ToList();

        Assert.Equal(2, models.Count);
        Assert.Equal("type1", models[0].Type);
        Assert.Equal("type2", models[1].Type);
    }

    [Fact]
    public void ToClaim_ThenToClaimModel_RoundTrips()
    {
        var original = new ClaimModel
        {
            Type = "round-trip",
            Value = "test",
            ValueType = ClaimValueTypes.String,
            Issuer = "issuer",
            OriginalIssuer = "orig",
        };

        var result = original.ToClaim().ToClaimModel();

        Assert.Equal(original.Type, result.Type);
        Assert.Equal(original.Value, result.Value);
        Assert.Equal(original.Issuer, result.Issuer);
    }
}
