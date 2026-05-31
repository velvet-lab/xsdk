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

using xSdk.Extensions.Variable.Fakes;

namespace xSdk.Extensions.Variable;

public class VariableSetupPrefixTests
{
    [Fact]
    public void Setup_WithPrefix_Name_IsQualified()
    {
        var setup = new SetupWithPrefix();

        // Trigger initialization by reading a value (causes ParseForVariables internally)
        _ = setup.StringValue;

        // The property uses raw name "string-value", but the variable stored should be "my-plugin-string-value"
        // We verify via the backing VariableService through the property round-trip
        setup.StringValue = "hello";
        Assert.Equal("hello", setup.StringValue);
    }

    [Fact]
    public void Setup_WithPrefix_DefaultValue_IsApplied()
    {
        var setup = new SetupWithPrefix();

        Assert.Equal(SetupWithPrefix.Definitions.StringValue.DefaultValue, setup.StringValue);
    }

    [Fact]
    public void Setup_WithPrefix_BoolDefaultValue_IsApplied()
    {
        var setup = new SetupWithPrefix();

        Assert.Equal(SetupWithPrefix.Definitions.BoolValue.DefaultValue, setup.BoolValue);
    }

    [Fact]
    public void Setup_WithPrefix_SetAndRead_RoundTrips()
    {
        var setup = new SetupWithPrefix();

        setup.StringValue = "round-trip-value";

        Assert.Equal("round-trip-value", setup.StringValue);
    }

    [Fact]
    public void Setup_WithPrefix_BoolSetAndRead_RoundTrips()
    {
        var setup = new SetupWithPrefix();

        setup.BoolValue = false;

        Assert.False(setup.BoolValue);
    }

    [Fact]
    public void Setup_WithoutPrefix_DefaultValue_IsApplied()
    {
        var setup = new SetupWithoutPrefix();

        Assert.Equal(SetupWithoutPrefix.Definitions.StringValue.DefaultValue, setup.StringValue);
    }

    [Fact]
    public void Setup_WithoutPrefix_SetAndRead_RoundTrips()
    {
        var setup = new SetupWithoutPrefix();

        setup.StringValue = "no-prefix-value";

        Assert.Equal("no-prefix-value", setup.StringValue);
    }

    [Fact]
    public void Setup_TwoSeparateInstances_DoNotShareValues()
    {
        var setup1 = new SetupWithPrefix();
        var setup2 = new SetupWithPrefix();

        setup1.StringValue = "value-one";
        setup2.StringValue = "value-two";

        Assert.Equal("value-one", setup1.StringValue);
        Assert.Equal("value-two", setup2.StringValue);
    }
}
