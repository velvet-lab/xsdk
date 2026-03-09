namespace xSdk.Data.Converters.Mapper;

public class SemVerConverterTests
{
    private readonly SemVer _version = new SemVer("1.2.3");

    private readonly string _versionString = "1.2.3";

    [Fact]
    public void ConvertSemVerToString()
    {
        var actual = SemVerConverter.Convert(_version);

        Assert.NotNull(actual);
        Assert.IsType<string>(actual);
    }

    [Fact]
    public void ConvertStringToSemVer()
    {
        var actual = SemVerConverter.Convert(_versionString);

        Assert.NotNull(actual);
        Assert.IsType<SemVer>(actual);
    }
}
