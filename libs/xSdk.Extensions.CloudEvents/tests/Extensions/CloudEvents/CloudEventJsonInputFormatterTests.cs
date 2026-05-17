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

using CloudNative.CloudEvents.SystemTextJson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace xSdk.Extensions.CloudEvents;

public class CloudEventJsonInputFormatterTests
{
    [Fact]
    public void Constructor_WithValidFormatter_DoesNotThrow()
    {
        JsonEventFormatter formatter = CloudEventFactory.CreateFormatter();

        Exception? ex = Record.Exception(() => new CloudEventJsonInputFormatter(formatter));

        Assert.Null(ex);
    }

    [Fact]
    public void Constructor_WithNullFormatter_ThrowsArgumentNullException() =>
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<ArgumentNullException>(() => new CloudEventJsonInputFormatter(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

    [Fact]
    public void SupportedMediaTypes_ContainsJson()
    {
        JsonEventFormatter formatter = CloudEventFactory.CreateFormatter();
        var inputFormatter = new CloudEventJsonInputFormatter(formatter);

        MediaTypeCollection types = inputFormatter.SupportedMediaTypes;

        Assert.Contains(types, t => t.ToString().Contains("application/json"));
    }

    [Fact]
    public void SupportedMediaTypes_ContainsCloudEventsJson()
    {
        JsonEventFormatter formatter = CloudEventFactory.CreateFormatter();
        var inputFormatter = new CloudEventJsonInputFormatter(formatter);

        MediaTypeCollection types = inputFormatter.SupportedMediaTypes;

        Assert.Contains(types, t => t.ToString().Contains("cloudevents"));
    }

    [Fact]
    public void ConfigureMvc_AddsFormatterToOptions()
    {
        JsonEventFormatter formatter = CloudEventFactory.CreateFormatter();
        var options = new MvcOptions();

        options.InputFormatters.Insert(0, new CloudEventJsonInputFormatter(formatter));

        Assert.NotEmpty(options.InputFormatters);
    }
}
