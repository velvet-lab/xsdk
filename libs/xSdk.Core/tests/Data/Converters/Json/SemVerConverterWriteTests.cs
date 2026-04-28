using System.IO;
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
