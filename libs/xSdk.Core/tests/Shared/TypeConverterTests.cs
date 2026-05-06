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

namespace xSdk.Shared;

public class TypeConverterTests
{
    [Theory]
    [InlineData("42", 42)]
    [InlineData("0", 0)]
    [InlineData("-1", -1)]
    public void ConvertTo_IntFromString_ReturnsCorrectValue(string input, int expected)
    {
        int result = TypeConverter.ConvertTo<int>(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertTo_DoubleFromString_ReturnsCorrectValue()
    {
        double result = TypeConverter.ConvertTo<double>("3.14");

        Assert.Equal(3.14, result, precision: 2);
    }

    [Fact]
    public void ConvertTo_BoolFromString_ReturnsTrue()
    {
        bool result = TypeConverter.ConvertTo<bool>("true");

        Assert.True(result);
    }

    [Fact]
    public void ConvertTo_BoolFromString_ReturnsFalse()
    {
        bool result = TypeConverter.ConvertTo<bool>("false");

        Assert.False(result);
    }

    [Fact]
    public void ConvertTo_LongFromString_ReturnsCorrectValue()
    {
        long result = TypeConverter.ConvertTo<long>("9999999999");

        Assert.Equal(9999999999L, result);
    }

    [Fact]
    public void ConvertTo_StringFromInt_ReturnsCorrectValue()
    {
        string? result = TypeConverter.ConvertTo<string>(42);

        Assert.Equal("42", result);
    }

    [Fact]
    public void ConvertTo_GuidFromString_ReturnsCorrectGuid()
    {
        var guid = Guid.NewGuid();

        Guid result = TypeConverter.ConvertTo<Guid>(guid.ToString());

        Assert.Equal(guid, result);
    }

    [Fact]
    public void ConvertTo_InvalidInput_ReturnsDefault()
    {
        int result = TypeConverter.ConvertTo<int>("not-a-number");

        Assert.Equal(0, result);
    }

    [Theory]
    [InlineData("42", typeof(int), 42)]
    [InlineData("3.14", typeof(double), 3.14)]
    [InlineData("true", typeof(bool), true)]
    public void ConvertTo_WithTargetType_ReturnsCorrectValue(string input, Type targetType, object expected)
    {
        object? result = TypeConverter.ConvertTo(input, targetType);

        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertTo_WithNullValue_ReturnsDefaultForValueType()
    {
        object? result = TypeConverter.ConvertTo(null, typeof(int));

        // For value types, null input returns the default value (0)
        Assert.Equal(0, result);
    }

    [Fact]
    public void ConvertTo_DecimalFromString_ReturnsCorrectValue()
    {
        decimal result = TypeConverter.ConvertTo<decimal>("123.45");

        Assert.Equal(123.45m, result);
    }

    [Fact]
    public void ConvertTo_ByteFromString_ReturnsCorrectValue()
    {
        byte result = TypeConverter.ConvertTo<byte>("200");

        Assert.Equal((byte)200, result);
    }

    [Fact]
    public void ConvertTo_FloatFromString_ReturnsCorrectValue()
    {
        float result = TypeConverter.ConvertTo<float>("1.5");

        Assert.Equal(1.5f, result, precision: 1);
    }

    [Fact]
    public void ConvertTo_ShortFromString_ReturnsCorrectValue()
    {
        short result = TypeConverter.ConvertTo<short>("100");

        Assert.Equal((short)100, result);
    }

    [Fact]
    public void ConvertTo_DateTimeFromString_ReturnsCorrectValue()
    {
        DateTime result = TypeConverter.ConvertTo<DateTime>("2024-01-15");

        Assert.Equal(new DateTime(2024, 1, 15), result.Date);
    }

    [Fact]
    public void ConvertTo_TimeSpanFromString_ReturnsCorrectValue()
    {
        // TimeSpanParser uses a custom format: number + unit suffix (ms, s, m, h, d)
        TimeSpan result = TypeConverter.ConvertTo<TimeSpan>("90m");

        Assert.Equal(TimeSpan.FromMinutes(90), result);
    }

    [Fact]
    public void ConvertTo_EnumFromString_ReturnsCorrectValue()
    {
        DayOfWeek result = TypeConverter.ConvertTo<DayOfWeek>("Monday");

        Assert.Equal(DayOfWeek.Monday, result);
    }

    [Fact]
    public void ConvertTo_GuidViaObjectConvertTo_ReturnsCorrectGuid()
    {
        var guid = Guid.NewGuid();

        object? result = TypeConverter.ConvertTo(guid.ToString(), typeof(Guid));

        Assert.Equal(guid, result);
    }

    [Fact]
    public void GetValueType_IntegerString_ReturnsInt()
    {
        Type result = TypeConverter.GetValueType("42");

        Assert.Equal(typeof(int), result);
    }

    [Fact]
    public void GetValueType_FloatString_ReturnsFloat()
    {
        Type result = TypeConverter.GetValueType("3.14");

        Assert.Equal(typeof(float), result);
    }

    [Fact]
    public void GetValueType_BoolString_ReturnsBool()
    {
        Type result = TypeConverter.GetValueType("true");

        Assert.Equal(typeof(bool), result);
    }

    [Fact]
    public void GetValueType_PlainString_ReturnsString()
    {
        Type result = TypeConverter.GetValueType("hello");

        Assert.Equal(typeof(string), result);
    }

    [Fact]
    public void GetValueType_NullValue_ReturnsObject()
    {
        Type result = TypeConverter.GetValueType(null);

        Assert.Equal(typeof(object), result);
    }

    [Fact]
    public void IsEmpty_EmptyGuid_ReturnsTrue()
    {
        bool result = TypeConverter.IsEmpty(Guid.Empty, typeof(Guid));

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_NonEmptyGuid_ReturnsFalse()
    {
        bool result = TypeConverter.IsEmpty(Guid.NewGuid(), typeof(Guid));

        Assert.False(result);
    }

    [Fact]
    public void IsEmpty_ZeroInt_ReturnsTrue()
    {
        bool result = TypeConverter.IsEmpty(0, typeof(int));

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_NonZeroInt_ReturnsFalse()
    {
        bool result = TypeConverter.IsEmpty(5, typeof(int));

        Assert.False(result);
    }

    [Fact]
    public void IsEmpty_EmptyString_ReturnsTrue()
    {
        bool result = TypeConverter.IsEmpty("", typeof(string));

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_NonEmptyString_ReturnsFalse()
    {
        bool result = TypeConverter.IsEmpty("hello", typeof(string));

        Assert.False(result);
    }

    [Fact]
    public void IsEmpty_ZeroTimeSpan_ReturnsTrue()
    {
        bool result = TypeConverter.IsEmpty(TimeSpan.Zero, typeof(TimeSpan));

        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_NonZeroTimeSpan_ReturnsFalse()
    {
        bool result = TypeConverter.IsEmpty(TimeSpan.FromHours(1), typeof(TimeSpan));

        Assert.False(result);
    }

    [Fact]
    public void IsEmpty_EmptyStringTimeSpan_ReturnsTrue()
    {
        bool result = TypeConverter.IsEmpty("", typeof(TimeSpan));

        Assert.True(result);
    }

    [Fact]
    public void IsNumeric_Int_ReturnsTrue()
    {
        Assert.True(TypeConverter.IsNumeric<int>());
    }

    [Fact]
    public void IsNumeric_Double_ReturnsTrue()
    {
        Assert.True(TypeConverter.IsNumeric<double>());
    }

    [Fact]
    public void IsNumeric_String_ReturnsFalse()
    {
        Assert.False(TypeConverter.IsNumeric<string>());
    }

    [Fact]
    public void IsNumeric_Bool_ReturnsFalse()
    {
        Assert.False(TypeConverter.IsNumeric<bool>());
    }

    [Fact]
    public void IsNumeric_Type_IntReturnsTrue()
    {
        Assert.True(TypeConverter.IsNumeric(typeof(int)));
    }

    [Fact]
    public void IsNumeric_Type_StringReturnsFalse()
    {
        Assert.False(TypeConverter.IsNumeric(typeof(string)));
    }

    [Fact]
    public void IsNumeric_NullableInt_ReturnsTrue()
    {
        Assert.True(TypeConverter.IsNumeric(typeof(int?)));
    }

    [Fact]
    public void IsEmpty_ZeroDouble_ReturnsTrue()
    {
        Assert.True(TypeConverter.IsEmpty(0.0, typeof(double)));
    }

    [Fact]
    public void IsEmpty_ZeroDecimal_ReturnsTrue()
    {
        Assert.True(TypeConverter.IsEmpty(0m, typeof(decimal)));
    }

    [Fact]
    public void IsEmpty_ZeroLong_ReturnsTrue()
    {
        Assert.True(TypeConverter.IsEmpty(0L, typeof(long)));
    }

    [Fact]
    public void IsEmpty_ZeroShort_ReturnsTrue()
    {
        Assert.True(TypeConverter.IsEmpty((short)0, typeof(short)));
    }

    [Fact]
    public void IsEmpty_ZeroFloat_ReturnsTrue()
    {
        Assert.True(TypeConverter.IsEmpty(0f, typeof(float)));
    }

    [Fact]
    public void IsEmpty_ZeroByte_ReturnsTrue()
    {
        Assert.True(TypeConverter.IsEmpty((byte)0, typeof(byte)));
    }

    [Fact]
    public void IsEmpty_ZeroUlong_ReturnsTrue()
    {
        Assert.True(TypeConverter.IsEmpty(0UL, typeof(ulong)));
    }

    [Fact]
    public void IsEmpty_ZeroUshort_ReturnsTrue()
    {
        Assert.True(TypeConverter.IsEmpty((ushort)0, typeof(ushort)));
    }

    [Fact]
    public void IsEmpty_ZeroUint_ReturnsTrue()
    {
        Assert.True(TypeConverter.IsEmpty(0U, typeof(uint)));
    }

    [Fact]
    public void IsEmpty_ZeroSbyte_ReturnsTrue()
    {
        Assert.True(TypeConverter.IsEmpty((sbyte)0, typeof(sbyte)));
    }

    [Fact]
    public void ConvertTo_CharFromString_ReturnsCorrectValue()
    {
        char result = TypeConverter.ConvertTo<char>("A");

        Assert.Equal('A', result);
    }

    [Fact]
    public void ConvertTo_SbyteFromString_ReturnsCorrectValue()
    {
        sbyte result = TypeConverter.ConvertTo<sbyte>("127");

        Assert.Equal((sbyte)127, result);
    }

    [Fact]
    public void ConvertTo_UlongFromString_ReturnsCorrectValue()
    {
        ulong result = TypeConverter.ConvertTo<ulong>("9999");

        Assert.Equal(9999UL, result);
    }

    [Fact]
    public void ConvertTo_UintFromString_ReturnsCorrectValue()
    {
        uint result = TypeConverter.ConvertTo<uint>("200");

        Assert.Equal(200U, result);
    }

    [Fact]
    public void ConvertTo_UshortFromString_ReturnsCorrectValue()
    {
        ushort result = TypeConverter.ConvertTo<ushort>("100");

        Assert.Equal((ushort)100, result);
    }
}
