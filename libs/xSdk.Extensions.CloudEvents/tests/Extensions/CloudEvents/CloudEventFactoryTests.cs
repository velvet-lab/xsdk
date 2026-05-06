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
using xSdk.Hosting;

namespace xSdk.Extensions.CloudEvents.Tests.Extensions.CloudEvents;

public class CloudEventFactoryTests()
{
    [Fact]
    public void CreateCloudEvent_WithScopeAndType_ReturnsValidCloudEvent()
    {
        var scope = "test/models";
        var type = "model.created";

        var cloudEvent = CloudEventFactory.CreateCloudEvent(scope, type);

        Assert.NotNull(cloudEvent);
        Assert.NotNull(cloudEvent.Type);
        Assert.Contains(type, cloudEvent.Type);
    }

    [Fact]
    public void CreateCloudEvent_WithSubject_SetsSubject()
    {
        var scope = "test/events";
        var type = "event.created";
        var subject = "mysubject";

        var cloudEvent = CloudEventFactory.CreateCloudEvent(scope, type, subject);

        Assert.NotNull(cloudEvent);
        Assert.Equal(subject, cloudEvent.Subject);
    }

    [Fact]
    public void CreateCloudEvent_WithPayload_SetsData()
    {
        var scope = "test/models";
        var type = "model.created";
        var payload = new { Name = "Test", Value = 42 };

        var cloudEvent = CloudEventFactory.CreateCloudEvent(scope, type, payload);

        Assert.NotNull(cloudEvent);
        Assert.NotNull(cloudEvent.Data);
    }

    [Fact]
    public void CreateCloudEvent_WithPayload_SetsDataContentType()
    {
        var scope = "test/models";
        var type = "model.updated";
        var payload = new { Name = "Test" };

        var cloudEvent = CloudEventFactory.CreateCloudEvent(scope, type, payload);

        Assert.NotNull(cloudEvent);
        Assert.Equal("application/json", cloudEvent.DataContentType);
    }

    [Fact]
    public void CreateCloudEvent_WithoutPayload_DataContentTypeIsNull()
    {
        var scope = "test/models";
        var type = "model.deleted";

        var cloudEvent = CloudEventFactory.CreateCloudEvent(scope, type);

        Assert.Null(cloudEvent.DataContentType);
    }

    [Fact]
    public void CreateCloudEvent_HasGeneratedId()
    {
        var cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "model.created");

        Assert.NotNull(cloudEvent.Id);
        Assert.NotEmpty(cloudEvent.Id);
    }

    [Fact]
    public void CreateCloudEvent_HasTimestamp()
    {
        var cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "model.created");

        Assert.NotNull(cloudEvent.Time);
    }

    [Theory]
    [InlineData("test/scope")]
    [InlineData("/test/scope")]
    [InlineData("test.scope")]
    public void CreateBaseUrls_WithVariousScopes_ReturnsUrls(string scope)
    {
        var normalizedScope = scope.TrimStart('/').Replace(".", "/");

        var (sourceUrl, schemeUrl) = CloudEventFactory.CreateBaseUrls(normalizedScope);

        Assert.NotNull(sourceUrl);
        Assert.NotNull(schemeUrl);
        Assert.NotEmpty(sourceUrl);
        Assert.NotEmpty(schemeUrl);
    }

    [Fact]
    public void CreateBaseUrls_WithEmptyScope_ThrowsSdkException()
    {
        Assert.Throws<SdkException>(() => CloudEventFactory.CreateBaseUrls(string.Empty));
    }

    [Fact]
    public void CreateAttribute_WithValidName_ReturnsAttribute()
    {
        var attr = CloudEventFactory.CreateAttribute("myattr", CloudEventAttributeType.String);

        Assert.NotNull(attr);
        Assert.Equal("myattr", attr.Name);
    }

    [Fact]
    public void CreateAttribute_WithSpecialChars_CleanesName()
    {
        var attr = CloudEventFactory.CreateAttribute("my-attr!", CloudEventAttributeType.String);

        Assert.NotNull(attr);
        Assert.DoesNotContain("-", attr.Name);
        Assert.DoesNotContain("!", attr.Name);
    }

    [Fact]
    public void CreateAttribute_WithLongName_TruncatesToTwentyChars()
    {
        var attr = CloudEventFactory.CreateAttribute("averylongnamethatexceedstwentycharacters", CloudEventAttributeType.String);

        Assert.NotNull(attr);
        Assert.True(attr.Name.Length <= 20);
    }

    [Fact]
    public void CreateRawCloudEvent_WithValidArgs_ReturnsCloudEvent()
    {
        var sourceBaseUrl = "https://test.de/events/spec/v1/test/scope";
        var scope = "test/scope";
        var type = "model.created";

        var cloudEvent = CloudEventFactory.CreateRawCloudEvent(sourceBaseUrl, scope, type, null, true, null);

        Assert.NotNull(cloudEvent);
        Assert.Contains(type, cloudEvent.Type);
    }

    [Fact]
    public void CreateFormatter_ReturnsJsonFormatter()
    {
        var formatter = CloudEventFactory.CreateFormatter();

        Assert.NotNull(formatter);
    }
}
