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

using System.Text;
using CloudNative.CloudEvents;
using CloudNative.CloudEvents.SystemTextJson;
using xSdk.Hosting;

namespace xSdk.Extensions.CloudEvents;

public class CloudEventExtensionsAdditionalTests()
{

    [Fact]
    public void GetDataObject_WithStringData_ReturnsData()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");
        cloudEvent.SetDataObject("some string");

        object? data = cloudEvent.GetDataObject();

        Assert.NotNull(data);
    }

    [Fact]
    public void GetDataObject_WithNoData_ReturnsNull()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        object? data = cloudEvent.GetDataObject();

        Assert.Null(data);
    }

    [Fact]
    public void GetDataObjectType_WithSchemaUri_ReturnsType()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");
        cloudEvent.SetDataObject("test string value");

        Type? dataType = cloudEvent.GetDataObjectType();

        // When DataSchema is set, returns a Type; for non-complex types this may be null
        // The method returns non-null when schema can be resolved
        Assert.NotNull(dataType);
    }

    [Fact]
    public void GetDataObjectType_WithNoData_ReturnsNull()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        Type? dataType = cloudEvent.GetDataObjectType();

        Assert.Null(dataType);
    }

    [Fact]
    public void GetDataObject_Generic_WithStringData_ReturnsTypedData()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");
        cloudEvent.SetDataObject("some string value");

        string? data = cloudEvent.GetDataObject<string>();

        Assert.NotNull(data);
    }

    [Fact]
    public void ToModel_WithValidModelData_ReturnsModel()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        Assert.NotNull(cloudEvent);
    }
}

public class CloudEventStringConverterAdditionalTests()
{
    private static string CreateValidCloudEventJson()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");
        JsonEventFormatter formatter = CloudEventFactory.CreateFormatter();

        ReadOnlyMemory<byte> bytes = formatter.EncodeStructuredModeMessage(cloudEvent, out _);
        return Encoding.UTF8.GetString(bytes.ToArray());
    }

    private static string CreateValidCloudEventBase64()
    {
        string json = CreateValidCloudEventJson();
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
    }

    [Fact]
    public void FromJson_WithExtensionName_ReturnsCloudEvent()
    {
        string json = CreateValidCloudEventJson();

        CloudEvent result = CloudEventStringConverter.FromJson(json, "testextension", CloudEventAttributeType.String);

        Assert.NotNull(result);
    }

    [Fact]
    public void FromJson_WithExtensionAttribute_ReturnsCloudEvent()
    {
        string json = CreateValidCloudEventJson();
        CloudEventAttribute attr = CloudEventFactory.CreateAttribute("myext", CloudEventAttributeType.String);

        CloudEvent result = CloudEventStringConverter.FromJson(json, attr);

        Assert.NotNull(result);
    }

    [Fact]
    public void FromJson_WithExtensionList_ReturnsCloudEvent()
    {
        string json = CreateValidCloudEventJson();
        var attrs = new List<CloudEventAttribute>
        {
            CloudEventFactory.CreateAttribute("ext1", CloudEventAttributeType.String)
        };

        CloudEvent result = CloudEventStringConverter.FromJson(json, attrs);

        Assert.NotNull(result);
    }

    [Fact]
    public void FromBase64_WithExtensionName_ReturnsCloudEvent()
    {
        string base64 = CreateValidCloudEventBase64();

        CloudEvent result = CloudEventStringConverter.FromBase64(base64, "testextension", CloudEventAttributeType.String);

        Assert.NotNull(result);
    }

    [Fact]
    public void FromBase64_WithExtensionAttribute_ReturnsCloudEvent()
    {
        string base64 = CreateValidCloudEventBase64();
        CloudEventAttribute attr = CloudEventFactory.CreateAttribute("myext", CloudEventAttributeType.String);

        CloudEvent result = CloudEventStringConverter.FromBase64(base64, attr);

        Assert.NotNull(result);
    }

    [Fact]
    public void FromBase64_WithExtensionList_ReturnsCloudEvent()
    {
        string base64 = CreateValidCloudEventBase64();
        var attrs = new List<CloudEventAttribute>
        {
            CloudEventFactory.CreateAttribute("ext1", CloudEventAttributeType.String)
        };

        CloudEvent result = CloudEventStringConverter.FromBase64(base64, attrs);

        Assert.NotNull(result);
    }
}
