using System.Text;
using System.Text.Json;
using xSdk.Data.Converters.Json;

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
