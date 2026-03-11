using System.Text.Json;
using xSdk.Data.Converters.Json;

namespace xSdk.Plugin.Tests.Data.Converters.Json;

public class VersionConverterTests
{
    private readonly JsonSerializerOptions _options;

    public VersionConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new VersionConverter());
    }

    [Fact]
    public void Write_SerializesVersionAsString()
    {
        var version = new Version(1, 2, 3, 4);

        var json = JsonSerializer.Serialize(version, _options);

        Assert.Equal("\"1.2.3.4\"", json);
    }

    [Fact]
    public void Read_ParsesVersionFromString()
    {
        var json = "\"1.2.3\"";

        var result = JsonSerializer.Deserialize<Version>(json, _options);

        Assert.NotNull(result);
        Assert.Equal(1, result.Major);
        Assert.Equal(2, result.Minor);
        Assert.Equal(3, result.Build);
    }

    [Fact]
    public void Read_WithEmptyString_ReturnsNull()
    {
        var json = "\"\"";

        var result = JsonSerializer.Deserialize<Version>(json, _options);

        Assert.Null(result);
    }

    [Fact]
    public void RoundTrip_SerializeDeserialize_RetainsValue()
    {
        var original = new Version(2, 5, 1);

        var json = JsonSerializer.Serialize(original, _options);
        var result = JsonSerializer.Deserialize<Version>(json, _options);

        Assert.Equal(original, result);
    }
}
