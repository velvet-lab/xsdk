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

namespace xSdk.Data;

public class FlatFileDatabaseSetupTests
{
    private readonly FlatFileDatabaseOptionsValidator _validator;

    public FlatFileDatabaseSetupTests()
    {
        _validator = new FlatFileDatabaseOptionsValidator();
    }

    [Fact]
    public void FlatFileDatabaseSetup_DefaultConstructor_SetsUseLowerCamelCaseToTrue()
    {
        var setup = new FlatFileDatabaseOptions();

        Assert.True(setup.UseLowerCamelCase);
    }

    [Fact]
    public void FlatFileDatabaseSetup_DefaultConstructor_SetsReloadBeforeGetCollectionToFalse()
    {
        var setup = new FlatFileDatabaseOptions();

        Assert.False(setup.ReloadBeforeGetCollection);
    }

    [Fact]
    public void FlatFileDatabaseSetup_FilePath_CanBeSet()
    {
        var setup = new FlatFileDatabaseOptions
        {
            FilePath = "/tmp/test.json"
        };

        Assert.Equal("/tmp/test.json", setup.FilePath);
    }

    [Fact]
    public void FlatFileDatabaseSetup_KeyProperty_CanBeSet()
    {
        var setup = new FlatFileDatabaseOptions
        {
            KeyProperty = "id"
        };

        Assert.Equal("id", setup.KeyProperty);
    }

    [Fact]
    public void FlatFileDatabaseSetup_EncryptionKey_CanBeSet()
    {
        var setup = new FlatFileDatabaseOptions
        {
            EncryptionKey = "secret-key"
        };

        Assert.Equal("secret-key", setup.EncryptionKey);
    }

    [Fact]
    public void FlatFileDatabaseSetup_UseLowerCamelCase_CanBeOverridden()
    {
        var setup = new FlatFileDatabaseOptions
        {
            UseLowerCamelCase = false
        };

        Assert.False(setup.UseLowerCamelCase);
    }

    [Fact]
    public void FlatFileDatabaseSetup_ReloadBeforeGetCollection_CanBeOverridden()
    {
        var setup = new FlatFileDatabaseOptions
        {
            ReloadBeforeGetCollection = true
        };

        Assert.True(setup.ReloadBeforeGetCollection);
    }

    [Fact]
    public void Validate_WithEmptyFilePath_AddsValidationError()
    {
        var setup = new FlatFileDatabaseOptions { FilePath = string.Empty };

        var results = _validator.Validate(setup);

        Assert.NotEmpty(results.Errors);
    }

    [Fact]
    public void Validate_WithValidFilePath_NoValidationErrors()
    {
        var setup = new FlatFileDatabaseOptions { FilePath = "mydata.json" };

        var results = _validator.Validate(setup);

        Assert.Empty(results.Errors);
    }

    [Fact]
    public void Validate_FilePathWithoutExtension_AppendsJsonExtension()
    {
        var setup = new FlatFileDatabaseOptions { FilePath = "mydata" };

        var results = _validator.Validate(setup);

        Assert.Throws<ValidationException>(() => _validator.ValidateAndThrow(setup));
    }

    [Fact]
    public void Validate_FilePathAlreadyWithJsonExtension_DoesNotDoubleAppend()
    {
        var setup = new FlatFileDatabaseOptions { FilePath = "mydata.json" };

        var results = _validator.Validate(setup);

        Assert.Equal("mydata.json", setup.FilePath);
    }

    [Fact]
    public void Validate_WithEmptyFilePath_ThrowsWhenRequired()
    {
        var setup = new FlatFileDatabaseOptions { FilePath = string.Empty };

        Assert.Throws<ValidationException>(() => _validator.ValidateAndThrow(setup));
    }
}
