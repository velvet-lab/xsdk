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
using CloudNative.CloudEvents.SystemTextJson;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Logging;
using xSdk.Extensions.Web;
using xSdk.Tools;

namespace xSdk.Extensions.CloudEvents;

public static class CloudEventFactory
{
    private static ILogger? _logger;
    private static ILogger Logger => _logger ??= LogManager.CreateLogger(typeof(CloudEventFactory));

    internal const string BASE_URL = $"https://xsdk.io";
    internal const string SOURCE_BASE_URL = $"{BASE_URL}/events/spec/v1";
    internal const string SCHEME_BASE_URL = $"{BASE_URL}/schemes/v1";

    public static CloudEvent CreateCloudEvent(string scope, string type)
        => CreateCloudEvent(scope, type, null, null, null);

    public static CloudEvent CreateCloudEvent(string scope, string type, IEnumerable<CloudEventAttribute> extensions)
        => CreateCloudEvent(scope, type, null, null, extensions);

    public static CloudEvent CreateCloudEvent(string scope, string type, string subject)
        => CreateCloudEvent(scope, type, subject, null, null);

    public static CloudEvent CreateCloudEvent(string scope, string type, string subject, IEnumerable<CloudEventAttribute> extensions)
        => CreateCloudEvent(scope, type, subject, null, extensions);

    public static CloudEvent CreateCloudEvent(string scope, string type, object payload)
        => CreateCloudEvent(scope, type, null, payload, null);

    public static CloudEvent CreateCloudEvent(string scope, string type, object payload, IEnumerable<CloudEventAttribute> extensions)
        => CreateCloudEvent(scope, type, null, payload, extensions);

    public static CloudEvent CreateCloudEvent(string scope, string type, string subject, object payload)
        => CreateCloudEvent(scope, type, subject, payload, null);

    public static CloudEvent CreateCloudEvent(string scope, string type, string? subject, object? payload, IEnumerable<CloudEventAttribute>? extensions)
    {
        (string? sourceBaseUrl, string? _) = CreateBaseUrls(scope);

        CloudEvent cloudEvent;
        if (payload == null)
        {
            // Event without a Data Object
            cloudEvent = CreateRawCloudEvent(sourceBaseUrl, scope, type, subject, default, extensions);
        }
        else
        {
            // Event with Data Object
            cloudEvent = CreateRawCloudEvent(sourceBaseUrl, scope, type, subject, ContentTypes.ApplicationJson, extensions);
            cloudEvent.SetDataObject(payload);
        }

        return cloudEvent;
    }

    public static CloudEventAttribute CreateAttribute(string name, CloudEventAttributeType type)
    {
        name = name.ToLower();
        name = StringTools.RemoveSpecialChars(name);

        if (name.Length > 20)
        {
            name = name.Substring(0, 20);
        }

        var attr = CloudEventAttribute.CreateExtension(name, type);
        return attr;
    }

    public static (string, string) CreateBaseUrls(string scope)
    {
        if (string.IsNullOrEmpty(scope))
        {
            throw new SdkException("A Scope is needed for CloudEvent");
        }

        if (scope.StartsWith('/'))
        {
            scope = scope.Substring(1);
        }

        if (scope.IndexOf('.') > -1)
        {
            scope = scope.Replace(".", "/");
        }

        string sourceBaseUrl = $"{SOURCE_BASE_URL}/{scope}".ToLower();
        string schemeBaseUrl = $"{SCHEME_BASE_URL}/{scope}".ToLower();

        if (!scope.StartsWith("http"))
        {
            sourceBaseUrl = $"{SOURCE_BASE_URL}/{scope}".ToLower();
        }

        return (sourceBaseUrl, schemeBaseUrl);
    }

