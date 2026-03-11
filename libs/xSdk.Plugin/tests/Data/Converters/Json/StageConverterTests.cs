using System.Text.Json;
using xSdk.Data.Converters.Json;

namespace xSdk.Plugin.Tests.Data.Converters.Json;

public class StageConverterTests
{
    private readonly JsonSerializerOptions _options;

    public StageConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new StageConverter());
    }

    [Theory]
    [InlineData(Stage.Development, "\"Development\"")]
    [InlineData(Stage.Integration, "\"Integration\"")]
    [InlineData(Stage.Production, "\"Production\"")]
    [InlineData(Stage.None, "\"None\"")]
    public void Write_SerializesStageAsString(Stage stage, string expectedJson)
    {
        var json = JsonSerializer.Serialize(stage, _options);

        Assert.Equal(expectedJson, json);
    }

    [Theory]
    [InlineData("\"Development\"", Stage.Development)]
    [InlineData("\"Integration\"", Stage.Integration)]
    [InlineData("\"Production\"", Stage.Production)]
    [InlineData("\"None\"", Stage.None)]
    public void Read_ParsesStageFromString(string json, Stage expected)
    {
        var result = JsonSerializer.Deserialize<Stage>(json, _options);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Read_EmptyString_ReturnsNone()
    {
        var json = "\"\"";

        var result = JsonSerializer.Deserialize<Stage>(json, _options);

        Assert.Equal(Stage.None, result);
    }

    [Fact]
    public void RoundTrip_SerializeDeserialize_RetainsValue()
    {
        var original = Stage.Production;

        var json = JsonSerializer.Serialize(original, _options);
        var result = JsonSerializer.Deserialize<Stage>(json, _options);

        Assert.Equal(original, result);
    }
}
