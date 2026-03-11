using xSdk.Data.Converters.Yaml;

namespace xSdk.Data.Tests.Converters.Yaml;

public class SemVerVersionConverterTests
{
    [Fact]
    public void Accepts_SemVerType_ReturnsTrue()
    {
        var converter = new SemVerVersionConverter();

        var result = converter.Accepts(typeof(SemVer));

        Assert.True(result);
    }

    [Fact]
    public void Accepts_StringType_ReturnsFalse()
    {
        var converter = new SemVerVersionConverter();

        Assert.False(converter.Accepts(typeof(string)));
    }

    [Fact]
    public void Accepts_IntType_ReturnsFalse()
    {
        var converter = new SemVerVersionConverter();

        Assert.False(converter.Accepts(typeof(int)));
    }

    [Fact]
    public void Accepts_VersionType_ReturnsFalse()
    {
        var converter = new SemVerVersionConverter();

        Assert.False(converter.Accepts(typeof(Version)));
    }
}
