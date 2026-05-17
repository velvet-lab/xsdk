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

using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace xSdk.Extensions.Options;

public class ApplicationOptionsTests
{   

    [Fact]
    public void Constructor_SetsVersionFromAssembly()
    {
        var options = new ApplicationOptions();

        Assert.NotNull(options.Version);
        Assert.NotNull(options.AppVersion);
        Assert.False(string.IsNullOrEmpty(options.AppVersion));
    }

    [Fact]
    public void Name_CanBeSetAndRead()
    {
        var options = new ApplicationOptions
        {
            Name = "testapp"
        };

        Assert.Equal("testapp", options.Name);
    }

    [Fact]
    public void Company_CanBeSetAndRead()
    {
        var options = new ApplicationOptions
        {
            Company = "Acme Corp"
        };

        Assert.Equal("Acme Corp", options.Company);
    }

    [Fact]
    public void Prefix_CanBeSetAndRead()
    {
        var options = new ApplicationOptions
        {
            Prefix = "ACME"
        };

        Assert.Equal("ACME", options.Prefix);
    }

    [Fact]
    public void Description_CanBeSetAndRead()
    {
        var options = new ApplicationOptions
        {
            Description = "A test app"
        };

        Assert.Equal("A test app", options.Description);
    }

    [Fact]
    public void AppVersion_CanBeSetAndRead()
    {
        var options = new ApplicationOptions
        {
            AppVersion = "2.0.0"
        };

        Assert.Equal("2.0.0", options.AppVersion);
    }

    [Fact]
    public void Definitions_AppName_HasExpectedValues()
    {
        Assert.Equal("app-name", ApplicationOptions.Definitions.AppName.Name);
        Assert.Equal("xsdk", ApplicationOptions.Definitions.AppName.DefaultValue);
    }

    [Fact]
    public void Definitions_AppCompany_HasExpectedValues()
    {
        Assert.Equal("app-company", ApplicationOptions.Definitions.AppCompany.Name);
        Assert.Equal("xcom", ApplicationOptions.Definitions.AppCompany.DefaultValue);
    }

    [Fact]
    public void Definitions_AppPrefix_HasExpectedValues()
    {
        Assert.Equal("app-prefix", ApplicationOptions.Definitions.AppPrefix.Name);
        Assert.Equal("XSDK", ApplicationOptions.Definitions.AppPrefix.DefaultValue);
    }

    [Fact]
    public void Definitions_AppDescription_HasName()
    {
        Assert.Equal("app-description", ApplicationOptions.Definitions.AppDescription.Name);
    }

    [Fact]
    public void Definitions_AppVersion_HasName()
    {
        Assert.Equal("app-version", ApplicationOptions.Definitions.AppVersion.Name);
    }
}

public class ApplicationOptionsValidatorTests
{
    [Fact]
    public void Validate_WithValidOptions_PassesValidation()
    {
        var options = new ApplicationOptions { Name = "app", Company = "co", Prefix = "APP", AppVersion = "1.0.0" };
        var validator = new ApplicationOptionsValidator();

        ValidationResult result = validator.Validate(options);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WithEmptyName_FailsValidation()
    {
        var options = new ApplicationOptions { Name = "", Company = "co", Prefix = "APP", AppVersion = "1.0.0" };
        var validator = new ApplicationOptionsValidator();

        ValidationResult result = validator.Validate(options);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(ApplicationOptions.Name));
    }

    [Fact]
    public void Validate_WithEmptyCompany_FailsValidation()
    {
        var options = new ApplicationOptions { Name = "app", Company = "", Prefix = "APP", AppVersion = "1.0.0" };
        var validator = new ApplicationOptionsValidator();

        ValidationResult result = validator.Validate(options);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(ApplicationOptions.Company));
    }

    [Fact]
    public void Validate_WithEmptyPrefix_FailsValidation()
    {
        var options = new ApplicationOptions { Name = "app", Company = "co", Prefix = "", AppVersion = "1.0.0" };
        var validator = new ApplicationOptionsValidator();

        ValidationResult result = validator.Validate(options);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(ApplicationOptions.Prefix));
    }

    [Fact]
    public void Validate_WithEmptyAppVersion_FailsValidation()
    {
        var options = new ApplicationOptions { Name = "app", Company = "co", Prefix = "APP", AppVersion = "" };
        var validator = new ApplicationOptionsValidator();

        ValidationResult result = validator.Validate(options);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(ApplicationOptions.AppVersion));
    }
}

public class ApplicationOptionsExtensionsTests
{
    [Fact]
    public void RegisterApplicationOptions_RegistersOptions_WithDefaults()
    {
        var services = new ServiceCollection();
        var options = new ApplicationOptions { Name = null, Company = null, Prefix = null, AppVersion = "1.0.0" };

        services.RegisterApplicationOptions(options);
        ServiceProvider provider = services.BuildServiceProvider();
        ApplicationOptions registered = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;

        Assert.Equal(ApplicationOptions.Definitions.AppName.DefaultValue, registered.Name);
        Assert.Equal(ApplicationOptions.Definitions.AppCompany.DefaultValue, registered.Company);
        Assert.Equal(ApplicationOptions.Definitions.AppPrefix.DefaultValue, registered.Prefix);
    }

    [Fact]
    public void RegisterApplicationOptions_RegistersOptions_WithCustomValues()
    {
        var services = new ServiceCollection();
        var options = new ApplicationOptions { Name = "custom", Company = "myco", Prefix = "CUST", AppVersion = "2.0.0" };

        services.RegisterApplicationOptions(options);
        ServiceProvider provider = services.BuildServiceProvider();
        ApplicationOptions registered = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;

        Assert.Equal("custom", registered.Name);
        Assert.Equal("myco", registered.Company);
        Assert.Equal("CUST", registered.Prefix);
    }

    [Fact]
    public void RegisterApplicationOptions_WithInvalidOptions_ThrowsValidationException()
    {
        var services = new ServiceCollection();
        // Empty strings are not null, so defaults are NOT applied; they remain empty and fail validation
        var options = new ApplicationOptions { Name = "x", Company = "x", Prefix = "x", AppVersion = "" };
        services.RegisterApplicationOptions(options);
        ServiceProvider provider = services.BuildServiceProvider();

        // ValidationException is thrown when IOptions<T>.Value is resolved
        Assert.Throws<ValidationException>(() => provider.GetRequiredService<IOptions<ApplicationOptions>>().Value);
    }
}
