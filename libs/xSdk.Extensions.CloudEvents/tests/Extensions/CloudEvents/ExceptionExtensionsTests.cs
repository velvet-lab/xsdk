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

using System.Text.Json;
using CloudNative.CloudEvents;

namespace xSdk.Extensions.CloudEvents;

public class ExceptionExtensionsTests()
{
    [Fact]
    public void ToCloudEvent_WithException_CreatesCloudEventWithScope()
    {
        var exception = new InvalidOperationException("Test error message");
        string scope = "test/scope";

        var cloudEvent = exception.ToCloudEvent(scope);

        Assert.NotNull(cloudEvent);
        Assert.Contains("error", cloudEvent.Type);
    }

    [Fact]
    public void ToCloudEvent_WithScopeAndType_CreatesCloudEventWithBothValues()
    {
        var exception = new ArgumentException("Invalid argument");
        string scope = "test/scope";
        string type = "argument.validation.error";

        var cloudEvent = exception.ToCloudEvent(scope, type);

        Assert.NotNull(cloudEvent);
        Assert.Contains(type, cloudEvent.Type);
    }

    [Fact]
    public void ToCloudEvent_WithScopeTypeAndSubject_CreatesCompleteCloudEvent()
    {
        var exception = new Exception("General error");
        string scope = "test/scope";
        string type = "general.error";
        string subject = "user/123";

        var cloudEvent = exception.ToCloudEvent(scope, type, subject);

        Assert.NotNull(cloudEvent);
        Assert.Contains(type, cloudEvent.Type);
        Assert.Equal(subject, cloudEvent.Subject);
    }

    [Fact]
    public void ToCloudEvent_WithNullType_DefaultsToError()
    {
        var exception = new Exception("Test error");
        string scope = "test/scope";

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var cloudEvent = exception.ToCloudEvent(scope, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        Assert.NotNull(cloudEvent);
        Assert.Contains("error", cloudEvent.Type);
    }

    [Fact]
    public void ToCloudEvent_WithEmptyType_DefaultsToError()
    {
        var exception = new Exception("Test error");
        string scope = "test/scope";

        var cloudEvent = exception.ToCloudEvent(scope, string.Empty);

        Assert.NotNull(cloudEvent);
        Assert.Contains("error", cloudEvent.Type);
    }

    [Fact]
    public void ToCloudEvent_PreservesExceptionMessage()
    {
        string errorMessage = "This is a test error message";
        var exception = new InvalidOperationException(errorMessage);

        var cloudEvent = exception.ToCloudEvent("test/scope");

        Assert.NotNull(cloudEvent);
        Assert.NotNull(cloudEvent.Data);
    }

    [Fact]
    public void IsException_WithExceptionData_ReturnsTrue()
    {
        var exception = new InvalidOperationException("Test");
        var cloudEvent = exception.ToCloudEvent("test/scope");

        bool result = cloudEvent.IsException();

        Assert.True(result);
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

        InvalidOperationException? exception = cloudEvent.ToException<InvalidOperationException>();

        Assert.NotNull(exception);
        Assert.Equal("Error message from CloudEvent", exception.Message);
    }

    [Fact]
    public void ToException_WithJsonElementStringData_ReturnsExceptionWithMessage()
    {
        string errorMessage = "JSON error message";
        JsonElement jsonElement = JsonSerializer.SerializeToElement(errorMessage);
        var cloudEvent = new CloudEvent
        {
            Type = "test.error",
            Source = new Uri("https://test.com"),
            Data = jsonElement
        };

        var exception = cloudEvent.ToException<Exception>();

        Assert.NotNull(exception);
        Assert.Equal(errorMessage, exception.Message);
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

        Assert.Null(exception);
    }

    [Fact]
    public void ToException_WithNullCloudEvent_ReturnsNull()
    {
        CloudEvent? cloudEvent = null;

        var exception = cloudEvent?.ToException<Exception>();

        Assert.Null(exception);
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

        ArgumentException? exception = cloudEvent.ToException<ArgumentException>();

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("Argument error message", exception.Message);
    }

    [Fact]
    public void RoundTrip_ExceptionToCloudEventToException_PreservesMessage()
    {
        string originalMessage = "Original error message";
        var originalException = new InvalidOperationException(originalMessage);

        var cloudEvent = originalException.ToCloudEvent("test/scope");
        InvalidOperationException? reconstructedException = cloudEvent.ToException<InvalidOperationException>();

        Assert.NotNull(reconstructedException);
        Assert.Equal(originalMessage, reconstructedException.Message);
    }

    [Fact]
    public void ToCloudEvent_WithCustomExceptionType_CreatesCloudEvent()
    {
        var exception = new ArgumentNullException("paramName", "Parameter cannot be null");

        var cloudEvent = exception.ToCloudEvent("test/scope", "validation.error");

        Assert.NotNull(cloudEvent);
        Assert.Contains("validation.error", cloudEvent.Type);
    }
}
