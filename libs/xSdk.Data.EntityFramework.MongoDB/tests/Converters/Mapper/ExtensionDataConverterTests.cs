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
            var json = @"
{
    ""key1"": ""value1"",
    ""key2"": 123,
    ""key3"": true
}";
            var actual = ExtensionDataConverter.Convert(json);

            Assert.NotNull(actual);
            Assert.IsType<Dictionary<string, object>>(actual);
        }

        [Fact]
        public void ConvertFromDictionary()
        {
            var dictionary = new Dictionary<string, object>
            {
                { "key1", "value1" },
                { "key2", 123 },
                { "key3", true }
            };

            var actual = ExtensionDataConverter.Convert(dictionary);

            Assert.NotNull(actual);
            Assert.IsType<string>(actual);
        }
    }
}
