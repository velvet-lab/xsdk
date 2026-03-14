using CloudNative.CloudEvents;
using xSdk.Extensions.CloudEvents;
using xSdk.Hosting;

namespace xSdk.Extensions.CloudEvents.Tests.Extensions.CloudEvents;

public class CloudEventExtensionsTests(TestHostFixture _) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void AddAttribute_AddsAttributeToCloudEvent()
    {
        var cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");
        var attr = CloudEventFactory.CreateAttribute("customattr", CloudEventAttributeType.String);

        cloudEvent.AddAttribute("customattr", CloudEventAttributeType.String, "test-value");

        var value = cloudEvent.GetAttributeValue<string>("customattr");
        Assert.Equal("test-value", value);
    }

    [Fact]
    public void RemoveAttribute_RemovesExistingAttribute()
    {
        var cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");
        cloudEvent.AddAttribute("removeattr", CloudEventAttributeType.String, "temp");

        cloudEvent.RemoveAttribute("removeattr");

        var value = cloudEvent.GetAttributeValue<string>("removeattr");
        Assert.Null(value);
    }

    [Fact]
    public void RemoveAttribute_NonExistentAttribute_DoesNotThrow()
    {
        var cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        var ex = Record.Exception(() => cloudEvent.RemoveAttribute("nonexistent"));

        Assert.Null(ex);
    }

    [Fact]
    public void GetAttributeValue_ReturnsDefaultWhenMissing()
    {
        var cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        var value = cloudEvent.GetAttributeValue<string>("missing");

        Assert.Null(value);
    }

    [Fact]
    public void GetScope_ReturnsExpectedScope()
    {
        var cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        var scope = cloudEvent.GetScope();

        Assert.Contains("test", scope);
    }

    [Fact]
    public void SetDataObject_WithStringData_SetsDataAndSchema()
    {
        var cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        cloudEvent.SetDataObject("some string data");

        Assert.NotNull(cloudEvent.Data);
        Assert.NotNull(cloudEvent.DataSchema);
    }

    [Fact]
    public void SetDataObject_WithNull_DoesNotSetData()
    {
        var cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        cloudEvent.SetDataObject(null!);

        Assert.Null(cloudEvent.Data);
    }

    [Fact]
    public void SetDataObject_WithException_SetsExceptionMessage()
    {
        var cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");
        var exception = new InvalidOperationException("Test exception");

        cloudEvent.SetDataObject(exception);

        Assert.Equal("Test exception", cloudEvent.Data);
    }
}
