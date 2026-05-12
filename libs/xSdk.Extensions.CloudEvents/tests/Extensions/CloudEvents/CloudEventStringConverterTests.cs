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
using CloudNative.CloudEvents.SystemTextJson;
using xSdk.Hosting;

namespace xSdk.Extensions.CloudEvents;

public class CloudEventStringConverterTests()
{
    [Fact]
    public void FromJson_WithValidJson_ReturnsCloudEvent()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");
        JsonEventFormatter formatter = CloudEventFactory.CreateFormatter();
        ReadOnlyMemory<byte> jsonBytes = formatter.EncodeStructuredModeMessage(cloudEvent, out _);
        string json = System.Text.Encoding.UTF8.GetString(jsonBytes.ToArray());

        CloudEvent result = CloudEventStringConverter.FromJson(json);

        Assert.NotNull(result);
        Assert.Equal(cloudEvent.Type, result.Type);
    }

    [Fact]
    public void FromJson_WithInvalidJson_ThrowsSdkException() => Assert.Throws<SdkException>(() => CloudEventStringConverter.FromJson("not a json string"));

    [Fact]
    public void FromBase64_WithNonBase64String_ThrowsSdkException() => Assert.Throws<SdkException>(() => CloudEventStringConverter.FromBase64("not-base64-@#$"));

    [Fact]
    public void FromBase64_WithValidBase64CloudEvent_ReturnsCloudEvent()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "test.event");
        JsonEventFormatter formatter = CloudEventFactory.CreateFormatter();
        ReadOnlyMemory<byte> jsonBytes = formatter.EncodeStructuredModeMessage(cloudEvent, out _);
        string json = System.Text.Encoding.UTF8.GetString(jsonBytes.ToArray());
        string base64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));

        CloudEvent result = CloudEventStringConverter.FromBase64(base64);

        Assert.NotNull(result);
    }
}
