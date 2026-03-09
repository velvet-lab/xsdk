using xSdk.Shared;

namespace xSdk.Plugin.Tests.Shared;

public class DictionaryExtensionsTests
{
    [Fact]
    public void AddOrNew_WithNewKey_AddsKeyValuePair()
    {
        var dictionary = new Dictionary<string, string>();

        dictionary.AddOrNew("key1", "value1");

        dictionary.Should().ContainKey("key1");
        dictionary["key1"].Should().Be("value1");
    }

    [Fact]
    public void AddOrNew_WithExistingKey_UpdatesValue()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "key1", "oldValue" }
        };

        dictionary.AddOrNew("key1", "newValue");

        dictionary["key1"].Should().Be("newValue");
        dictionary.Should().HaveCount(1);
    }

    [Fact]
    public void AddOrNew_WithNullKey_DoesNothing()
    {
        var dictionary = new Dictionary<string, string>();

        dictionary.AddOrNew<string, string>(null, "value");

        dictionary.Should().BeEmpty();
    }

    [Fact]
    public void AddOrNew_WithEmptyStringKey_DoesNotAdd()
    {
        var dictionary = new Dictionary<string, string>();

        dictionary.AddOrNew(string.Empty, "value");

        dictionary.Should().BeEmpty();
    }

    [Fact]
    public void AddOrNew_WithIntValue_ConvertsToString()
    {
        var dictionary = new Dictionary<string, string>();

        dictionary.AddOrNew("key1", 42);

        dictionary["key1"].Should().Be("42");
    }

    [Fact]
    public void AddOrNew_WithBoolValue_ConvertsToString()
    {
        var dictionary = new Dictionary<string, string>();

        dictionary.AddOrNew("key1", true);

        dictionary["key1"].Should().Be("True");
    }

    [Fact]
    public void AddOrNew_WithExistingIntValue_UpdatesValue()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "key1", "10" }
        };

        dictionary.AddOrNew("key1", 42);

        dictionary["key1"].Should().Be("42");
    }

    [Fact]
    public void AddOrNew_WithKeyValuePair_AddsNewPair()
    {
        var dictionary = new Dictionary<string, int>();
        var kvp = new KeyValuePair<string, int>("key1", 100);

        dictionary.AddOrNew(kvp);

        dictionary.Should().ContainKey("key1");
        dictionary["key1"].Should().Be(100);
    }

    [Fact]
    public void AddOrNew_WithKeyValuePair_UpdatesExistingPair()
    {
        var dictionary = new Dictionary<string, int>
        {
            { "key1", 50 }
        };
        var kvp = new KeyValuePair<string, int>("key1", 100);

        dictionary.AddOrNew(kvp);

        dictionary["key1"].Should().Be(100);
        dictionary.Should().HaveCount(1);
    }

    [Fact]
    public void AddOrNew_GenericDictionary_WithNewKey_AddsKeyValuePair()
    {
        var dictionary = new Dictionary<int, string>();

        dictionary.AddOrNew(1, "value1");

        dictionary.Should().ContainKey(1);
        dictionary[1].Should().Be("value1");
    }

    [Fact]
    public void AddOrNew_GenericDictionary_WithExistingKey_UpdatesValue()
    {
        var dictionary = new Dictionary<int, string>
        {
            { 1, "oldValue" }
        };

        dictionary.AddOrNew(1, "newValue");

        dictionary[1].Should().Be("newValue");
        dictionary.Should().HaveCount(1);
    }

    [Fact]
    public void GetValue_WithExistingKey_ReturnsConvertedValue()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "age", "42" }
        };

        var result = dictionary.GetValue<int>("age");

        result.Should().Be(42);
    }

    [Fact]
    public void GetValue_WithNonExistingKey_ReturnsDefault()
    {
        var dictionary = new Dictionary<string, string>();

        var result = dictionary.GetValue<int>("nonexistent");

        result.Should().Be(0); // default for int
    }

    [Fact]
    public void GetValue_WithEmptyKey_ReturnsDefault()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "key1", "value1" }
        };

        var result = dictionary.GetValue<string>(string.Empty);

        result.Should().BeNull();
    }

    [Fact]
    public void GetValue_WithNullKey_ReturnsDefault()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "key1", "value1" }
        };

        var result = dictionary.GetValue<string>(null);

        result.Should().BeNull();
    }

    [Fact]
    public void GetValue_WithBooleanValue_ConvertsCorrectly()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "isActive", "true" }
        };

        var result = dictionary.GetValue<bool>("isActive");

        result.Should().BeTrue();
    }

    [Fact]
    public void GetValue_WithDoubleValue_ConvertsCorrectly()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "price", "19.99" }
        };

        var result = dictionary.GetValue<double>("price");

        result.Should().Be(19.99);
    }

    [Fact]
    public void AddOrNew_MultipleCalls_MaintainsCorrectCount()
    {
        var dictionary = new Dictionary<string, string>();

        dictionary.AddOrNew("key1", "value1");
        dictionary.AddOrNew("key2", "value2");
        dictionary.AddOrNew("key1", "updatedValue1");

        dictionary.Should().HaveCount(2);
        dictionary["key1"].Should().Be("updatedValue1");
        dictionary["key2"].Should().Be("value2");
    }
}
