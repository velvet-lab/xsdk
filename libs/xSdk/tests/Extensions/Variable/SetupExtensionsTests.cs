using xSdk.Hosting;

namespace xSdk.Extensions.Variable;

public class SetupExtensionsTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void ValidateAnnotations_ValidSetup_ReturnsTrue()
    {
        var service = fixture
            .ConfigureServices(services => services.AddVariableServices())
            .GetService<IVariableService>();
        var setup = service.GetSetup<EnvironmentSetup>();

        var isValid = setup.ValidateAnnotations(out var results);

        Assert.True(isValid);
        Assert.Empty(results);
    }

    [Fact]
    public void ValidateMember_PassingValidator_DoesNotAddError()
    {
        var setup = new EnvironmentSetup();

        setup.ValidateMember(s => false, "Should not fail");

        Assert.Empty(setup.Results);
    }

    [Fact]
    public void ValidateMember_FailingValidator_AddsError()
    {
        var setup = new EnvironmentSetup();

        setup.ValidateMember(s => true, "Validation failed");

        Assert.Single(setup.Results);
        Assert.Equal("Validation failed", setup.Results.First().ErrorMessage);
    }

    [Fact]
    public void ValidateMember_SameErrorMessage_AddedOnlyOnce()
    {
        var setup = new EnvironmentSetup();

        setup.ValidateMember(s => true, "Duplicate error");
        setup.ValidateMember(s => true, "Duplicate error");

        Assert.Single(setup.Results);
    }

    [Fact]
    public void ValidateMember_DifferentErrorMessages_BothAdded()
    {
        var setup = new EnvironmentSetup();

        setup.ValidateMember(s => true, "Error A");
        setup.ValidateMember(s => true, "Error B");

        Assert.Equal(2, setup.Results.Count);
    }

    [Fact]
    public void ValidateMember_WithMemberNames_ErrorContainsMemberNames()
    {
        var setup = new EnvironmentSetup();

        setup.ValidateMember(s => true, "Error with member", "AppName");

        Assert.Single(setup.Results);
        Assert.Contains("AppName", setup.Results.First().MemberNames);
    }

    [Fact]
    public void ValidateMember_ThrowingValidator_TreatedAsError()
    {
        var setup = new EnvironmentSetup();

        setup.ValidateMember<EnvironmentSetup>(s => AlwaysThrows(s), "Thrown");

        Assert.Single(setup.Results);
    }

    private static bool AlwaysThrows(EnvironmentSetup s) => throw new InvalidOperationException();

    [Fact]
    public void ValidateResults_EmptyResults_ReturnsTrue()
    {
        var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

        var isValid = results.ValidateResults();

        Assert.True(isValid);
    }

    [Fact]
    public void ValidateResults_WithResults_ReturnsFalse()
    {
        var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>
        {
            new System.ComponentModel.DataAnnotations.ValidationResult("Error")
        };

        var isValid = results.ValidateResults();

        Assert.False(isValid);
    }

    [Fact]
    public void ValidateAnnotations_WithAllowedEmptyProperties_FiltersErrors()
    {
        var service = fixture
            .ConfigureServices(services => services.AddVariableServices())
            .GetService<IVariableService>();
        var setup = service.GetSetup<EnvironmentSetup>();

        setup.ValidateAnnotations(out var results, new[] { "AppName" });

        Assert.DoesNotContain(results, r => r.MemberNames != null && r.MemberNames.Contains("AppName"));
    }
}
