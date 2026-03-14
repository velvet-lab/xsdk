using System.Text.Json;
using xSdk.Data.Converters.Json;

namespace xSdk.Data.Tests.Converters.Json;

public class ObjectConverterTests
{
    private readonly JsonSerializerOptions _options;

    public ObjectConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new ObjectConverter());
    }

    [Fact]
    public void Read_StringToken_ReturnsStringValue()
    {
        var json = "\"hello world\"";

        var result = JsonSerializer.Deserialize<object>(json, _options);

        Assert.Equal("hello world", result);
    }

    [Fact]
    public void Read_EmptyString_ReturnsNull()
    {
        var json = "\"\"";

        var result = JsonSerializer.Deserialize<object>(json, _options);

        Assert.Null(result);
    }

    [Fact]
    public void Write_StringValue_SerializesAsString()
    {
        object value = "test-value";

        var json = JsonSerializer.Serialize(value, _options);

        Assert.Equal("\"test-value\"", json);
    }
}
