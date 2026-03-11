using xSdk.Shared;

namespace xSdk.Plugin.Tests.Shared;

public class TypeConverterTests
{
    [Theory]
    [InlineData("42", 42)]
    [InlineData("0", 0)]
    [InlineData("-1", -1)]
    public void ConvertTo_IntFromString_ReturnsCorrectValue(string input, int expected)
    {
        var result = TypeConverter.ConvertTo<int>(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertTo_DoubleFromString_ReturnsCorrectValue()
    {
        var result = TypeConverter.ConvertTo<double>("3.14");

        Assert.Equal(3.14, result, precision: 2);
    }

    [Fact]
    public void ConvertTo_BoolFromString_ReturnsTrue()
    {
        var result = TypeConverter.ConvertTo<bool>("true");

        Assert.True(result);
    }

    [Fact]
    public void ConvertTo_BoolFromString_ReturnsFalse()
    {
        var result = TypeConverter.ConvertTo<bool>("false");

        Assert.False(result);
    }

    [Fact]
    public void ConvertTo_LongFromString_ReturnsCorrectValue()
    {
        var result = TypeConverter.ConvertTo<long>("9999999999");

        Assert.Equal(9999999999L, result);
    }

    [Fact]
    public void ConvertTo_StringFromInt_ReturnsCorrectValue()
    {
        var result = TypeConverter.ConvertTo<string>(42);

        Assert.Equal("42", result);
    }

    [Fact]
    public void ConvertTo_GuidFromString_ReturnsCorrectGuid()
    {
        var guid = Guid.NewGuid();

        var result = TypeConverter.ConvertTo<Guid>(guid.ToString());

        Assert.Equal(guid, result);
    }

    [Fact]
    public void ConvertTo_InvalidInput_ReturnsDefault()
    {
        var result = TypeConverter.ConvertTo<int>("not-a-number");

        Assert.Equal(0, result);
    }

    [Theory]
    [InlineData("42", typeof(int), 42)]
    [InlineData("3.14", typeof(double), 3.14)]
    [InlineData("true", typeof(bool), true)]
    public void ConvertTo_WithTargetType_ReturnsCorrectValue(string input, Type targetType, object expected)
    {
        var result = TypeConverter.ConvertTo(input, targetType);

        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertTo_WithNullValue_ReturnsDefaultForValueType()
    {
        var result = TypeConverter.ConvertTo(null, typeof(int));

        // For value types, null input returns the default value (0)
        Assert.Equal(0, result);
    }

    [Fact]
    public void ConvertTo_DecimalFromString_ReturnsCorrectValue()
    {
        var result = TypeConverter.ConvertTo<decimal>("123.45");

        Assert.Equal(123.45m, result);
    }

    [Fact]
    public void ConvertTo_ByteFromString_ReturnsCorrectValue()
    {
        var result = TypeConverter.ConvertTo<byte>("200");

        Assert.Equal((byte)200, result);
    }

    [Fact]
    public void ConvertTo_FloatFromString_ReturnsCorrectValue()
    {
        var result = TypeConverter.ConvertTo<float>("1.5");

        Assert.Equal(1.5f, result, precision: 1);
    }

    [Fact]
    public void ConvertTo_ShortFromString_ReturnsCorrectValue()
    {
        var result = TypeConverter.ConvertTo<short>("100");

        Assert.Equal((short)100, result);
    }
}
