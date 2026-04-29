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

using System.Diagnostics.CodeAnalysis;
using System.Text;
using CloudNative.CloudEvents;
using CloudNative.CloudEvents.AspNetCore;
using CloudNative.CloudEvents.Core;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace xSdk.Extensions.CloudEvents;

/// <summary>
/// A <see cref="TextInputFormatter"/> that parses HTTP requests into CloudEvents.
/// </summary>
[ExcludeFromCodeCoverage(Justification = "MVC TextInputFormatter – requires a running ASP.NET Core MVC pipeline.")]
public class CloudEventJsonInputFormatter : TextInputFormatter
{
    private readonly CloudEventFormatter _formatter;

    /// <summary>
    /// Constructs a new instance that uses the given formatter for deserialization.
    /// </summary>
    /// <param name="formatter"></param>
    public CloudEventJsonInputFormatter(CloudEventFormatter formatter)
    {
        _formatter = Validation.CheckNotNull(formatter, nameof(formatter));
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/cloudevents+json"));

        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    /// <inheritdoc />
    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
    {
        Validation.CheckNotNull(context, nameof(context));
        Validation.CheckNotNull(encoding, nameof(encoding));

        var request = context.HttpContext.Request;

        try
        {
            var cloudEvent = await request.ToCloudEventAsync(_formatter);
            return await InputFormatterResult.SuccessAsync(cloudEvent);
        }
        catch (Exception)
        {
            return await InputFormatterResult.FailureAsync();
        }
    }

    /// <inheritdoc />
    protected override bool CanReadType(Type type) => type == typeof(CloudEvent) && base.CanReadType(type);
}
