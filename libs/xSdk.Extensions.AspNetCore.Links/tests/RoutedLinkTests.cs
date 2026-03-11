using xSdk.Data;
using xSdk.Extensions.Links;

namespace xSdk.Extensions.AspNetCore.Links.Tests;

public class RoutedLinkTests
{
    private class TestModel : Model
    {
        public TestModel()
        {
            PrimaryKey = new GuidStringPK();
        }

        public string Name { get; set; } = string.Empty;
        public new string Id
        {
            get => PrimaryKey.GetValue<string>();
            set => PrimaryKey.SetValue(value);
        }
    }

    [Fact]
    public void RoutedLink_Name_ReturnsConstructorValue()
    {
        var link = new RoutedLink<TestModel>("self", "GetById", null);

        Assert.Equal("self", link.Name);
    }

    [Fact]
    public void RoutedLink_MethodName_ReturnsConstructorValue()
    {
        var link = new RoutedLink<TestModel>("self", "GetById", null);

        Assert.Equal("GetById", link.MethodName);
    }

    [Fact]
    public void RoutedLink_Values_ReturnsConstructorValue()
    {
        Func<TestModel, object> values = m => new { id = m.Id };
        var link = new RoutedLink<TestModel>("self", "GetById", values);

        Assert.NotNull(link.Values);
    }

    [Fact]
    public void RoutedLink_ConcreteModel_WhenModelIsCompatibleType_ReturnsModel()
    {
        var model = new TestModel { Name = "Test" };
        var link = new RoutedLink<TestModel>("self", "GetById", null);
        link.Model = model;

        var result = link.ConcreteModel;

        Assert.NotNull(result);
        Assert.Same(model, result);
    }

    [Fact]
    public void RoutedLink_ConcreteModel_WhenModelIsNull_ReturnsDefault()
    {
        var link = new RoutedLink<TestModel>("self", "GetById", null);

        var result = link.ConcreteModel;

        Assert.Null(result);
    }
}
