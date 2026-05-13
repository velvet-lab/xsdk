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

using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using CloudNative.CloudEvents;
using CloudNative.CloudEvents.Core;
using CloudNative.CloudEvents.Http;
using CloudNative.CloudEvents.SystemTextJson;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Web;
using xSdk.Hosting;
using xSdk.Tools;

namespace xSdk.Extensions.CloudEvents;

public static class CloudEventWebExtensions
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public static string ToJson(this CloudEvent cloudEvent)
    {
        (ReadOnlyMemory<byte> body, ContentType _) = cloudEvent.ToHttpContent();
        string json = Encoding.UTF8.GetString(body.Span);

        return json;
    }

    public static string? ToBase64(this CloudEvent cloudEvent)
    {
        string cloudEventAsJson = cloudEvent.ToJson();
        return Base64Tools.ConvertToBase64(cloudEventAsJson);
    }

    public static (ReadOnlyMemory<byte>, ContentType contenType) ToHttpContent(this CloudEvent cloudEvent) =>
        cloudEvent.ToHttpContent(CloudEventFactory.CreateFormatter());

    public static (ReadOnlyMemory<byte>, ContentType contenType) ToHttpContent(this CloudEvent cloudEvent, JsonSerializerOptions serializer) =>
        cloudEvent.ToHttpContent(CloudEventFactory.CreateFormatter(serializer));

    public static (ReadOnlyMemory<byte>, ContentType contenType) ToHttpContent(
        this CloudEvent cloudEvent,
        JsonSerializerOptions serializer,
        JsonDocumentOptions document
    ) => cloudEvent.ToHttpContent(CloudEventFactory.CreateFormatter(serializer, document));

    private static (ReadOnlyMemory<byte>, ContentType contenType) ToHttpContent(this CloudEvent cloudEvent, JsonEventFormatter formatter)
    {
        Validation.CheckCloudEventArgument(cloudEvent, nameof(cloudEvent));

        if (formatter != null)
        {
            ReadOnlyMemory<byte> body = formatter.EncodeStructuredModeMessage(cloudEvent, out ContentType contentType);
            return (body, contentType);
        }

        return (null, null);
    }

    public static void PostToHttp(this CloudEvent cloudEvent, string url) => cloudEvent.PostToHttpAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();

    public static void PostToHttp(this CloudEvent cloudEvent, string url, IDictionary<string, string> additionalHeaders) =>
        cloudEvent.PostToHttpAsync(url, additionalHeaders).ConfigureAwait(false).GetAwaiter().GetResult();

    public static void PostToHttp(this CloudEvent cloudEvent, string url, JsonSerializerOptions serializer) =>
        cloudEvent.PostToHttpAsync(url, serializer).ConfigureAwait(false).GetAwaiter().GetResult();

    public static void PostToHttp(
        this CloudEvent cloudEvent,
        string url,
        JsonSerializerOptions serializer,
        IDictionary<string, string> additionalHeaders
    ) => cloudEvent.PostToHttpAsync(url, serializer, additionalHeaders).ConfigureAwait(false).GetAwaiter().GetResult();

    public static void PostToHttp(this CloudEvent cloudEvent, string url, JsonSerializerOptions serializer, JsonDocumentOptions document) =>
        cloudEvent.PostToHttpAsync(url, serializer, document).ConfigureAwait(false).GetAwaiter().GetResult();

    public static void PostToHttp(
        this CloudEvent cloudEvent,
        string url,
        JsonSerializerOptions serializer,
        JsonDocumentOptions document,
        IDictionary<string, string> additionalHeaders
    ) => cloudEvent.PostToHttpAsync(url, serializer, document, additionalHeaders).ConfigureAwait(false).GetAwaiter().GetResult();

    public static void PostToHttp(this CloudEvent cloudEvent, string url, ReadOnlyMemory<byte> body, ContentType contentType) =>
        cloudEvent.PostToHttpAsync(url, body, contentType).ConfigureAwait(false).GetAwaiter().GetResult();

    public static void PostToHttp(
        this CloudEvent cloudEvent,
        string url,
        ReadOnlyMemory<byte> body,
        ContentType contentType,
        IDictionary<string, string> additionalHeaders
    ) => cloudEvent.PostToHttpAsync(url, body, contentType, additionalHeaders).ConfigureAwait(false).GetAwaiter().GetResult();

    public static Task PostToHttpAsync(this CloudEvent cloudEvent, string url, CancellationToken token = default)
    {
        (ReadOnlyMemory<byte> body, ContentType? contentType) = cloudEvent.ToHttpContent();
        return cloudEvent.PostToHttpAsync(url, body, contentType, null, token);
    }

    public static Task PostToHttpAsync(
        this CloudEvent cloudEvent,
        string url,
        IDictionary<string, string>? additionalHeaders,
        CancellationToken token = default
    )
    {
        (ReadOnlyMemory<byte> body, ContentType? contentType) = cloudEvent.ToHttpContent();
        return cloudEvent.PostToHttpAsync(url, body, contentType, additionalHeaders, token);
    }

    public static Task PostToHttpAsync(this CloudEvent cloudEvent, string url, JsonSerializerOptions serializer, CancellationToken token = default)
    {
        (ReadOnlyMemory<byte> body, ContentType? contentType) = cloudEvent.ToHttpContent(serializer);
        return cloudEvent.PostToHttpAsync(url, body, contentType, null, token);
    }

    public static Task PostToHttpAsync(
        this CloudEvent cloudEvent,
        string url,
        JsonSerializerOptions serializer,
        IDictionary<string, string> additionalHeaders,
        CancellationToken token = default
    )
    {
        (ReadOnlyMemory<byte> body, ContentType? contentType) = cloudEvent.ToHttpContent(serializer);
        return cloudEvent.PostToHttpAsync(url, body, contentType, additionalHeaders, token);
    }

    public static Task PostToHttpAsync(
        this CloudEvent cloudEvent,
        string url,
        JsonSerializerOptions serializer,
        JsonDocumentOptions document,
        CancellationToken token = default
    ) => cloudEvent.PostToHttpAsync(url, serializer, document, null, token);

    public static Task PostToHttpAsync(
        this CloudEvent cloudEvent,
        string url,
        JsonSerializerOptions serializer,
        JsonDocumentOptions document,
        IDictionary<string, string>? additionalHeaders,
        CancellationToken token = default
    )
    {
        (ReadOnlyMemory<byte> body, ContentType? contentType) = cloudEvent.ToHttpContent(serializer, document);
        return cloudEvent.PostToHttpAsync(url, body, contentType, additionalHeaders, token);
    }

    public static Task PostToHttpAsync(
        this CloudEvent cloudEvent,
        string url,
        ReadOnlyMemory<byte> body,
        ContentType contentType,
        CancellationToken token = default
    ) => cloudEvent.PostToHttpAsync(url, body, contentType, null, token);

    public static async Task PostToHttpAsync(
        this CloudEvent cloudEvent,
        string url,
        ReadOnlyMemory<byte> body,
        ContentType contentType,
        IDictionary<string, string>? additionalHeaders,
        CancellationToken token = default
    )
    {
        Guard.IsNullOrEmpty(url);
        Guard.IsNotNull(contentType);

        try
        {
            _logger.LogInformation("Send CloudEvent to '{url}'", url);
            using (HttpClient client = HttpClientBuilder.CreateHttpClient(new Uri(url)))
            {
                foreach (KeyValuePair<string, string> header in additionalHeaders ?? new Dictionary<string, string>())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }

                using (var stream = new MemoryStream(body.ToArray()))
                {
                    var streamContent = new StreamContent(stream);
                    streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType.MediaType);

                    _logger.LogInformation("Add CloudEvent Attributes as Http Header");
                    foreach (KeyValuePair<CloudEventAttribute, object> item in cloudEvent.GetPopulatedAttributes())
                    {
                        CloudEventAttribute attribute = item.Key;
                        object value = item.Value;
                        string headerName = $"{HttpUtilities.HttpHeaderPrefix}{attribute.Name}";
                        string headerValue = HttpUtilities.EncodeHeaderValue(attribute.Format(value));
                        streamContent.Headers.Add(headerName, headerValue);
                    }

                    streamContent.Headers.Add(HttpUtilities.SpecVersionHttpHeader, HttpUtilities.EncodeHeaderValue(cloudEvent.SpecVersion.VersionId));

                    HttpResponseMessage response = await client.PostAsync(url, streamContent, token);
                    response.EnsureSuccessStatusCode();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CloudEvent could not posted to '{url}' (Reason: {reason})", url, ex.Message);
            throw new InvalidOperationException($"CloudEvent could not be posted to '{url}'.", ex);
        }
    }
}
