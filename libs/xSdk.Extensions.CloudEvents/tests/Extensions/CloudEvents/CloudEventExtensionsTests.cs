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

using CloudNative.CloudEvents;
using xSdk.Hosting;

namespace xSdk.Extensions.CloudEvents;

public class CloudEventExtensionsTests()
{
    [Fact]
    public void AddAttribute_AddsAttributeToCloudEvent()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");
        CloudEventFactory.CreateAttribute("customattr", CloudEventAttributeType.String);

        cloudEvent.AddAttribute("customattr", CloudEventAttributeType.String, "test-value");

        string? value = cloudEvent.GetAttributeValue<string>("customattr");
        Assert.Equal("test-value", value);
    }

    [Fact]
    public void RemoveAttribute_RemovesExistingAttribute()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");
        cloudEvent.AddAttribute("removeattr", CloudEventAttributeType.String, "temp");

        cloudEvent.RemoveAttribute("removeattr");

        string? value = cloudEvent.GetAttributeValue<string>("removeattr");
        Assert.Null(value);
    }

    [Fact]
    public void RemoveAttribute_NonExistentAttribute_DoesNotThrow()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        Exception? ex = Record.Exception(() => cloudEvent.RemoveAttribute("nonexistent"));

        Assert.Null(ex);
    }

    [Fact]
    public void GetAttributeValue_ReturnsDefaultWhenMissing()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        string? value = cloudEvent.GetAttributeValue<string>("missing");

        Assert.Null(value);
    }

    [Fact]
    public void GetScope_ReturnsExpectedScope()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        string scope = cloudEvent.GetScope();

        Assert.Contains("test", scope);
    }

    [Fact]
    public void SetDataObject_WithStringData_SetsDataAndSchema()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        cloudEvent.SetDataObject("some string data");

        Assert.NotNull(cloudEvent.Data);
        Assert.NotNull(cloudEvent.DataSchema);
    }

    [Fact]
    public void SetDataObject_WithNull_DoesNotSetData()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        cloudEvent.SetDataObject(null!);

        Assert.Null(cloudEvent.Data);
    }

    [Fact]
    public void SetDataObject_WithException_SetsExceptionMessage()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");
        var exception = new InvalidOperationException("Test exception");

        cloudEvent.SetDataObject(exception);

        Assert.Equal("Test exception", cloudEvent.Data);
    }
}
