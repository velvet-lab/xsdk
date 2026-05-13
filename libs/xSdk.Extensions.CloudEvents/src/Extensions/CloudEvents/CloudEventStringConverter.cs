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
using xSdk.Tools;

namespace xSdk.Extensions.CloudEvents;

public static class CloudEventStringConverter
{
    public static CloudEvent FromBase64(string raw) => FromBase64Internal(raw, null);

    public static CloudEvent FromBase64(string raw, string extensionName, CloudEventAttributeType extensionType)
    {
        CloudEventAttribute attr = CloudEventFactory.CreateAttribute(extensionName, extensionType);
        return FromBase64(raw, attr);
    }

    public static CloudEvent FromBase64(string raw, CloudEventAttribute extension) => FromBase64Internal(raw, [extension]);

    public static CloudEvent FromBase64(string raw, IEnumerable<CloudEventAttribute> extensions) => FromBase64Internal(raw, extensions);

    private static CloudEvent FromBase64Internal(string raw, IEnumerable<CloudEventAttribute>? extensions)
    {
        if (Base64Tools.IsBase64(raw))
        {
            string? cloudEventAsJson = Base64Tools.ConvertFromBase64(raw);
            if (!string.IsNullOrEmpty(cloudEventAsJson))
            {
                CloudEvent cloudEvent = FromJson(cloudEventAsJson, extensions);
                return cloudEvent;
            }
            else
            {
                throw new SdkException("Given Base64 String could not be decoded to a valid Json String");
            }
        }
        else
        {
            throw new SdkException("Given String is not a Base64 String");
        }
    }

    public static CloudEvent FromJson(string json) => FromJsonInternal(json, null);

    public static CloudEvent FromJson(string json, string extensionName, CloudEventAttributeType extensionType)
    {
        CloudEventAttribute attr = CloudEventFactory.CreateAttribute(extensionName, extensionType);
        return FromJson(json, attr);
    }

    public static CloudEvent FromJson(string json, CloudEventAttribute extension) => FromJsonInternal(json, [extension]);

    public static CloudEvent FromJson(string json, IEnumerable<CloudEventAttribute>? extensions) => FromJsonInternal(json, extensions);

    private static CloudEvent FromJsonInternal(string json, IEnumerable<CloudEventAttribute>? extensions)
    {
        if (JsonTools.IsJson(json))
        {
            byte[] body = Encoding.UTF8.GetBytes(json);
            JsonEventFormatter formatter = CloudEventFactory.CreateFormatter();

            CloudEventFactory.MergeAttributes(extensions);
            return formatter.DecodeStructuredModeMessage(body, null, extensions);
        }
        else
        {
            throw new SdkException("Given String is not a Json String");
        }
    }
}
