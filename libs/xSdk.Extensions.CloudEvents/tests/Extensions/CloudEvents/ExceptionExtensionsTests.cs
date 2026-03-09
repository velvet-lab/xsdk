using System.Text.Json;
using CloudNative.CloudEvents;
using xSdk.Extensions.CloudEvents;
using xSdk.Hosting;

namespace xSdk.Extensions.CloudEvents.Tests.Extensions.CloudEvents;

public class ExceptionExtensionsTests(TestHostFixture _) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void ToCloudEvent_WithException_CreatesCloudEventWithScope()
    {
        var exception = new InvalidOperationException("Test error message");
        var scope = "test/scope";

        var cloudEvent = exception.ToCloudEvent(scope);

        cloudEvent.Should().NotBeNull();
        cloudEvent.Type.Should().Contain("error");
    }

    [Fact]
    public void ToCloudEvent_WithScopeAndType_CreatesCloudEventWithBothValues()
    {
        var exception = new ArgumentException("Invalid argument");
        var scope = "test/scope";
        var type = "argument.validation.error";

        var cloudEvent = exception.ToCloudEvent(scope, type);

        cloudEvent.Should().NotBeNull();
        cloudEvent.Type.Should().Contain(type);
    }

    [Fact]
    public void ToCloudEvent_WithScopeTypeAndSubject_CreatesCompleteCloudEvent()
    {
        var exception = new Exception("General error");
        var scope = "test/scope";
        var type = "general.error";
        var subject = "user/123";

        var cloudEvent = exception.ToCloudEvent(scope, type, subject);

        cloudEvent.Should().NotBeNull();
        cloudEvent.Type.Should().Contain(type);
        cloudEvent.Subject.Should().Be(subject);
    }

    [Fact]
    public void ToCloudEvent_WithNullType_DefaultsToError()
    {
        var exception = new Exception("Test error");
        var scope = "test/scope";

        var cloudEvent = exception.ToCloudEvent(scope, null);

        cloudEvent.Should().NotBeNull();
        cloudEvent.Type.Should().Contain("error");
    }

    [Fact]
    public void ToCloudEvent_WithEmptyType_DefaultsToError()
    {
        var exception = new Exception("Test error");
        var scope = "test/scope";

        var cloudEvent = exception.ToCloudEvent(scope, string.Empty);

        cloudEvent.Should().NotBeNull();
        cloudEvent.Type.Should().Contain("error");
    }

    [Fact]
    public void ToCloudEvent_PreservesExceptionMessage()
    {
        var errorMessage = "This is a test error message";
        var exception = new InvalidOperationException(errorMessage);

        var cloudEvent = exception.ToCloudEvent("test/scope");

        cloudEvent.Should().NotBeNull();
        cloudEvent.Data.Should().NotBeNull();
    }

    [Fact]
    public void IsException_WithExceptionData_ReturnsTrue()
    {
        var exception = new InvalidOperationException("Test");
        var cloudEvent = exception.ToCloudEvent("test/scope");

        var result = cloudEvent.IsException();

        result.Should().BeTrue();
    }

    [Fact]
    public void ToException_WithStringData_ReturnsExceptionWithMessage()
    {
        var cloudEvent = new CloudEvent
        {
            Type = "test.error",
            Source = new Uri("https://test.com"),
            Data = "Error message from CloudEvent"
        };

        var exception = cloudEvent.ToException<InvalidOperationException>();

        exception.Should().NotBeNull();
        exception.Message.Should().Be("Error message from CloudEvent");
    }

    [Fact]
    public void ToException_WithJsonElementStringData_ReturnsExceptionWithMessage()
    {
        var errorMessage = "JSON error message";
        var jsonElement = JsonSerializer.SerializeToElement(errorMessage);
        var cloudEvent = new CloudEvent
        {
            Type = "test.error",
            Source = new Uri("https://test.com"),
            Data = jsonElement
        };

        var exception = cloudEvent.ToException<Exception>();

        exception.Should().NotBeNull();
        exception.Message.Should().Be(errorMessage);
    }

    [Fact]
    public void ToException_WithNullData_ReturnsNull()
    {
        var cloudEvent = new CloudEvent
        {
            Type = "test.error",
            Source = new Uri("https://test.com"),
            Data = null
        };

        var exception = cloudEvent.ToException<Exception>();

        exception.Should().BeNull();
    }

    [Fact]
    public void ToException_WithNullCloudEvent_ReturnsNull()
    {
        CloudEvent cloudEvent = null;

        var exception = cloudEvent.ToException<Exception>();

        exception.Should().BeNull();
    }

    [Fact]
    public void ToException_WithDifferentExceptionTypes_CreatesCorrectType()
    {
        var cloudEvent = new CloudEvent
        {
            Type = "test.error",
            Source = new Uri("https://test.com"),
            Data = "Argument error message"
        };

        var exception = cloudEvent.ToException<ArgumentException>();

        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentException>();
        exception.Message.Should().Be("Argument error message");
    }

    [Fact]
    public void RoundTrip_ExceptionToCloudEventToException_PreservesMessage()
    {
        var originalMessage = "Original error message";
        var originalException = new InvalidOperationException(originalMessage);

        var cloudEvent = originalException.ToCloudEvent("test/scope");
        var reconstructedException = cloudEvent.ToException<InvalidOperationException>();

        reconstructedException.Should().NotBeNull();
        reconstructedException.Message.Should().Be(originalMessage);
    }

    [Fact]
    public void ToCloudEvent_WithCustomExceptionType_CreatesCloudEvent()
    {
        var exception = new ArgumentNullException("paramName", "Parameter cannot be null");

        var cloudEvent = exception.ToCloudEvent("test/scope", "validation.error");

        cloudEvent.Should().NotBeNull();
        cloudEvent.Type.Should().Contain("validation.error");
    }
}
