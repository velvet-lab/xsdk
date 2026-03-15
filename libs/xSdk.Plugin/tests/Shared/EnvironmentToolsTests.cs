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

using xSdk.Shared;

namespace xSdk.Plugin.Tests.Shared;

public class EnvironmentToolsTests
{
    private const string TestVarName = "XSDK_TEST_VAR_UNIT_TEST";
    private const string TestValue = "TestValue123";

    public EnvironmentToolsTests()
    {
        // Cleanup before each test
        CleanupTestVariable();
    }

    [Fact]
    public void ReadEnvironmentVariable_WithExistingProcessVariable_ReturnsValue()
    {
        Environment.SetEnvironmentVariable(TestVarName, TestValue, EnvironmentVariableTarget.Process);

        var result = EnvironmentTools.ReadEnvironmentVariable(TestVarName);

        Assert.Equal(TestValue, result);

        CleanupTestVariable();
    }

    [Fact]
    public void ReadEnvironmentVariable_WithNonExistingVariable_ReturnsNull()
    {
        var result = EnvironmentTools.ReadEnvironmentVariable("NON_EXISTING_VAR_XYZ_123");

        Assert.Null(result);
    }

    [Fact]
    public void ReadEnvironmentVariable_WithDefaultValue_ReturnsDefaultWhenNotFound()
    {
        var defaultValue = "DefaultValue";

        var result = EnvironmentTools.ReadEnvironmentVariable("NON_EXISTING_VAR", defaultValue);

        Assert.Equal(defaultValue, result);
    }

    [Fact]
    public void ReadEnvironmentVariable_WithDefaultValue_ReturnsActualValueWhenExists()
    {
        Environment.SetEnvironmentVariable(TestVarName, TestValue, EnvironmentVariableTarget.Process);
        var defaultValue = "DefaultValue";

        var result = EnvironmentTools.ReadEnvironmentVariable(TestVarName, defaultValue);

        Assert.Equal(TestValue, result);

        CleanupTestVariable();
    }

    [Fact]
    public void ReadEnvironmentVariable_WithEmptyString_ReturnsDefaultValue()
    {
        Environment.SetEnvironmentVariable(TestVarName, string.Empty, EnvironmentVariableTarget.Process);
        var defaultValue = "DefaultValue";

        var result = EnvironmentTools.ReadEnvironmentVariable(TestVarName, defaultValue);

        Assert.Equal(defaultValue, result);

        CleanupTestVariable();
    }

    [Fact]
    public void TryReadEnvironmentVariable_WithExistingVariable_ReturnsTrueAndValue()
    {
        Environment.SetEnvironmentVariable(TestVarName, TestValue, EnvironmentVariableTarget.Process);

        var success = EnvironmentTools.TryReadEnvironmentVariable(TestVarName, out var value);

        Assert.True(success);
        Assert.Equal(TestValue, value);

        CleanupTestVariable();
    }

    [Fact]
    public void TryReadEnvironmentVariable_WithNonExistingVariable_ReturnsFalseAndEmptyString()
    {
        var success = EnvironmentTools.TryReadEnvironmentVariable("NON_EXISTING_VAR", out var value);

        Assert.False(success);
        Assert.Equal(string.Empty, value);
    }

    [Fact]
    public void TryReadEnvironmentVariable_WithDefaultValue_ReturnsTrueAndDefaultWhenNotFound()
    {
        var defaultValue = "DefaultValue";

        var success = EnvironmentTools.TryReadEnvironmentVariable("NON_EXISTING_VAR", out var value, defaultValue);

        Assert.True(success);
        Assert.Equal(defaultValue, value);
    }

    [Fact]
    public void TryReadEnvironmentVariable_WithDefaultValue_ReturnsActualValueWhenExists()
    {
        Environment.SetEnvironmentVariable(TestVarName, TestValue, EnvironmentVariableTarget.Process);
        var defaultValue = "DefaultValue";

        var success = EnvironmentTools.TryReadEnvironmentVariable(TestVarName, out var value, defaultValue);

        Assert.True(success);
        Assert.Equal(TestValue, value);

        CleanupTestVariable();
    }

    [Fact]
    public void TryReadEnvironmentVariable_WithEmptyVariable_UsesDefaultValue()
    {
        Environment.SetEnvironmentVariable(TestVarName, string.Empty, EnvironmentVariableTarget.Process);
        var defaultValue = "DefaultValue";

        var success = EnvironmentTools.TryReadEnvironmentVariable(TestVarName, out var value, defaultValue);

        Assert.True(success);
        Assert.Equal(defaultValue, value);

        CleanupTestVariable();
    }

    [Fact]
    public void ReadEnvironmentVariable_ChecksPriorityOrder_ProcessFirst()
    {
        var processValue = "ProcessValue";
        Environment.SetEnvironmentVariable(TestVarName, processValue, EnvironmentVariableTarget.Process);

        var result = EnvironmentTools.ReadEnvironmentVariable(TestVarName);

        Assert.Equal(processValue, result);

        CleanupTestVariable();
    }

    [Fact]
    public void TryReadEnvironmentVariable_WithNullDefault_ReturnsFalseWhenNotFound()
    {
        var success = EnvironmentTools.TryReadEnvironmentVariable("NON_EXISTING_VAR", out var value, null);

        Assert.False(success);
        Assert.Equal(string.Empty, value);
    }

    private void CleanupTestVariable()
    {
        Environment.SetEnvironmentVariable(TestVarName, null, EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable(TestVarName, null, EnvironmentVariableTarget.User);
    }
}
