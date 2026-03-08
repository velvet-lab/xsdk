using System.Text.Json;

namespace xSdk.Data.Converters.Mapper;

public class JsonElementConverterTests
{

    private readonly JsonElement JsonEl = JsonSerializer.SerializeToElement(@"
{
    ""key1"": ""value1"",
    ""key2"": 123,
    ""key3"": true
}
");

    private readonly static string JsonString = "IlxyXG57XHJcbiAgICBcdTAwMjJrZXkxXHUwMDIyOiBcdTAwMjJ2YWx1ZTFcdTAwMjIsXHJcbiAgICBcdTAwMjJrZXkyXHUwMDIyOiAxMjMsXHJcbiAgICBcdTAwMjJrZXkzXHUwMDIyOiB0cnVlXHJcbn1cclxuIg==";

    [Fact]
    public void ConvertJsonElementToBase64String()
    {
        var actual = JsonElementConverter.Convert(JsonEl);

        Assert.NotNull(actual);
        Assert.IsType<string>(actual);
    }

    [Fact]
    public void ConvertBase64StringToJsonElement()
    {
        var actual = JsonElementConverter.Convert(JsonString);
    }
}
