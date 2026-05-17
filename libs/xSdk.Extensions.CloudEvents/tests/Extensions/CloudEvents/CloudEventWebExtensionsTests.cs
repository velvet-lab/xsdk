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

public class CloudEventWebExtensionsTests()
{
    [Fact]
    public void ToJson_WithValidCloudEvent_ReturnsJsonString()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        string json = cloudEvent.ToJson();

        Assert.NotNull(json);
        Assert.NotEmpty(json);
        Assert.StartsWith("{", json.TrimStart());
    }

    [Fact]
    public void ToBase64_WithValidCloudEvent_ReturnsBase64String()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        string? base64 = cloudEvent.ToBase64();

        Assert.NotNull(base64);
        Assert.NotEmpty(base64);
        Assert.True(IsBase64(base64));
    }

    [Fact]
    public void ToHttpContent_WithValidCloudEvent_ReturnsBytesAndContentType()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        (ReadOnlyMemory<byte> body, System.Net.Mime.ContentType? contentType) = cloudEvent.ToHttpContent();

        Assert.False(body.IsEmpty);
        Assert.NotNull(contentType);
    }

    [Fact]
    public void ToHttpContent_WithJsonSerializerOptions_ReturnsBytesAndContentType()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        (ReadOnlyMemory<byte> body, System.Net.Mime.ContentType? contentType) = cloudEvent.ToHttpContent(options);

        Assert.False(body.IsEmpty);
        Assert.NotNull(contentType);
    }

    [Fact]
    public void ToHttpContent_WithJsonSerializerOptionsAndDocument_ReturnsBytesAndContentType()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");
        var options = new JsonSerializerOptions();
        var documentOptions = new JsonDocumentOptions();

        (ReadOnlyMemory<byte> body, System.Net.Mime.ContentType? contentType) = cloudEvent.ToHttpContent(options, documentOptions);

        Assert.False(body.IsEmpty);
        Assert.NotNull(contentType);
    }

    [Fact]
    public void ToJson_RoundTrip_CanBeConvertedBack()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        string json = cloudEvent.ToJson();
        CloudEvent restored = CloudEventStringConverter.FromJson(json);

        Assert.NotNull(restored);
        Assert.Equal(cloudEvent.Type, restored.Type);
    }

    [Fact]
    public void ToBase64_RoundTrip_CanBeConvertedBack()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");

        string? base64 = cloudEvent.ToBase64();
        if (!string.IsNullOrEmpty(base64))
        {
            CloudEvent restored = CloudEventStringConverter.FromBase64(base64);

            Assert.NotNull(restored);
            Assert.Equal(cloudEvent.Type, restored.Type);
        }
        else
        {
            Assert.NotNull(base64);
        }
    }

    [Fact]
    public void ToJson_WithDataPayload_JsonContainsData()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");
        cloudEvent.SetDataObject(new { Name = "TestPayload" });

        string json = cloudEvent.ToJson();

        Assert.NotNull(json);
        Assert.Contains("TestPayload", json);
    }

    private static bool IsBase64(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        try
        {
            byte[] decoded = Convert.FromBase64String(value);
            return decoded.Length > 0;
        }
        catch
        {
            return false;
        }
    }
}
