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

using System.Text;
using System.Text.Json;

namespace xSdk.Data.Converters.Json;

public class SemVerConverterTests
{
    [Fact]
    public void Read_Base64EncodedString_ReturnsSemVerWithVersionAndRange()
    {
        var tmp = "1.2.3;~1.2.3";
        var encoded = xSdk.Tools.Base64Tools.ConvertToBase64(tmp);
        var json = $"\"{encoded}\"";
        var bytes = Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(bytes);
        reader.Read();

        var conv = new SemVerConverter();
        var sem = conv.Read(ref reader, typeof(SemVer), new JsonSerializerOptions());

        Assert.Equal("1.2.3", sem.Version);
        Assert.Equal("~1.2.3", sem.Range);
    }
}
