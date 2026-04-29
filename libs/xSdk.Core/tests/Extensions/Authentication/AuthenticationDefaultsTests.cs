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

using AspNetCore.Authentication.ApiKey;

namespace xSdk.Extensions.Authentication;

public class AuthenticationDefaultsTests
{
    [Fact]
    public void ApiKeyAuth_Name_HasExpectedValue()
    {
        Assert.Equal("API Key Authentication", AuthenticationDefaults.ApiKeyAuth.Name);
    }

    [Fact]
    public void ApiKeyAuth_InHeader_Header_HasExpectedValue()
    {
        Assert.Equal("X-API-KEY", AuthenticationDefaults.ApiKeyAuth.InHeader.Header);
    }

    [Fact]
    public void ApiKeyAuth_InHeader_Scheme_ContainsApiKeyScheme()
    {
        Assert.Contains(ApiKeyDefaults.AuthenticationScheme, AuthenticationDefaults.ApiKeyAuth.InHeader.Scheme);
    }

    [Fact]
    public void ApiKeyAuth_InAuthorizationHeader_Header_HasExpectedValue()
    {
        Assert.Equal("ApiKeyName", AuthenticationDefaults.ApiKeyAuth.InAuthorizationHeader.Header);
    }

    [Fact]
    public void ApiKeyAuth_InAuthorizationHeader_Scheme_ContainsApiKeyScheme()
    {
        Assert.Contains(ApiKeyDefaults.AuthenticationScheme, AuthenticationDefaults.ApiKeyAuth.InAuthorizationHeader.Scheme);
    }

    [Fact]
    public void MultiAuth_Scheme_HasExpectedValue()
    {
        Assert.Equal("MultiAuthScheme", AuthenticationDefaults.MulitAuth.Scheme);
    }
}
