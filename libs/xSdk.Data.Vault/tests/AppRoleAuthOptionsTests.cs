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

namespace xSdk.Data;

public class AppRoleAuthOptionsTests
{
    [Fact]
    public void AppRoleAuthOptions_DefaultRoleId_IsNull()
    {
        var options = new AppRoleAuthOptions();

        Assert.Null(options.RoleId);
    }

    [Fact]
    public void AppRoleAuthOptions_DefaultSecret_IsNull()
    {
        var options = new AppRoleAuthOptions();

        Assert.Null(options.Secret);
    }

    [Fact]
    public void AppRoleAuthOptions_SetRoleId_StoresValue()
    {
        var options = new AppRoleAuthOptions();
        options.RoleId = "my-role-id";

        Assert.Equal("my-role-id", options.RoleId);
    }

    [Fact]
    public void AppRoleAuthOptions_SetSecret_StoresValue()
    {
        var options = new AppRoleAuthOptions();
        options.Secret = "my-secret";

        Assert.Equal("my-secret", options.Secret);
    }
}

public class AppRoleAuthOptionsValidatorTests
{
    private readonly AppRoleAuthOptionsValidator _validator = new();

    [Fact]
    public void Validate_WhenBothValuesPresent_IsValid()
    {
        var options = new AppRoleAuthOptions
        {
            RoleId = "my-role",
            Secret = "my-secret"
        };

        var result = _validator.Validate(options);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WhenRoleIdEmpty_IsInvalid()
    {
        var options = new AppRoleAuthOptions
        {
            RoleId = string.Empty,
            Secret = "my-secret"
        };

        var result = _validator.Validate(options);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("RoleId"));
    }

    [Fact]
    public void Validate_WhenSecretEmpty_IsInvalid()
    {
        var options = new AppRoleAuthOptions
        {
            RoleId = "my-role",
            Secret = string.Empty
        };

        var result = _validator.Validate(options);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("Secret"));
    }

    [Fact]
    public void Validate_WhenBothNull_HasErrors()
    {
        var options = new AppRoleAuthOptions();

        var result = _validator.Validate(options);

        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.PropertyName == "RoleId");
        Assert.Contains(result.Errors, e => e.PropertyName == "Secret");
    }
}
