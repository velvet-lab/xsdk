using xSdk.Data.Converters.Mapper;

namespace xSdk.Plugin.Tests.Data.Converters.Mapper;

public class GuidConverterTests
{
    [Fact]
    public void Convert_FromValidStringToGuid_ReturnsGuid()
    {
        var guid = Guid.NewGuid();

        var result = GuidConverter.Convert(guid.ToString());

        Assert.Equal(guid, result);
    }

    [Fact]
    public void Convert_FromInvalidString_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => GuidConverter.Convert("not-a-guid"));
    }

    [Fact]
    public void Convert_FromGuidToString_ReturnsString()
    {
        var guid = Guid.NewGuid();

        var result = GuidConverter.Convert(guid);

        Assert.Equal(guid.ToString(), result);
    }

    [Fact]
    public void Convert_FromEmptyGuidToString_ReturnsStringRepresentation()
    {
        var result = GuidConverter.Convert(Guid.Empty);

        Assert.Equal(Guid.Empty.ToString(), result);
    }
}
