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

using xSdk.Security.Claims;

namespace xSdk.Security.Claims;

public class AuthorityTests
{
    [Fact]
    public void Name_HasExpectedValue()
    {
        Assert.Equal("xSDK Authority", Authority.Name);
    }

    [Fact]
    public void Name_IsNotEmpty()
    {
        Assert.NotEmpty(Authority.Name);
    }
}

public class SdkClaimTypesTests
{
    [Fact]
    public void ApiKeyName_ReturnsNonEmptyString()
    {
        var result = SdkClaimTypes.ApiKey.Name;

        Assert.NotEmpty(result);
    }

    [Fact]
    public void ApiKeyIdentifier_ReturnsNonEmptyString()
    {
        var result = SdkClaimTypes.ApiKey.Identifier;

        Assert.NotEmpty(result);
    }

    [Fact]
    public void ApiKeyName_ContainsApikey()
    {
        var result = SdkClaimTypes.ApiKey.Name;

        Assert.Contains("apikey", result);
    }

    [Fact]
    public void ApiKeyIdentifier_ContainsApikey()
    {
        var result = SdkClaimTypes.ApiKey.Identifier;

        Assert.Contains("apikey", result);
    }

    [Fact]
    public void ApiKeyName_ContainsName()
    {
        var result = SdkClaimTypes.ApiKey.Name;

        Assert.Contains("name", result);
    }

    [Fact]
    public void ApiKeyIdentifier_ContainsIdentifier()
    {
        var result = SdkClaimTypes.ApiKey.Identifier;

        Assert.Contains("identifier", result);
    }

    [Fact]
    public void ApiKeyName_StartsWithHttps()
    {
        var result = SdkClaimTypes.ApiKey.Name;

        Assert.StartsWith("https://", result);
    }
}
