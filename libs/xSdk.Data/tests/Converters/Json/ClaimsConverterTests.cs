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
using System.Text.Json;
using xSdk.Data.Converters.Json;

namespace xSdk.Data.Tests.Converters.Json;

public class ClaimsConverterTests
{
    private readonly JsonSerializerOptions _options;

    public ClaimsConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new ClaimsConverter());
    }

    [Fact]
    public void Write_SerializesClaimsAsJsonArray()
    {
        var claims = new List<Claim>
        {
            new Claim("test-type", "test-value"),
        };

        var json = JsonSerializer.Serialize<IEnumerable<Claim>>(claims, _options);

        Assert.Contains("test-type", json);
        Assert.Contains("test-value", json);
    }

    [Fact]
    public void Read_DeserializesClaimsFromJsonArray()
    {
        var json = """
            [
              {
                "type": "email",
                "value": "user@example.com",
                "valueType": "http://www.w3.org/2001/XMLSchema#string",
                "issuer": "LOCAL AUTHORITY",
                "originalIssuer": "LOCAL AUTHORITY"
              }
            ]
            """;

        var result = JsonSerializer.Deserialize<IEnumerable<Claim>>(json, _options);

        Assert.NotNull(result);
        var claim = result.Single();
        Assert.Equal("email", claim.Type);
        Assert.Equal("user@example.com", claim.Value);
    }

    [Fact]
    public void Read_EmptyArray_ReturnsEmptyList()
    {
        var json = "[]";

        var result = JsonSerializer.Deserialize<IEnumerable<Claim>>(json, _options);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Read_ClaimWithoutType_IsSkipped()
    {
        var json = """
            [
              {
                "value": "some-value"
              }
            ]
            """;

        var result = JsonSerializer.Deserialize<IEnumerable<Claim>>(json, _options);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Read_ClaimWithoutValue_IsSkipped()
    {
        var json = """
            [
              {
                "type": "test-type"
              }
            ]
            """;

        var result = JsonSerializer.Deserialize<IEnumerable<Claim>>(json, _options);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void RoundTrip_SerializeDeserialize_RetainsClaims()
    {
        var claims = new List<Claim>
        {
            new Claim("role", "admin", ClaimValueTypes.String, "test-issuer", "test-original"),
        };

        var json = JsonSerializer.Serialize<IEnumerable<Claim>>(claims, _options);
        var result = JsonSerializer.Deserialize<IEnumerable<Claim>>(json, _options)!.ToList();

        Assert.Single(result);
        Assert.Equal("role", result[0].Type);
        Assert.Equal("admin", result[0].Value);
    }

    [Fact]
    public void Write_MultipleClaims_SerializesAll()
    {
        var claims = new List<Claim>
        {
            new Claim("type1", "value1"),
            new Claim("type2", "value2"),
        };

        var json = JsonSerializer.Serialize<IEnumerable<Claim>>(claims, _options);

        Assert.Contains("type1", json);
        Assert.Contains("type2", json);
    }
}
