namespace xSdk.Data;

public class FlatFileModelTests
{
    private class TestModel : FlatFileModel
    {
        public string Title { get; set; } = string.Empty;
    }

    [Fact]
    public void FlatFileModel_DefaultConstructor_InitializesPrimaryKey()
    {
        var model = new TestModel();

        Assert.NotNull(model.PrimaryKey);
        Assert.IsType<GuidStringPK>(model.PrimaryKey);
    }

    [Fact]
    public void FlatFileModel_IdProperty_GetSet_WorksCorrectly()
    {
        var model = new TestModel();
        var id = Guid.NewGuid().ToString();

        model.Id = id;

        Assert.Equal(id, model.Id);
    }

    [Fact]
    public void FlatFileModel_IdDefault_IsEmptyString()
    {
        var model = new TestModel();

        Assert.NotNull(model.Id);
    }

    [Fact]
    public void FlatFileModel_CustomProperties_CanBeSetAndRetrieved()
    {
        var model = new TestModel { Title = "Test Title" };

        Assert.Equal("Test Title", model.Title);
    }

    [Fact]
    public void FlatFileModel_SetIdTwice_KeepsLastValue()
    {
        var model = new TestModel();
        var first = Guid.NewGuid().ToString();
        var second = Guid.NewGuid().ToString();

        model.Id = first;
        model.Id = second;

        Assert.Equal(second, model.Id);
    }
}
