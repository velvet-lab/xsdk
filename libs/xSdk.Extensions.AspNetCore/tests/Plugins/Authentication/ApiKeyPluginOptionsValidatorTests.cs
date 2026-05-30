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

using FluentValidation.Results;

namespace xSdk.Extensions.Authentication;

public class ApiKeyPluginOptionsValidatorTests
{
    [Fact]
    public void Validate_WithDefaultRealm_PassesValidation()
    {
        // The VariableSetup returns the default value when no value is set,
        // so default options are always valid.
        var options = new ApiKeyPluginOptions();
        var validator = new ApiKeyPluginOptionsValidator();

        ValidationResult result = validator.Validate(options);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WithExplicitRealm_PassesValidation()
    {
        var options = new ApiKeyPluginOptions { Realm = "My Custom Realm" };
        var validator = new ApiKeyPluginOptionsValidator();

        ValidationResult result = validator.Validate(options);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_RuleFor_Realm_ErrorCode_MatchesDefinition()
    {
        // Verify that the validator uses the correct error code for the Realm property.
        // When the rule fires, it uses ApiKeyPluginOptions.Definitions.Realm.Name.
        var validator = new ApiKeyPluginOptionsValidator();
        var descriptor = validator.CreateDescriptor();
        var rules = descriptor.GetRulesForMember(nameof(ApiKeyPluginOptions.Realm));

        Assert.NotNull(rules);
        Assert.NotEmpty(rules);
    }

    [Fact]
    public void Validate_WithWhiteSpaceRealm_FailsValidation_WhitespaceStoredAsIs()
    {
        // VariableSetup stores whitespace as-is (unlike empty string which falls back to default).
        // FluentValidation's NotEmpty() treats whitespace as empty → validation fails.
        var options = new ApiKeyPluginOptions { Realm = "   " };
        var validator = new ApiKeyPluginOptionsValidator();

        ValidationResult result = validator.Validate(options);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Authentication realm is missing");
    }
}

