using xSdk.Data.Mocks;

namespace xSdk.Data;

public class DataExtensionsTests
{
    [Fact]
    public void ToEntity_FromModel_MapsCorrectly()
    {
        var model = new TestModel { Name = "Alice", Age = 30 };

        var entity = model.ToEntity<TestMappingProfile, TestEntity>();

        Assert.NotNull(entity);
        Assert.Equal(model.Name, entity.Name);
        Assert.Equal(model.Age, entity.Age);
    }

    [Fact]
    public void ToModel_FromEntity_MapsCorrectly()
    {
        var entity = new TestEntity { Name = "Bob", Age = 25 };

        var model = entity.ToModel<TestMappingProfile, TestModel>();

        Assert.NotNull(model);
        Assert.Equal(entity.Name, model.Name);
        Assert.Equal(entity.Age, model.Age);
    }

}
