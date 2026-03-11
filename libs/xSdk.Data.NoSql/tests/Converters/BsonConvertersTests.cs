using LiteDB;
using xSdk.Data.Converters.Bson;
using xSdk.Data.Converters.Mapper;

namespace xSdk.Data.Tests.Converters;

public class BsonValueConverterTests
{
    [Fact]
    public void Convert_Boolean_ReturnsBsonBool()
    {
        var result = BsonValueConverter.Convert(true);

        Assert.Equal(BsonType.Boolean, result.Type);
        Assert.True(result.AsBoolean);
    }

    [Fact]
    public void Convert_Int_ReturnsBsonInt()
    {
        var result = BsonValueConverter.Convert(42);

        Assert.Equal(BsonType.Int32, result.Type);
        Assert.Equal(42, result.AsInt32);
    }

    [Fact]
    public void Convert_Long_ReturnsBsonLong()
    {
        var result = BsonValueConverter.Convert(100L);

        Assert.Equal(BsonType.Int64, result.Type);
        Assert.Equal(100L, result.AsInt64);
    }

    [Fact]
    public void Convert_Double_ReturnsBsonDouble()
    {
        var result = BsonValueConverter.Convert(3.14);

        Assert.Equal(BsonType.Double, result.Type);
        Assert.Equal(3.14, result.AsDouble, 2);
    }

    [Fact]
    public void Convert_String_ReturnsBsonString()
    {
        var result = BsonValueConverter.Convert("hello");

        Assert.Equal(BsonType.String, result.Type);
        Assert.Equal("hello", result.AsString);
    }

    [Fact]
    public void Convert_Guid_ReturnsBsonGuid()
    {
        var guid = Guid.NewGuid();

        var result = BsonValueConverter.Convert(guid);

        Assert.Equal(BsonType.Guid, result.Type);
        Assert.Equal(guid, result.AsGuid);
    }

    [Fact]
    public void Convert_DateTime_ReturnsBsonDateTime()
    {
        var dt = new DateTime(2024, 1, 1);

        var result = BsonValueConverter.Convert(dt);

        Assert.Equal(BsonType.DateTime, result.Type);
    }

    [Fact]
    public void Convert_ObjectId_ReturnsBsonObjectId()
    {
        var objectId = ObjectId.NewObjectId();

        var result = BsonValueConverter.Convert(objectId);

        Assert.Equal(BsonType.ObjectId, result.Type);
    }

    [Fact]
    public void Convert_UnsupportedType_ThrowsSdkException()
    {
        Assert.Throws<SdkException>(() => BsonValueConverter.Convert(new object()));
    }
}

public class ObjectIdConverterTests
{
    [Fact]
    public void Convert_ObjectIdToString_ReturnsHexString()
    {
        var objectId = ObjectId.NewObjectId();

        var result = ObjectIdConverter.Convert(objectId);

        Assert.Equal(objectId.ToString(), result);
    }

    [Fact]
    public void Convert_StringToObjectId_ReturnsObjectId()
    {
        var objectId = ObjectId.NewObjectId();
        var str = objectId.ToString();

        var result = ObjectIdConverter.Convert(str);

        Assert.Equal(objectId, result);
    }

    [Fact]
    public void Convert_NullObjectId_ReturnsDefault()
    {
        ObjectId nullId = default;

        var result = ObjectIdConverter.Convert(nullId);

        Assert.Equal(default(string), result);
    }

    [Fact]
    public void Convert_NullString_ReturnsDefault()
    {
        string nullStr = null!;

        var result = ObjectIdConverter.Convert(nullStr);

        Assert.Equal(default(ObjectId), result);
    }
}
