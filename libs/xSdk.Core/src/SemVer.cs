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

using System.Text.Json.Serialization;
using xSdk.Tools;

namespace xSdk;

public sealed class SemVer
{
    private readonly string? _origin;

    public SemVer(string version)
    {
        _origin = version;

        if (HasRangeStrings(version))
        {
            Range = version;
            Version = ReplaceRangeStrings(version);
        }
        else
        {
            Version = version;
            Range = $"~{version}";
        }

        Version = ConvertToSemVer(Version);

        RangeObject = new SemanticVersioning.Range(Range);
        VersionObject = new SemanticVersioning.Version(Version);
    }

    public SemVer(string version, string range)
    {
        _origin = version;

        Version = version ?? throw new ArgumentNullException(nameof(version));
        Range = range ?? throw new ArgumentNullException(nameof(range));

        if (HasRangeStrings(version))
        {
            Version = ReplaceRangeStrings(version);
        }

        Version = ConvertToSemVer(Version);

        RangeObject = new SemanticVersioning.Range(Range);
        VersionObject = new SemanticVersioning.Version(Version);
    }

    private SemVer(SemanticVersioning.Version versionObject, SemanticVersioning.Range rangeObject)
    {
        Version = versionObject.ToString();
        Range = rangeObject.ToString();

        VersionObject = versionObject;
        RangeObject = rangeObject;
    }

    [JsonIgnore]
    public SemanticVersioning.Version VersionObject { get; }

    [JsonIgnore]
    public SemanticVersioning.Range RangeObject { get; }

    [JsonIgnore]
    public bool IsPreRelease => VersionObject.IsPreRelease;

    public string Version { get; }

    public string Range { get; }

    [JsonIgnore]
    public bool IsSatisfied => RangeObject.IsSatisfied(VersionObject);

    [JsonIgnore]
    public bool IsRange => HasRangeStrings(_origin);

    public SemVer? MaxSatisfying(IEnumerable<SemVer> versions) => MaxSatisfying(versions, false);

    public SemVer? MaxSatisfying(IEnumerable<SemVer> versions, bool includePreRelease)
    {
        SemanticVersioning.Version? highestVersion = null;

        foreach (SemVer version in versions)
        {
            SemanticVersioning.Version baseVersion = version.VersionObject.BaseVersion();
            if (RangeObject.IsSatisfied(baseVersion) && (!version.IsPreRelease || (version.IsPreRelease && includePreRelease)))
            {
                highestVersion ??= version.VersionObject;

                if (version.VersionObject > highestVersion)
                {
                    highestVersion = version.VersionObject;
                }                
            }
        }

        if (highestVersion is not null)
        {
            return new SemVer(highestVersion, RangeObject);
        }

        //var versionObject = _rangeObject.MaxSatisfying(nativeVersions);
        //if (versionObject != null)
        //    return new SemVer(versionObject, _rangeObject);

        return null;
    }

    public override string ToString() => ToString(false);

    public string ToString(bool useRange)
    {
        string version = Version;
        if (useRange)
        {
            version = Range;
        }

        return version;
    }

    /// <summary>
    /// Converts the primaryKey of the current System.Version object to its equivalent System.String
    /// representation. A specified count indicates the number of components to return.
    /// </summary>
    /// <param name="fieldCount">The number of components to return. The fieldCount ranges from 0 to 4.</param>
    /// <returns>
    /// The System.String representation of the values of the major, minor, build, and revision components of the
    /// current System.Version object, each separated by a period character ('.'). The fieldCount parameter determines
    /// how many components are returned.
    ///
    ///     0 – An empty string ("").
    ///     1 – major
    ///     2 – major.minor
    ///     3 – major.minor.build
    ///     4 – major.minor.build.revision
    /// </returns>
    public string ToString(int fieldCount)
    {
        int currentCount = Version.Count(x => x == '.') + 1;
        if (fieldCount > currentCount)
        {
            fieldCount = currentCount;
        }

        return new Version(Version).ToString(fieldCount);
    }

    // "operator==" should not be overloaded on reference types
    // see csharpsquid:S3875
    //public static bool operator ==(SemVer left, SemVer right)
    //{
    //    if (left is null)
    //    {
    //        if (right is null)
    //        {
    //            return true;
    //        }

    //        return false;
    //    }
    //    else
    //    {
    //        if (right is null)
    //        {
    //            return false;
    //        }
    //    }

    //    return left.VersionObject == right.VersionObject;
    //}

    //public static bool operator !=(SemVer left, SemVer right) => !(left == right);

    public static bool operator >(SemVer left, SemVer right)
    {
        if (left is null)
        {
            if (right is null)
            {
                return false;
            }

            return false;
        }
        else
        {
            if (right is null)
            {
                return true;
            }
        }

        return left.VersionObject > right.VersionObject;
    }

    public static bool operator <(SemVer left, SemVer right) => !(left > right);

    public static bool operator >=(SemVer left, SemVer right)
    {
        if (left is null)
        {
            if (right is null)
            {
                return false;
            }

            return false;
        }
        else
        {
            if (right is null)
            {
                return true;
            }
        }

        return left.VersionObject >= right.VersionObject;
    }

    public static bool operator <=(SemVer left, SemVer right) => !(left > right);

    public static bool HasRangeStrings(string? value)
    {
        if(string.IsNullOrEmpty(value))
        {
            return false;
        }

        return value.IndexOf('~') > -1 || value.IndexOf('^') > -1 || value.IndexOf(".x") > -1;
    }

    private static string ReplaceRangeStrings(string value)
    {
        string result = value.Replace("~", string.Empty).Replace("^", string.Empty).Replace(".x", string.Empty);
        if (result.Contains('-', StringComparison.CurrentCulture))
        {
            result = result.Substring(0, result.IndexOf('-'));
        }

        return result;
    }

    private static string ConvertToSemVer(string value)
    {
        string preReleaseString = string.Empty;
        if (value.Contains('-', StringComparison.CurrentCulture))
        {
            preReleaseString = value.Substring(value.IndexOf('-'));
        }

        string tempValue = ReplaceRangeStrings(value);
        if (tempValue.Count(x => x == '.') > 2)
        {
            var tmpVersion = new Version(tempValue);
            tempValue = $"{tmpVersion.Major}.{tmpVersion.Minor}.{tmpVersion.Build}";
        }

        if (tempValue.Count(x => x == '.') < 2)
        {
            tempValue = $"{tempValue}.0";
        }

        if (!tempValue.Contains('.', StringComparison.CurrentCulture))
        {
            tempValue = $"{tempValue}.0.0";
        }

        return $"{tempValue}{preReleaseString}";
    }

    public override int GetHashCode() => ObjectTools.CreateAutomaticHashCode(this);

    public override bool Equals(object? obj)
    {
        if (obj is not null)
        {
            return this == (SemVer)obj;
        }

        return false;
    }
}
