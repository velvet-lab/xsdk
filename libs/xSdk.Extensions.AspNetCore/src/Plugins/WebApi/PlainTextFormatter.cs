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
using Microsoft.AspNetCore.Mvc.Formatters;

namespace xSdk.Plugins.WebApi;

[ExcludeFromCodeCoverage(Justification = "MVC TextInputFormatter – requires a running web host with MVC pipeline.")]
internal class PlainTextFormatter : TextInputFormatter
{
    public PlainTextFormatter()
    {
        SupportedMediaTypes.Add("text/plain");
    }

    protected override bool CanReadType(Type type)
    {
        return type == typeof(string);
    }

    public override async Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
    {
        string data = await ReadInternalAsync(context);

        return InputFormatterResult.Success(data);
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
    {
        string data = await ReadInternalAsync(context);

        return InputFormatterResult.Success(data);
    }

    private async Task<string> ReadInternalAsync(InputFormatterContext context)
    {
        string? data = null;
        using (var streamReader = new StreamReader(context.HttpContext.Request.Body))
        {
            data = await streamReader.ReadToEndAsync();
        }

        return data;
    }
}
