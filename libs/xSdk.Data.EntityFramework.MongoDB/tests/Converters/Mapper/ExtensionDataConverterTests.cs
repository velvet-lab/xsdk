using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xSdk.Data.Converters.Mapper
{
    public sealed class ExtensionDataConverterTests
    {
        [Fact]
        public void ConvertToDictionary()
        {
            var converter = new ExtensionDataConverter.ToModelProperty();

            var json = @"
{
    ""key1"": ""value1"",
    ""key2"": 123,
    ""key3"": true
}";
            var actual = converter.Convert(json, null);

            Assert.NotNull(actual);
            Assert.IsType<Dictionary<string, object>>(actual);
        }

        [Fact]
        public void ConvertFromDictionary()
        {
            var converter = new ExtensionDataConverter.ToEntityProperty();
            var dictionary = new Dictionary<string, object>
            {
                { "key1", "value1" },
                { "key2", 123 },
                { "key3", true }
            };

            var actual = converter.Convert(dictionary, null);

            Assert.NotNull(actual);
            Assert.IsType<string>(actual);
        }
    }
}
