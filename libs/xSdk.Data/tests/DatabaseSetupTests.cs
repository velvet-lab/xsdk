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

using System.ComponentModel.DataAnnotations;

namespace xSdk.Data.Tests;

public class DatabaseSetupTests
{
    private class TestDatabaseSetup : DatabaseSetup
    {
        public bool WasValidated { get; private set; }

        protected override void ValidateSetup()
        {
            WasValidated = true;
        }
    }

    private class InvalidDatabaseSetup : DatabaseSetup
    {
        protected override void ValidateSetup()
        {
            Results.Add(new ValidationResult("Invalid setup"));
        }
    }

    [Fact]
    public void DefaultConstructor_InitializesProperties()
    {
        var setup = new TestDatabaseSetup();

        Assert.NotNull(setup.Properties);
        Assert.Empty(setup.Properties);
    }

    [Fact]
    public void DefaultConstructor_InitializesResults()
    {
        var setup = new TestDatabaseSetup();

        Assert.NotNull(setup.Results);
        Assert.Empty(setup.Results);
    }

    [Fact]
    public void Initialize_DoesNotThrow()
    {
        var setup = new TestDatabaseSetup();

        var ex = Record.Exception(() => setup.Initialize());

        Assert.Null(ex);
    }

    [Fact]
    public void Validate_CallsValidateSetup()
    {
        var setup = new TestDatabaseSetup();

        setup.Validate();

        Assert.True(setup.WasValidated);
    }

    [Fact]
    public void Validate_WithValidSetup_DoesNotThrow()
    {
        var setup = new TestDatabaseSetup();

        var ex = Record.Exception(() => setup.Validate());

        Assert.Null(ex);
    }

    [Fact]
    public void Validate_WithInvalidSetup_ThrowsSdkException()
    {
        var setup = new InvalidDatabaseSetup();

        Assert.Throws<SdkException>(() => setup.Validate());
    }

    [Fact]
    public void Validate_WithInvalidSetupAndNoThrow_DoesNotThrow()
    {
        var setup = new InvalidDatabaseSetup();

        var ex = Record.Exception(() => setup.Validate(throwIfFails: false));

        Assert.Null(ex);
    }

    [Fact]
    public void Properties_CanAddEntries()
    {
        var setup = new TestDatabaseSetup();

        setup.Properties["key"] = "value";

        Assert.Equal("value", setup.Properties["key"]);
    }
}
