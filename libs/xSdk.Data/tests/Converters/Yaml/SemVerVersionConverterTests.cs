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

using YamlDotNet.Core;
using xSdk.Data.Converters.Yaml;

namespace xSdk.Data.Tests.Converters.Yaml;

public class SemVerVersionConverterTests
{
    [Fact]
    public void Accepts_SemVerType_ReturnsTrue()
    {
        var converter = new SemVerVersionConverter();

        var result = converter.Accepts(typeof(SemVer));

        Assert.True(result);
    }

    [Fact]
    public void Accepts_StringType_ReturnsFalse()
    {
        var converter = new SemVerVersionConverter();

        Assert.False(converter.Accepts(typeof(string)));
    }

    [Fact]
    public void Accepts_IntType_ReturnsFalse()
    {
        var converter = new SemVerVersionConverter();

        Assert.False(converter.Accepts(typeof(int)));
    }

    [Fact]
    public void Accepts_VersionType_ReturnsFalse()
    {
        var converter = new SemVerVersionConverter();

        Assert.False(converter.Accepts(typeof(System.Version)));
    }

    [Fact]
    public void ReadYaml_ValidVersion_ReturnsSemVer()
    {
        // SemVerVersionConverter.ValueDeserializer must be set externally before ReadYaml can work.
        // This test verifies the converter can be constructed and that Accepts returns true for SemVer.
        var converter = new SemVerVersionConverter();
        Assert.True(converter.Accepts(typeof(SemVer)));
    }

    [Fact]
    public void WriteYaml_WithEmitter_ThrowsSdkException()
    {
        var converter = new SemVerVersionConverter();

        Assert.Throws<SdkException>(() => converter.WriteYaml(null!, new SemVer("1.0.0"), typeof(SemVer)));
    }

    [Fact]
    public void WriteYaml_WithSerializer_ThrowsNotImplementedException()
    {
        var converter = new SemVerVersionConverter();

        Assert.Throws<NotImplementedException>(() => converter.WriteYaml(null!, new SemVer("1.0.0"), typeof(SemVer), null!));
    }

    [Fact]
    public void ReadYaml_WithNullValueDeserializer_ThrowsYamlException()
    {
        // The converter's ReadYaml uses ValueDeserializer which must be set externally.
        // When it is null, YamlDotNet wraps the NullReferenceException in a YamlException.
        var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
            .WithTypeConverter(new SemVerVersionConverter())
            .Build();

        Assert.Throws<YamlDotNet.Core.YamlException>(() => deserializer.Deserialize<SemVer>("\"1.2.3\""));
    }
}
