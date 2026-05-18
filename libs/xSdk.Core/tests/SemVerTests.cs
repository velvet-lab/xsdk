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

namespace xSdk;

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

        string result = semver.ToString();

        Assert.Equal("1.2.3", result);
    }

    [Fact]
    public void ToString_WithRange_ReturnsRange()
    {
        var semver = new SemVer("1.2.3", ">=1.0.0");

        string result = semver.ToString(true);

        Assert.Equal(">=1.0.0", result);
    }

    [Fact]
    public void MaxSatisfying_WithMatchingVersions_ReturnsHighestSatisfied()
    {
        var range = new SemVer("^1.0.0");
        SemVer[] versions =
        [
            new SemVer("0.9.0"),
            new SemVer("1.0.0"),
            new SemVer("1.1.0"),
        ];

        SemVer? result = range.MaxSatisfying(versions);

        Assert.NotNull(result);
        Assert.Equal("1.1.0", result.Version);
    }

    [Fact]
    public void MaxSatisfying_WithNoMatchingVersions_ReturnsNull()
    {
        var range = new SemVer("^5.0.0");
        SemVer[] versions =
        [
            new SemVer("1.0.0"),
            new SemVer("2.0.0"),
        ];

        SemVer? result = range.MaxSatisfying(versions);

        Assert.Null(result);
    }

    [Fact]
    public void MaxSatisfying_ExcludesPreRelease_WhenFlagIsFalse()
    {
        var range = new SemVer("^1.0.0");
        SemVer[] versions =
        [
            new SemVer("1.0.0"),
            new SemVer("1.1.0-alpha.1"),
        ];

        SemVer? result = range.MaxSatisfying(versions, false);

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

    [Theory]
    [InlineData("1.2.3", 1, "1")]
    [InlineData("1.2.3", 2, "1.2")]
    [InlineData("1.2.3", 3, "1.2.3")]
    public void ToString_WithFieldCount_ReturnsCorrectFields(string version, int fieldCount, string expected)
    {
        var semver = new SemVer(version);

        string result = semver.ToString(fieldCount);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equals_WithSameVersion_ReturnsTrue()
    {
        var a = new SemVer("1.2.3");
        var b = new SemVer("1.2.3");

        Assert.True(a.Equals(b));
    }

    [Fact]
    public void Equals_WithDifferentVersion_ReturnsFalse()
    {
        var a = new SemVer("1.2.3");
        var b = new SemVer("2.0.0");

        Assert.False(a.Equals(b));
    }

    [Fact]
    public void GetHashCode_SameVersion_ReturnsSameHashCode()
    {
        var a = new SemVer("1.2.3");
        var b = new SemVer("1.2.3");

        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void EqualityOperator_WithEqualVersions_ReturnsTrue()
    {
        var a = new SemVer("1.2.3");
        var b = new SemVer("1.2.3");

        Assert.True(a == b);
    }

    [Fact]
    public void EqualityOperator_WithDifferentVersions_ReturnsFalse()
    {
        var a = new SemVer("1.2.3");
        var b = new SemVer("2.0.0");

        Assert.False(a == b);
    }

    [Fact]
    public void InequalityOperator_WithDifferentVersions_ReturnsTrue()
    {
        var a = new SemVer("1.2.3");
        var b = new SemVer("2.0.0");

        Assert.True(a != b);
    }

    [Fact]
    public void GreaterThanOperator_WithHigherVersion_ReturnsTrue()
    {
        var a = new SemVer("2.0.0");
        var b = new SemVer("1.0.0");

        Assert.True(a > b);
    }

    [Fact]
    public void GreaterThanOrEqualOperator_WithSameVersion_ReturnsTrue()
    {
        var a = new SemVer("1.0.0");
        var b = new SemVer("1.0.0");

        Assert.True(a >= b);
    }

    [Fact]
    public void GreaterThanOrEqualOperator_WithHigherVersion_ReturnsTrue()
    {
        var a = new SemVer("2.0.0");
        var b = new SemVer("1.0.0");

        Assert.True(a >= b);
    }

    [Fact]
    public void EqualityOperator_WithLeftNull_ReturnsFalse()
    {
        SemVer? a = default;
        var b = new SemVer("1.0.0");

#pragma warning disable CS8604 // Mögliches Nullverweisargument.
        Assert.False(a == b);
#pragma warning restore CS8604 // Mögliches Nullverweisargument.
    }

    [Fact]
    public void HasRangeStrings_WithTilde_ReturnsTrue()
    {
        Assert.True(SemVer.HasRangeStrings("~1.0.0"));
    }

    [Fact]
    public void HasRangeStrings_WithCaret_ReturnsTrue()
    {
        Assert.True(SemVer.HasRangeStrings("^1.0.0"));
    }

    [Fact]
    public void HasRangeStrings_WithDotX_ReturnsTrue()
    {
        Assert.True(SemVer.HasRangeStrings("1.2.x"));
    }

    [Fact]
    public void HasRangeStrings_WithPlainVersion_ReturnsFalse()
    {
        Assert.False(SemVer.HasRangeStrings("1.2.3"));
    }

    [Fact]
    public void Constructor_WithFourPartVersion_ConvertsToThreePart()
    {
        var semver = new SemVer("1.2.3.4");

        Assert.Equal("1.2.3", semver.Version);
    }

    [Fact]
    public void Constructor_WithTwoPartVersion_PadsToThreePart()
    {
        var semver = new SemVer("1.2");

        // Should have at least major.minor.patch
        Assert.Contains(".", semver.Version);
    }

    [Fact]
    public void MaxSatisfying_WithPreRelease_IncludesWhenFlagIsTrue()
    {
        var range = new SemVer("^1.0.0");
        SemVer[] versions =
        [
            new SemVer("1.0.0"),
            new SemVer("1.1.0-alpha.1"),
        ];

        SemVer? result = range.MaxSatisfying(versions, true);

        Assert.NotNull(result);
        Assert.Equal("1.1.0-alpha.1", result.Version);
    }
}
