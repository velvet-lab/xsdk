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

namespace xSdk.Extensions.Web;

public class ContentTypesTests
{
    [Fact]
    public void None_IsNoneString()
    {
        Assert.Equal("None", ContentTypes.None);
    }

    [Fact]
    public void ApplicationJson_IsCorrectMimeType()
    {
        Assert.Equal("application/json", ContentTypes.ApplicationJson);
    }

    [Fact]
    public void ApplicationProblemJson_IsCorrectMimeType()
    {
        Assert.Equal("application/problem+json", ContentTypes.ApplicationProblemJson);
    }

    [Fact]
    public void ApplicationXml_IsCorrectMimeType()
    {
        Assert.Equal("application/xml", ContentTypes.ApplicationXml);
    }

    [Fact]
    public void TextHtml_IsCorrectMimeType()
    {
        Assert.Equal("text/html", ContentTypes.TextHtml);
    }

    [Fact]
    public void TextPlain_IsCorrectMimeType()
    {
        Assert.Equal("text/plain", ContentTypes.TextPlain);
    }

    [Fact]
    public void ImagePng_IsCorrectMimeType()
    {
        Assert.Equal("image/png", ContentTypes.ImagePng);
    }

    [Fact]
    public void ImageJpeg_IsCorrectMimeType()
    {
        Assert.Equal("image/jpeg", ContentTypes.ImageJpeg);
    }
}
