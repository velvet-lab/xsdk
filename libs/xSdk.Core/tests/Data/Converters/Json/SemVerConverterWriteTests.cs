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

public class SemVerConverterWriteTests
{
    [Fact]
    public void Write_WritesBase64String_ContainsVersionAndRange()
    {
        var conv = new SemVerConverter();
        var sem = new SemVer("1.2.3", "~1.2.3");

        using var ms = new MemoryStream();
        using (var writer = new Utf8JsonWriter(ms))
        {
            conv.Write(writer, sem, new JsonSerializerOptions());
            writer.Flush();
        }

        var json = Encoding.UTF8.GetString(ms.ToArray());
        var doc = JsonDocument.Parse(json);
        var value = doc.RootElement.GetString();

        var decoded = xSdk.Tools.Base64Tools.ConvertFromBase64(value);

        Assert.Equal("1.2.3;~1.2.3", decoded);
    }
}
