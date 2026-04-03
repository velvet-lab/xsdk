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

using System.Text.Json;
using xSdk;
using xSdk.Data.Converters.Json;

namespace xSdk.Data.Converters.Json;

public class SemVerJsonConverterTests
{
    private readonly JsonSerializerOptions _options;

    public SemVerJsonConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new SemVerConverter());
    }

    [Fact]
    public void RoundTrip_SimpleVersion_SerializesAndDeserializesCorrectly()
    {
        var original = new SemVer("1.2.3");

        var json = JsonSerializer.Serialize(original, _options);
        var result = JsonSerializer.Deserialize<SemVer>(json, _options);

        Assert.NotNull(result);
        Assert.Equal(original.Version, result.Version);
    }

    [Fact]
    public void Write_ProducesBase64EncodedString()
    {
        var semver = new SemVer("1.0.0");

        var json = JsonSerializer.Serialize(semver, _options);

        // Should not be plain text version because it's base64
        Assert.DoesNotContain("1.0.0", json);
    }

    [Fact]
    public void RoundTrip_VersionWithRange_PreservesRange()
    {
        var original = new SemVer("1.2.3", ">=1.0.0");

        var json = JsonSerializer.Serialize(original, _options);
        var result = JsonSerializer.Deserialize<SemVer>(json, _options);

        Assert.NotNull(result);
        Assert.Equal(original.Range, result.Range);
    }
}
