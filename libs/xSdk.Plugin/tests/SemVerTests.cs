namespace xSdk.Plugin.Tests;

public class SemVerTests
{
    [Fact]
    public void Constructor_WithSimpleVersion_SetsVersionAndRange()
    {
        var semver = new SemVer("1.2.3");

        Assert.Equal("1.2.3", semver.Version);
        Assert.NotNull(semver.Range);
    }

    [Fact]
    public void Constructor_WithVersionAndRange_SetsBothProperties()
    {
        var semver = new SemVer("1.2.3", ">=1.0.0");

        Assert.Equal("1.2.3", semver.Version);
        Assert.Equal(">=1.0.0", semver.Range);
    }

    [Fact]
    public void IsSatisfied_WhenVersionSatisfiesRange_ReturnsTrue()
    {
        var semver = new SemVer("1.2.3", ">=1.0.0");

        Assert.True(semver.IsSatisfied);
    }

    [Fact]
    public void IsSatisfied_WhenVersionDoesNotSatisfyRange_ReturnsFalse()
    {
        var semver = new SemVer("0.9.0", ">=1.0.0");

        Assert.False(semver.IsSatisfied);
    }

    [Fact]
    public void IsPreRelease_WithPreReleaseVersion_ReturnsTrue()
    {
        var semver = new SemVer("1.0.0-alpha.1");

        Assert.True(semver.IsPreRelease);
    }

    [Fact]
    public void IsPreRelease_WithStableVersion_ReturnsFalse()
    {
        var semver = new SemVer("1.0.0");

        Assert.False(semver.IsPreRelease);
    }

    [Fact]
    public void IsRange_WithRangeString_ReturnsTrue()
    {
        var semver = new SemVer("^1.0.0");

        Assert.True(semver.IsRange);
    }

    [Fact]
    public void IsRange_WithPlainVersion_ReturnsFalse()
    {
        var semver = new SemVer("1.0.0");

        Assert.False(semver.IsRange);
    }

    [Fact]
    public void ToString_WithoutRange_ReturnsVersion()
    {
        var semver = new SemVer("1.2.3");

        var result = semver.ToString();

        Assert.Equal("1.2.3", result);
    }

    [Fact]
    public void ToString_WithRange_ReturnsRange()
    {
        var semver = new SemVer("1.2.3", ">=1.0.0");

        var result = semver.ToString(true);

        Assert.Equal(">=1.0.0", result);
    }

    [Fact]
    public void MaxSatisfying_WithMatchingVersions_ReturnsHighestSatisfied()
    {
        var range = new SemVer("^1.0.0");
        var versions = new[]
        {
            new SemVer("0.9.0"),
            new SemVer("1.0.0"),
            new SemVer("1.1.0"),
        };

        var result = range.MaxSatisfying(versions);

        Assert.NotNull(result);
        Assert.Equal("1.1.0", result.Version);
    }

    [Fact]
    public void MaxSatisfying_WithNoMatchingVersions_ReturnsNull()
    {
        var range = new SemVer("^5.0.0");
        var versions = new[]
        {
            new SemVer("1.0.0"),
            new SemVer("2.0.0"),
        };

        var result = range.MaxSatisfying(versions);

        Assert.Null(result);
    }

    [Fact]
    public void MaxSatisfying_ExcludesPreRelease_WhenFlagIsFalse()
    {
        var range = new SemVer("^1.0.0");
        var versions = new[]
        {
            new SemVer("1.0.0"),
            new SemVer("1.1.0-alpha.1"),
        };

        var result = range.MaxSatisfying(versions, false);

        Assert.NotNull(result);
        Assert.Equal("1.0.0", result.Version);
    }

    [Fact]
    public void Constructor_WithNullVersion_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new SemVer(null!, ">=1.0.0"));
    }

    [Fact]
    public void Constructor_WithNullRange_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new SemVer("1.0.0", null!));
    }

    [Fact]
    public void VersionObject_IsNotNull()
    {
        var semver = new SemVer("1.2.3");

        object versionObject = semver.VersionObject;
        Assert.NotNull(versionObject);
    }

    [Fact]
    public void RangeObject_IsNotNull()
    {
        var semver = new SemVer("1.2.3");

        object rangeObject = semver.RangeObject;
        Assert.NotNull(rangeObject);
    }

    [Fact]
    public void IsSatisfied_WithRangeVersion_DefaultRangeIsTilde()
    {
        // When created with just version, range is ~version
        var semver = new SemVer("1.2.3");

        // ~1.2.3 is satisfied by 1.2.3
        Assert.True(semver.IsSatisfied);
    }
}
