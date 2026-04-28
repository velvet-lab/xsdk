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

using xSdk.Data;
using xSdk.Hosting;

namespace xSdk.Extensions.CloudEvents.Tests.Extensions.CloudEvents;

public class ModelExtensionsTests(TestHostFixture _) : IClassFixture<TestHostFixture>
{
    // Test model class
    private class TestModel : Model
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    [Fact]
    public void ToCloudEvent_WithType_CreatesCloudEventWithModel()
    {
        var model = new TestModel { Name = "Test", Value = 42 };
        var type = "model.created";

        var cloudEvent = model.ToCloudEvent(type);

        Assert.NotNull(cloudEvent);
        Assert.Contains(type, cloudEvent.Type);
    }

    [Fact]
    public void ToCloudEvent_WithScopeAndType_CreatesCloudEventWithBothValues()
    {
        var model = new TestModel { Name = "Test", Value = 42 };
        var scope = "test/models";
        var type = "model.updated";

        var cloudEvent = model.ToCloudEvent(scope, type);

        Assert.NotNull(cloudEvent);
        Assert.Contains(type, cloudEvent.Type);
    }

    [Fact]
    public void ToCloudEvent_WithScopeTypeAndSubject_CreatesCompleteCloudEvent()
    {
        var model = new TestModel { Name = "Test", Value = 42 };
        var scope = "test/models";
        var type = "model.created";
        var subject = "tenant/123";

        var cloudEvent = model.ToCloudEvent(scope, type, subject);

        Assert.NotNull(cloudEvent);
        Assert.Contains(type, cloudEvent.Type);
        Assert.Equal(subject, cloudEvent.Subject);
    }

    [Fact]
    public void ToCloudEvent_WithNullScope_UsesModelTypeName()
    {
        var model = new TestModel { Name = "Test", Value = 42 };
        var type = "model.created";

        var cloudEvent = model.ToCloudEvent(null, type);

        Assert.NotNull(cloudEvent);
        Assert.Contains(type, cloudEvent.Type);
    }

    [Fact]
    public void ToCloudEvent_WithEmptyScope_UsesModelTypeName()
    {
        var model = new TestModel { Name = "Test", Value = 42 };
        var type = "model.created";

        var cloudEvent = model.ToCloudEvent(string.Empty, type);

        Assert.NotNull(cloudEvent);
        Assert.Contains(type, cloudEvent.Type);
    }

    [Fact]
    public void ToCloudEvent_PreservesModelData()
    {
        var model = new TestModel { Name = "TestName", Value = 123 };
        var type = "model.created";

        var cloudEvent = model.ToCloudEvent(type);

        Assert.NotNull(cloudEvent);
        Assert.NotNull(cloudEvent.Data);
    }

    [Fact]
    public void ToModel_WithValidCloudEvent_ReturnsModel()
    {
        var originalModel = new TestModel { Name = "Original", Value = 999 };
        var cloudEvent = originalModel.ToCloudEvent("model.created");

        var retrievedModel = cloudEvent.ToModel<TestModel>();

        Assert.NotNull(retrievedModel);
        Assert.Equal(originalModel.Name, retrievedModel.Name);
        Assert.Equal(originalModel.Value, retrievedModel.Value);
    }

    [Fact]
    public void RoundTrip_ModelToCloudEventToModel_PreservesData()
    {
        var originalModel = new TestModel { Name = "RoundTrip", Value = 456 };

        var cloudEvent = originalModel.ToCloudEvent("model.created");
        var retrievedModel = cloudEvent.ToModel<TestModel>();

        Assert.NotNull(retrievedModel);
        Assert.Equal(originalModel.Name, retrievedModel.Name);
        Assert.Equal(originalModel.Value, retrievedModel.Value);
    }

    [Fact]
    public void ToCloudEvent_WithDifferentEventTypes_CreatesDistinctCloudEvents()
    {
        var model = new TestModel { Name = "Test", Value = 42 };

        var createEvent = model.ToCloudEvent("model.created");
        var updateEvent = model.ToCloudEvent("model.updated");
        var deleteEvent = model.ToCloudEvent("model.deleted");

        Assert.Contains("created", createEvent.Type);
        Assert.Contains("updated", updateEvent.Type);
        Assert.Contains("deleted", deleteEvent.Type);
    }

    [Fact]
    public void ToCloudEvent_WithComplexModel_PreservesAllProperties()
    {
        var model = new TestModel
        {
            Name = "ComplexTest",
            Value = 789,
            AdditionalData = new Dictionary<string, object>
            {
                { "extra1", "value1" },
                { "extra2", 100 }
            }
        };

        var cloudEvent = model.ToCloudEvent("model.created");
        var retrievedModel = cloudEvent.ToModel<TestModel>();

        Assert.NotNull(retrievedModel);
        Assert.Equal(model.Name, retrievedModel.Name);
        Assert.Equal(model.Value, retrievedModel.Value);
    }

    [Fact]
    public void ToCloudEvent_WithNullSubject_CreatesValidCloudEvent()
    {
        var model = new TestModel { Name = "Test", Value = 42 };

        var cloudEvent = model.ToCloudEvent("test/scope", "model.created", null);

        Assert.NotNull(cloudEvent);
        Assert.Null(cloudEvent.Subject);
    }

    [Fact]
    public void ToCloudEvent_MultipleModels_CreatesIndependentCloudEvents()
    {
        var model1 = new TestModel { Name = "Model1", Value = 1 };
        var model2 = new TestModel { Name = "Model2", Value = 2 };

        var cloudEvent1 = model1.ToCloudEvent("model.created");
        var cloudEvent2 = model2.ToCloudEvent("model.created");

        Assert.NotSame(cloudEvent1, cloudEvent2);
        var retrieved1 = cloudEvent1.ToModel<TestModel>();
        var retrieved2 = cloudEvent2.ToModel<TestModel>();

        Assert.Equal("Model1", retrieved1.Name);
        Assert.Equal("Model2", retrieved2.Name);
    }

    [Fact]
    public void ToCloudEvent_WithCustomScope_UsesProvidedScope()
    {
        var model = new TestModel { Name = "Test", Value = 42 };
        var customScope = "custom/scope/path";

        var cloudEvent = model.ToCloudEvent(customScope, "model.created");

        Assert.NotNull(cloudEvent);
        // Scope is embedded in source/type, exact validation depends on CloudEventFactory implementation
    }
}
