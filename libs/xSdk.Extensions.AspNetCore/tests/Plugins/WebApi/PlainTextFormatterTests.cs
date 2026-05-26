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

using Microsoft.AspNetCore.Mvc.Formatters;
using xSdk.Extensions.WebApi;

namespace xSdk.Plugins.WebApi;

public class PlainTextFormatterTests
{
    [Fact]
    public void Constructor_SupportedMediaTypes_ContainsTextPlain()
    {
        var formatter = new PlainTextFormatter();

        Assert.Contains("text/plain", formatter.SupportedMediaTypes);
    }

    [Fact]
    public void CanReadType_String_ReturnsTrue()
    {
        var formatter = new PlainTextFormatter();

        var result = formatter.CanRead(CreateContext(typeof(string)));

        Assert.True(result);
    }

    [Fact]
    public void CanReadType_Int_ReturnsFalse()
    {
        var formatter = new PlainTextFormatter();

        // Can only read string types
        Assert.False(formatter.CanRead(CreateContext(typeof(int))));
    }

    private static InputFormatterContext CreateContext(Type modelType)
    {
        var httpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
        httpContext.Request.ContentType = "text/plain";

        var modelMetadataProvider = new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider();
        var modelMetadata = modelMetadataProvider.GetMetadataForType(modelType);

        return new InputFormatterContext(
            httpContext,
            "model",
            new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary(),
            modelMetadata,
            (stream, encoding) => new System.IO.StreamReader(stream, encoding)
        );
    }
}