    public static CloudEvent CreateRawCloudEvent(
        string sourceBaseUrl,
        string? scope,
        string? type,
        string? subject,
        string? contentType,
        IEnumerable<CloudEventAttribute>? extensions
    )
    {
        if (string.IsNullOrEmpty(type))
        {
            throw new SdkException("A Type is needed for CloudEvent");
        }

        if (string.IsNullOrEmpty(scope))
        {
            throw new SdkException("A Scope is neededA Scope is needed for CloudEvent");
        }

        if (type.StartsWith('/'))
        {
            type = type.Substring(1);
        }

        type = type.Replace("/", ".");

        int msgLength = 16; // Convert NsqMessage.MsgIdLength
        var cloudEvent = new CloudEvent(CloudEventsSpecVersion.V1_0)
        {
            // Global Unique ID
            Id = Guid.NewGuid().ToString("N").Substring(0, msgLength),

            // When is the CloudEvent created
            Time = DateTimeOffset.Now,

            // The Event Name for the Model,
            // e.g. de.aminoo.blueprint.plan to plan a Blueprint, or
            // de.aminoo.download.gpack to download a GPack
            // otherwise when e.g. a Blueprint is planed the Event
            // returned is de.aminoo.blueprint.planed
            Type = type.ToLower(),

            // The Cateogry or Scope for the Cloudevent
            Source = new Uri(sourceBaseUrl.ToLower(), UriKind.RelativeOrAbsolute),
        };

        if (!string.IsNullOrEmpty(contentType) && contentType != ContentTypes.None)
        {
            // The Data Object Type
            cloudEvent.DataContentType = contentType;
        }

        if (!string.IsNullOrEmpty(subject))
        {
            cloudEvent.Subject = subject;
        }

        if (!cloudEvent.IsValid)
        {
            Logger.LogWarning("Cloud Event is not valid. Some Attributes missing");
        }

        // Add Default Attributes
        cloudEvent.EnrichAttributes(extensions);

        return cloudEvent;
    }

    public static JsonEventFormatter CreateFormatter() => CreateFormatter(JsonTools.GetSerializerOptions(true), JsonTools.GetDocumentOptions());

    public static JsonEventFormatter CreateFormatter(JsonSerializerOptions serializer) => CreateFormatter(serializer, JsonTools.GetDocumentOptions());

    public static JsonEventFormatter CreateFormatter(JsonSerializerOptions serializer, JsonDocumentOptions document) =>
        new(serializer, document);

    internal static IEnumerable<CloudEventAttribute> MergeAttributes(IEnumerable<CloudEventAttribute>? attributes)
    {
        attributes ??= [];

        Dictionary<CloudEventAttribute, object> defaultAttributes = LoadDefaultAttributes();
        var attributesAsList = attributes.ToList();

        defaultAttributes
            .Where(x => !attributes.Any(a => string.Compare(a.Name, x.Key.Name, true) == 0))
            .ToList()
            .ForEach(item => attributesAsList.Add(item.Key));

        return attributesAsList;
    }

    internal static bool TryGetValueForAttribute(CloudEventAttribute attribute, out object? value)
    {
        Dictionary<CloudEventAttribute, object> defaultAttributes = LoadDefaultAttributes();

        value = default;
        if (defaultAttributes.Any(x => string.Compare(x.Key.Name, attribute.Name, true) == 0))
        {
            KeyValuePair<CloudEventAttribute, object> ce = defaultAttributes.SingleOrDefault(x => string.Compare(x.Key.Name, attribute.Name, true) == 0);
            value = ce.Value;
            return true;
        }

        return false;
    }

    private static Dictionary<CloudEventAttribute, object> LoadDefaultAttributes()
    {
        return new Dictionary<CloudEventAttribute, object>()
        {
            { CreateAttribute(nameof(Environment.MachineName), CloudEventAttributeType.String), Environment.MachineName },
            { CreateAttribute(nameof(Environment.UserName), CloudEventAttributeType.String), Environment.UserName },
            { CreateAttribute(nameof(Environment.UserDomainName), CloudEventAttributeType.String), Environment.UserDomainName },
        };
    }
}
