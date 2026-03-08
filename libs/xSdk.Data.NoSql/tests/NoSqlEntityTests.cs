using LiteDB;
using xSdk.Data.Mocks;

namespace xSdk.Data;

public class NoSqlEntityTests
{
    [Fact]
    public void CreateNewEntity()
    {
        var entity = new TestEntity();

        Assert.NotNull(entity);
        Assert.NotNull(entity.PrimaryKey);
        Assert.Equal(entity.PrimaryKey.GetValue(), entity.Id);
        Assert.IsType<ObjectId>(entity.PrimaryKey.GetValue());
    }

    [Fact]
    public void CreateNewEntityFromExistingPrimaryKey()
    {
        var pk = ObjectId.NewObjectId();
        var entity = new TestEntity();
        entity.Id = pk;

        Assert.NotNull(entity);
        Assert.NotNull(entity.PrimaryKey);
        Assert.Equal(entity.PrimaryKey.GetValue(), entity.Id);
        Assert.IsType<ObjectId>(entity.PrimaryKey.GetValue());
    }

    [Fact]
    public void CreateNewEntityByInterface()
    {
        var pk = ObjectId.NewObjectId();
        IEntity entity = new TestEntity();
        entity.Id = pk;

        Assert.NotNull(entity);
        Assert.NotNull(entity.PrimaryKey);
        Assert.Equal(entity.PrimaryKey.GetValue(), entity.Id);
        Assert.IsType<ObjectId>(entity.PrimaryKey.GetValue());
    }
}
