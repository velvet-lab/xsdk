using System.Text.Json;

namespace xSdk.Data.Converters.Mapper
{
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
            var converter = new JsonElementConverter.ToEntityProperty();
            var actual = converter.Convert(JsonEl, default);

            Assert.NotNull(actual);
            Assert.IsType<string>(actual);
        }

        [Fact]
        public void ConvertBase64StringToJsonElement()
        {
            var converter = new JsonElementConverter.ToModelProperty();
            var actual = converter.Convert(JsonString, default);

            Assert.NotNull(actual);
        }
    }
}
