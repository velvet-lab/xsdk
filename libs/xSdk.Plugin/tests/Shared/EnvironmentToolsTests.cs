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

        result.Should().Be(TestValue);

        CleanupTestVariable();
    }

    [Fact]
    public void ReadEnvironmentVariable_WithNonExistingVariable_ReturnsNull()
    {
        var result = EnvironmentTools.ReadEnvironmentVariable("NON_EXISTING_VAR_XYZ_123");

        result.Should().BeNull();
    }

    [Fact]
    public void ReadEnvironmentVariable_WithDefaultValue_ReturnsDefaultWhenNotFound()
    {
        var defaultValue = "DefaultValue";

        var result = EnvironmentTools.ReadEnvironmentVariable("NON_EXISTING_VAR", defaultValue);

        result.Should().Be(defaultValue);
    }

    [Fact]
    public void ReadEnvironmentVariable_WithDefaultValue_ReturnsActualValueWhenExists()
    {
        Environment.SetEnvironmentVariable(TestVarName, TestValue, EnvironmentVariableTarget.Process);
        var defaultValue = "DefaultValue";

        var result = EnvironmentTools.ReadEnvironmentVariable(TestVarName, defaultValue);

        result.Should().Be(TestValue);

        CleanupTestVariable();
    }

    [Fact]
    public void ReadEnvironmentVariable_WithEmptyString_ReturnsDefaultValue()
    {
        Environment.SetEnvironmentVariable(TestVarName, string.Empty, EnvironmentVariableTarget.Process);
        var defaultValue = "DefaultValue";

        var result = EnvironmentTools.ReadEnvironmentVariable(TestVarName, defaultValue);

        result.Should().Be(defaultValue);

        CleanupTestVariable();
    }

    [Fact]
    public void TryReadEnvironmentVariable_WithExistingVariable_ReturnsTrueAndValue()
    {
        Environment.SetEnvironmentVariable(TestVarName, TestValue, EnvironmentVariableTarget.Process);

        var success = EnvironmentTools.TryReadEnvironmentVariable(TestVarName, out var value);

        success.Should().BeTrue();
        value.Should().Be(TestValue);

        CleanupTestVariable();
    }

    [Fact]
    public void TryReadEnvironmentVariable_WithNonExistingVariable_ReturnsFalseAndEmptyString()
    {
        var success = EnvironmentTools.TryReadEnvironmentVariable("NON_EXISTING_VAR", out var value);

        success.Should().BeFalse();
        value.Should().BeEmpty();
    }

    [Fact]
    public void TryReadEnvironmentVariable_WithDefaultValue_ReturnsTrueAndDefaultWhenNotFound()
    {
        var defaultValue = "DefaultValue";

        var success = EnvironmentTools.TryReadEnvironmentVariable("NON_EXISTING_VAR", out var value, defaultValue);

        success.Should().BeTrue();
        value.Should().Be(defaultValue);
    }

    [Fact]
    public void TryReadEnvironmentVariable_WithDefaultValue_ReturnsActualValueWhenExists()
    {
        Environment.SetEnvironmentVariable(TestVarName, TestValue, EnvironmentVariableTarget.Process);
        var defaultValue = "DefaultValue";

        var success = EnvironmentTools.TryReadEnvironmentVariable(TestVarName, out var value, defaultValue);

        success.Should().BeTrue();
        value.Should().Be(TestValue);

        CleanupTestVariable();
    }

    [Fact]
    public void TryReadEnvironmentVariable_WithEmptyVariable_UsesDefaultValue()
    {
        Environment.SetEnvironmentVariable(TestVarName, string.Empty, EnvironmentVariableTarget.Process);
        var defaultValue = "DefaultValue";

        var success = EnvironmentTools.TryReadEnvironmentVariable(TestVarName, out var value, defaultValue);

        success.Should().BeTrue();
        value.Should().Be(defaultValue);

        CleanupTestVariable();
    }

    [Fact]
    public void ReadEnvironmentVariable_ChecksPriorityOrder_ProcessFirst()
    {
        var processValue = "ProcessValue";
        Environment.SetEnvironmentVariable(TestVarName, processValue, EnvironmentVariableTarget.Process);

        var result = EnvironmentTools.ReadEnvironmentVariable(TestVarName);

        result.Should().Be(processValue);

        CleanupTestVariable();
    }

    [Fact]
    public void TryReadEnvironmentVariable_WithNullDefault_ReturnsFalseWhenNotFound()
    {
        var success = EnvironmentTools.TryReadEnvironmentVariable("NON_EXISTING_VAR", out var value, null);

        success.Should().BeFalse();
        value.Should().BeEmpty();
    }

    private void CleanupTestVariable()
    {
        Environment.SetEnvironmentVariable(TestVarName, null, EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable(TestVarName, null, EnvironmentVariableTarget.User);
    }
}
