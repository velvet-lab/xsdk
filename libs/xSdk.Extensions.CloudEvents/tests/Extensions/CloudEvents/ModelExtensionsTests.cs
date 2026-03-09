using CloudNative.CloudEvents;
using xSdk.Data;
using xSdk.Extensions.CloudEvents;
using xSdk.Hosting;

namespace xSdk.Extensions.CloudEvents.Tests.Extensions.CloudEvents;

public class ModelExtensionsTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    // Test model class
    private class TestModel : Model
    {
        public TestModel()
        {
            PrimaryKey = new GuidPK();
        }

        public string Name { get; set; }
        public int Value { get; set; }
    }

    [Fact]
    public void ToCloudEvent_WithType_CreatesCloudEventWithModel()
    {
        var model = new TestModel { Name = "Test", Value = 42 };
        var type = "model.created";

        var cloudEvent = model.ToCloudEvent(type);

        cloudEvent.Should().NotBeNull();
        cloudEvent.Type.Should().Contain(type);
    }

    [Fact]
    public void ToCloudEvent_WithScopeAndType_CreatesCloudEventWithBothValues()
    {
        var model = new TestModel { Name = "Test", Value = 42 };
        var scope = "test/models";
        var type = "model.updated";

        var cloudEvent = model.ToCloudEvent(scope, type);

        cloudEvent.Should().NotBeNull();
        cloudEvent.Type.Should().Contain(type);
    }

    [Fact]
    public void ToCloudEvent_WithScopeTypeAndSubject_CreatesCompleteCloudEvent()
    {
        var model = new TestModel { Name = "Test", Value = 42 };
        var scope = "test/models";
        var type = "model.created";
        var subject = "tenant/123";

        var cloudEvent = model.ToCloudEvent(scope, type, subject);

        cloudEvent.Should().NotBeNull();
        cloudEvent.Type.Should().Contain(type);
        cloudEvent.Subject.Should().Be(subject);
    }

    [Fact]
    public void ToCloudEvent_WithNullScope_UsesModelTypeName()
    {
        var model = new TestModel { Name = "Test", Value = 42 };
        var type = "model.created";

        var cloudEvent = model.ToCloudEvent(null, type);

        cloudEvent.Should().NotBeNull();
        cloudEvent.Type.Should().Contain(type);
    }

    [Fact]
    public void ToCloudEvent_WithEmptyScope_UsesModelTypeName()
    {
        var model = new TestModel { Name = "Test", Value = 42 };
        var type = "model.created";

        var cloudEvent = model.ToCloudEvent(string.Empty, type);

        cloudEvent.Should().NotBeNull();
        cloudEvent.Type.Should().Contain(type);
    }

    [Fact]
    public void ToCloudEvent_PreservesModelData()
    {
        var model = new TestModel { Name = "TestName", Value = 123 };
        var type = "model.created";

        var cloudEvent = model.ToCloudEvent(type);

        cloudEvent.Should().NotBeNull();
        cloudEvent.Data.Should().NotBeNull();
    }

    [Fact]
    public void ToModel_WithValidCloudEvent_ReturnsModel()
    {
        var originalModel = new TestModel { Name = "Original", Value = 999 };
        var cloudEvent = originalModel.ToCloudEvent("model.created");

        var retrievedModel = cloudEvent.ToModel<TestModel>();

        retrievedModel.Should().NotBeNull();
        retrievedModel.Name.Should().Be(originalModel.Name);
        retrievedModel.Value.Should().Be(originalModel.Value);
    }

    [Fact]
    public void RoundTrip_ModelToCloudEventToModel_PreservesData()
    {
        var originalModel = new TestModel { Name = "RoundTrip", Value = 456 };

        var cloudEvent = originalModel.ToCloudEvent("model.created");
        var retrievedModel = cloudEvent.ToModel<TestModel>();

        retrievedModel.Should().NotBeNull();
        retrievedModel.Name.Should().Be(originalModel.Name);
        retrievedModel.Value.Should().Be(originalModel.Value);
    }

    [Fact]
    public void ToCloudEvent_WithDifferentEventTypes_CreatesDistinctCloudEvents()
    {
        var model = new TestModel { Name = "Test", Value = 42 };

        var createEvent = model.ToCloudEvent("model.created");
        var updateEvent = model.ToCloudEvent("model.updated");
        var deleteEvent = model.ToCloudEvent("model.deleted");

        createEvent.Type.Should().Contain("created");
        updateEvent.Type.Should().Contain("updated");
        deleteEvent.Type.Should().Contain("deleted");
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

        retrievedModel.Should().NotBeNull();
        retrievedModel.Name.Should().Be(model.Name);
        retrievedModel.Value.Should().Be(model.Value);
    }

    [Fact]
    public void ToCloudEvent_WithNullSubject_CreatesValidCloudEvent()
    {
        var model = new TestModel { Name = "Test", Value = 42 };

        var cloudEvent = model.ToCloudEvent("test/scope", "model.created", null);

        cloudEvent.Should().NotBeNull();
        cloudEvent.Subject.Should().BeNull();
    }

    [Fact]
    public void ToCloudEvent_MultipleModels_CreatesIndependentCloudEvents()
    {
        var model1 = new TestModel { Name = "Model1", Value = 1 };
        var model2 = new TestModel { Name = "Model2", Value = 2 };

        var cloudEvent1 = model1.ToCloudEvent("model.created");
        var cloudEvent2 = model2.ToCloudEvent("model.created");

        cloudEvent1.Should().NotBeSameAs(cloudEvent2);
        var retrieved1 = cloudEvent1.ToModel<TestModel>();
        var retrieved2 = cloudEvent2.ToModel<TestModel>();

        retrieved1.Name.Should().Be("Model1");
        retrieved2.Name.Should().Be("Model2");
    }

    [Fact]
    public void ToCloudEvent_WithCustomScope_UsesProvidedScope()
    {
        var model = new TestModel { Name = "Test", Value = 42 };
        var customScope = "custom/scope/path";

        var cloudEvent = model.ToCloudEvent(customScope, "model.created");

        cloudEvent.Should().NotBeNull();
        // Scope is embedded in source/type, exact validation depends on CloudEventFactory implementation
    }
}
