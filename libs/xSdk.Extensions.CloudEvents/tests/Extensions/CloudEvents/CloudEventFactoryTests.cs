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
using xSdk.Extensions.Web;

namespace xSdk.Extensions.CloudEvents;

public class CloudEventFactoryTests()
{
    [Fact]
    public void CreateCloudEvent_WithScopeAndType_ReturnsValidCloudEvent()
    {
        string scope = "test/models";
        string type = "model.created";

        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent(scope, type);

        Assert.NotNull(cloudEvent);
        Assert.NotNull(cloudEvent.Type);
        Assert.Contains(type, cloudEvent.Type);
    }

    [Fact]
    public void CreateCloudEvent_WithSubject_SetsSubject()
    {
        string scope = "test/events";
        string type = "event.created";
        string subject = "mysubject";

        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent(scope, type, subject);

        Assert.NotNull(cloudEvent);
        Assert.Equal(subject, cloudEvent.Subject);
    }

    [Fact]
    public void CreateCloudEvent_WithPayload_SetsData()
    {
        string scope = "test/models";
        string type = "model.created";
        var payload = new { Name = "Test", Value = 42 };

        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent(scope, type, payload);

        Assert.NotNull(cloudEvent);
        Assert.NotNull(cloudEvent.Data);
    }

    [Fact]
    public void CreateCloudEvent_WithPayload_SetsDataContentType()
    {
        string scope = "test/models";
        string type = "model.updated";
        var payload = new { Name = "Test" };

        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent(scope, type, payload);

        Assert.NotNull(cloudEvent);
        Assert.Equal("application/json", cloudEvent.DataContentType);
    }

    [Fact]
    public void CreateCloudEvent_WithoutPayload_DataContentTypeIsNull()
    {
        string scope = "test/models";
        string type = "model.deleted";

        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent(scope, type);

        Assert.Null(cloudEvent.DataContentType);
    }

    [Fact]
    public void CreateCloudEvent_HasGeneratedId()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "model.created");

        Assert.NotNull(cloudEvent.Id);
        Assert.NotEmpty(cloudEvent.Id);
    }

    [Fact]
    public void CreateCloudEvent_HasTimestamp()
    {
        CloudEvent cloudEvent = CloudEventFactory.CreateCloudEvent("test/scope", "model.created");

        Assert.NotNull(cloudEvent.Time);
    }

    [Theory]
    [InlineData("test/scope")]
    [InlineData("/test/scope")]
    [InlineData("test.scope")]
    public void CreateBaseUrls_WithVariousScopes_ReturnsUrls(string scope)
    {
        string normalizedScope = scope.TrimStart('/').Replace(".", "/");

        (string? sourceUrl, string? schemeUrl) = CloudEventFactory.CreateBaseUrls(normalizedScope);

        Assert.NotNull(sourceUrl);
        Assert.NotNull(schemeUrl);
        Assert.NotEmpty(sourceUrl);
        Assert.NotEmpty(schemeUrl);
    }

    [Fact]
    public void CreateBaseUrls_WithEmptyScope_ThrowsSdkException() => Assert.Throws<SdkException>(() => CloudEventFactory.CreateBaseUrls(string.Empty));

    [Fact]
    public void CreateAttribute_WithValidName_ReturnsAttribute()
    {
        CloudEventAttribute attr = CloudEventFactory.CreateAttribute("myattr", CloudEventAttributeType.String);

        Assert.NotNull(attr);
        Assert.Equal("myattr", attr.Name);
    }

    [Fact]
    public void CreateAttribute_WithSpecialChars_CleanesName()
    {
        CloudEventAttribute attr = CloudEventFactory.CreateAttribute("my-attr!", CloudEventAttributeType.String);

        Assert.NotNull(attr);
        Assert.DoesNotContain("-", attr.Name);
        Assert.DoesNotContain("!", attr.Name);
    }

    [Fact]
    public void CreateAttribute_WithLongName_TruncatesToTwentyChars()
    {
        CloudEventAttribute attr = CloudEventFactory.CreateAttribute("averylongnamethatexceedstwentycharacters", CloudEventAttributeType.String);

        Assert.NotNull(attr);
        Assert.True(attr.Name.Length <= 20);
    }

    [Fact]
    public void CreateRawCloudEvent_WithValidArgs_ReturnsCloudEvent()
    {
        string sourceBaseUrl = "https://test.de/events/spec/v1/test/scope";
        string scope = "test/scope";
        string type = "model.created";

        CloudEvent cloudEvent = CloudEventFactory.CreateRawCloudEvent(sourceBaseUrl, scope, type, null, ContentTypes.ApplicationJson, null);

        Assert.NotNull(cloudEvent);
        Assert.Contains(type, cloudEvent.Type);
    }

    [Fact]
    public void CreateFormatter_ReturnsJsonFormatter()
    {
        JsonEventFormatter formatter = CloudEventFactory.CreateFormatter();

        Assert.NotNull(formatter);
    }
}
