using System.Text.Json;
using xSdk;
using xSdk.Data.Converters.Json;

namespace xSdk.Plugin.Tests.Data.Converters.Json;

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
