using CloudNative.CloudEvents;
using xSdk.Extensions.CloudEvents;
using xSdk.Hosting;

namespace xSdk.Extensions.CloudEvents.Tests.Extensions.CloudEvents;

public class CloudEventStringConverterTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void FromJson_WithValidJson_ReturnsCloudEvent()
    {
        var cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");
        var formatter = CloudEventFactory.CreateFormatter();
        var jsonBytes = formatter.EncodeStructuredModeMessage(cloudEvent, out var contentType);
        var json = System.Text.Encoding.UTF8.GetString(jsonBytes.ToArray());

        var result = CloudEventStringConverter.FromJson(json);

        Assert.NotNull(result);
        Assert.Equal(cloudEvent.Type, result.Type);
    }

    [Fact]
    public void FromJson_WithInvalidJson_ThrowsSdkException()
    {
        Assert.Throws<SdkException>(() => CloudEventStringConverter.FromJson("not a json string"));
    }

    [Fact]
    public void FromBase64_WithNonBase64String_ThrowsSdkException()
    {
        Assert.Throws<SdkException>(() => CloudEventStringConverter.FromBase64("not-base64-@#$"));
    }

    [Fact]
    public void FromBase64_WithValidBase64CloudEvent_ReturnsCloudEvent()
    {
        var cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");
        var formatter = CloudEventFactory.CreateFormatter();
        var jsonBytes = formatter.EncodeStructuredModeMessage(cloudEvent, out _);
        var json = System.Text.Encoding.UTF8.GetString(jsonBytes.ToArray());
        var base64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));

        var result = CloudEventStringConverter.FromBase64(base64);

        Assert.NotNull(result);
    }
}
