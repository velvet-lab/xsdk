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
using xSdk.Shared;

namespace xSdk;

public sealed class SemVer
{
    private readonly string _origin;

    private readonly string _version;
    private readonly string _range;
    private readonly SemanticVersioning.Version _versionObject;
    private readonly SemanticVersioning.Range _rangeObject;

    public SemVer(string version)
    {
        _origin = version;

        if (HasRangeStrings(version))
        {
            _range = version;
            _version = ReplaceRangeStrings(version);
        }
        else
        {
            _version = version;
            _range = $"~{version}";
        }

        _version = ConvertToSemVer(_version);

        this._rangeObject = new SemanticVersioning.Range(_range);
        this._versionObject = new SemanticVersioning.Version(_version);
    }

    public SemVer(string version, string range)
    {
        _origin = version;

        this._version = version ?? throw new ArgumentNullException(nameof(version));
        this._range = range ?? throw new ArgumentNullException(nameof(range));

        if (HasRangeStrings(version))
            _version = ReplaceRangeStrings(version);

        _version = ConvertToSemVer(_version);

        this._rangeObject = new SemanticVersioning.Range(_range);
        this._versionObject = new SemanticVersioning.Version(_version);
    }

    private SemVer(SemanticVersioning.Version versionObject, SemanticVersioning.Range rangeObject)
    {
        this._version = versionObject.ToString();
        this._range = rangeObject.ToString();

        this._versionObject = versionObject ?? throw new ArgumentNullException(nameof(versionObject));
        this._rangeObject = rangeObject ?? throw new ArgumentNullException(nameof(rangeObject));
    }

    [JsonIgnore]
    public SemanticVersioning.Version VersionObject => _versionObject;

    [JsonIgnore]
    public SemanticVersioning.Range RangeObject => _rangeObject;

    [JsonIgnore]
    public bool IsPreRelease => _versionObject.IsPreRelease;

    public string Version => _version;

    public string Range => _range;

    [JsonIgnore]
    public bool IsSatisfied => _rangeObject.IsSatisfied(this._versionObject);

    [JsonIgnore]
    public bool IsRange => HasRangeStrings(this._origin);

    public SemVer MaxSatisfying(IEnumerable<SemVer> versions) => MaxSatisfying(versions, false);

    public SemVer MaxSatisfying(IEnumerable<SemVer> versions, bool includePreRelease)
    {
        SemanticVersioning.Version highestVersion = null;

        foreach (var version in versions)
        {
            var baseVersion = version._versionObject.BaseVersion();
            if (_rangeObject.IsSatisfied(baseVersion))
            {
                if (!version.IsPreRelease || version.IsPreRelease && includePreRelease)
                {
                    if (highestVersion == null)
                    {
                        highestVersion = version._versionObject;
                    }
                    else
                    {
                        if (version._versionObject > highestVersion)
                        {
                            highestVersion = version._versionObject;
                        }
                    }
                }
            }
        }

        if (highestVersion != null)
        {
            return new SemVer(highestVersion, _rangeObject);
        }

        //var versionObject = _rangeObject.MaxSatisfying(nativeVersions);
        //if (versionObject != null)
        //    return new SemVer(versionObject, _rangeObject);

        return null;
    }

    public override string ToString() => ToString(false);

    public string ToString(bool useRange)
    {
        var version = this.Version;
        if (useRange)
            version = this.Range;

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
        var currentCount = (this.Version.Count(x => x == '.') + 1);
        if (fieldCount > currentCount)
        {
            fieldCount = currentCount;
        }
        return new Version(this.Version).ToString(fieldCount);
    }

    public static bool operator ==(SemVer left, SemVer right)
    {
        if (left is null)
        {
            if (right is null)
                return true;

            return false;
        }
        else
        {
            if (right is null)
                return false;
        }
        return left.VersionObject == right.VersionObject;
    }

    public static bool operator !=(SemVer left, SemVer right) => !(left == right);

    public static bool operator >(SemVer left, SemVer right)
    {
        if (left is null)
        {
            if (right is null)
                return false;

            return false;
        }
        else
        {
            if (right is null)
                return true;
        }
        return left.VersionObject > right.VersionObject;
    }

    public static bool operator <(SemVer left, SemVer right) => !(left < right);

    public static bool operator >=(SemVer left, SemVer right)
    {
        if (left is null)
        {
            if (right is null)
                return false;

            return false;
        }
        else
        {
            if (right is null)
                return true;
        }
        return left.VersionObject >= right.VersionObject;
    }

    public static bool operator <=(SemVer left, SemVer right) => !(left <= right);

    public static bool HasRangeStrings(string value) => (value.IndexOf("~") > -1 || value.IndexOf("^") > -1 || value.IndexOf(".x") > -1);

    private static string ReplaceRangeStrings(string value)
    {
        var result = value.Replace("~", "").Replace("^", "").Replace(".x", "");
        if (result.IndexOf("-") != -1)
        {
            result = result.Substring(0, result.IndexOf("-"));
        }
        return result;
    }

    private static string ConvertToSemVer(string value)
    {
        var preReleaseString = string.Empty;
        if (value.IndexOf("-") != -1)
        {
            preReleaseString = value.Substring(value.IndexOf("-"));
        }

        var tempValue = ReplaceRangeStrings(value);
        if (tempValue.Count(x => x == '.') > 2)
        {
            var tmpVersion = new Version(tempValue);
            tempValue = $"{tmpVersion.Major}.{tmpVersion.Minor}.{tmpVersion.Build}";
        }

        if (tempValue.Count(x => x == '.') < 2)
            tempValue = $"{tempValue}.0";

        if (tempValue.IndexOf(".") == -1)
            tempValue = $"{tempValue}.0.0";

        return $"{tempValue}{preReleaseString}";
    }

    public override int GetHashCode() => ObjectHelper.CreateAutomaticHashCode(this);

    public override bool Equals(object obj) => this == (SemVer)obj;
}
