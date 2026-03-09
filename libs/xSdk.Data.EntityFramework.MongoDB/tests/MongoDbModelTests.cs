using MongoDB.Bson;
using xSdk.Data.Mocks;

namespace xSdk.Data;

public class MongoDbModelTests
{
    [Fact]
    public void CreateNewModel()
    {
        var model = new TestModel();

        Assert.NotNull(model);
        Assert.NotNull(model.PrimaryKey);
        Assert.Equal(model.PrimaryKey.GetValue(), model.Id);
        Assert.IsType<string>(model.PrimaryKey.GetValue());
    }

    [Fact]
    public void CreateNewEntityFromExistingPrimaryKey()
    {
        var pk = ObjectId.GenerateNewId().ToString();
        var model = new TestModel();
        model.Id = pk;

        Assert.NotNull(model);
        Assert.NotNull(model.PrimaryKey);
        Assert.Equal(model.PrimaryKey.GetValue(), model.Id);
        Assert.IsType<string>(model.PrimaryKey.GetValue());
    }

    [Fact]
    public void CreateNewEntityByInterface()
    {
        var pk = ObjectId.GenerateNewId().ToString();
        IModel model = new TestModel();
        model.Id = pk;

        Assert.NotNull(model);
        Assert.NotNull(model.PrimaryKey);
        Assert.Equal(model.PrimaryKey.GetValue(), model.Id);
        Assert.IsType<string>(model.PrimaryKey.GetValue());
    }
}
