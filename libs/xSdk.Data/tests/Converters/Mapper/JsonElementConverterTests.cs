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

namespace xSdk.Data.Converters.Mapper;

public class JsonElementConverterTests
{

    private readonly JsonElement _jsonEl = JsonSerializer.SerializeToElement(@"
{
    ""key1"": ""value1"",
    ""key2"": 123,
    ""key3"": true
}
");

    private readonly static string _jsonString =
        "IlxyXG57XHJcbiAgICBcdTAwMjJrZXkxXHUwMDIyOiBcdTAwMjJ2YWx1ZTFcdTAwMjIsXHJcbiAgICBcdTAwMjJrZXkyXHUwMDIyOiAxMjMsXHJcbiAgICBcdTAwMjJrZXkzXHUwMDIyOiB0cnVlXHJcbn1cclxuIg==";

    [Fact]
    public void ConvertJsonElementToBase64String()
    {
        var actual = JsonElementConverter.Convert(_jsonEl);

        Assert.NotNull(actual);
        Assert.IsType<string>(actual);
    }

    [Fact]
    public void ConvertBase64StringToJsonElement()
    {
        var actual = JsonElementConverter.Convert(_jsonString);
    }
}
