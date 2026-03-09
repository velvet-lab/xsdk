using xSdk.Shared;

namespace xSdk.Plugin.Tests.Shared;

public class DictionaryExtensionsTests
{
    [Fact]
    public void AddOrNew_WithNewKey_AddsKeyValuePair()
    {
        var dictionary = new Dictionary<string, string>();

        dictionary.AddOrNew("key1", "value1");

        Assert.Equal("value1", dictionary["key1"]);
    }

    [Fact]
    public void AddOrNew_WithExistingKey_UpdatesValue()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "key1", "oldValue" }
        };

        dictionary.AddOrNew("key1", "newValue");

        Assert.Equal("newValue", dictionary["key1"]);
        Assert.Single(dictionary);
    }

    [Fact]
    public void AddOrNew_WithNullKey_DoesNothing()
    {
        var dictionary = new Dictionary<string, string>();

        dictionary.AddOrNew<string, string>(null, "value");

        Assert.Empty(dictionary);
    }

    [Fact]
    public void AddOrNew_WithEmptyStringKey_DoesNotAdd()
    {
        var dictionary = new Dictionary<string, string>();

        dictionary.AddOrNew(string.Empty, "value");

        Assert.Empty(dictionary);
    }

    [Fact]
    public void AddOrNew_WithIntValue_ConvertsToString()
    {
        var dictionary = new Dictionary<string, string>();

        dictionary.AddOrNew("key1", 42);

        Assert.Equal("42", dictionary["key1"]);
    }

    [Fact]
    public void AddOrNew_WithBoolValue_ConvertsToString()
    {
        var dictionary = new Dictionary<string, string>();

        dictionary.AddOrNew("key1", true);

        Assert.Equal("True", dictionary["key1"]);
    }

    [Fact]
    public void AddOrNew_WithExistingIntValue_UpdatesValue()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "key1", "10" }
        };

        dictionary.AddOrNew("key1", 42);

        Assert.Equal("42", dictionary["key1"]);
    }

    [Fact]
    public void AddOrNew_WithKeyValuePair_AddsNewPair()
    {
        var dictionary = new Dictionary<string, int>();
        var kvp = new KeyValuePair<string, int>("key1", 100);

        dictionary.AddOrNew(kvp);

        Assert.Contains("key1", dictionary);
        Assert.Equal(100, dictionary["key1"]);
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

        Assert.Equal(100, dictionary["key1"]);
        Assert.Single(dictionary);
    }

    [Fact]
    public void AddOrNew_GenericDictionary_WithNewKey_AddsKeyValuePair()
    {
        var dictionary = new Dictionary<int, string>();

        dictionary.AddOrNew(1, "value1");

        Assert.Contains(1, dictionary);
        Assert.Equal("value1", dictionary[1]);
    }

    [Fact]
    public void AddOrNew_GenericDictionary_WithExistingKey_UpdatesValue()
    {
        var dictionary = new Dictionary<int, string>
        {
            { 1, "oldValue" }
        };

        dictionary.AddOrNew(1, "newValue");

        Assert.Equal("newValue", dictionary[1]);
        Assert.Single(dictionary);
    }

    [Fact]
    public void GetValue_WithExistingKey_ReturnsConvertedValue()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "age", "42" }
        };

        var result = dictionary.GetValue<int>("age");

        Assert.Equal(42, result);
    }

    [Fact]
    public void GetValue_WithNonExistingKey_ReturnsDefault()
    {
        var dictionary = new Dictionary<string, string>();

        var result = dictionary.GetValue<int>("nonexistent");

        Assert.Equal(0, result); // default for int
    }

    [Fact]
    public void GetValue_WithEmptyKey_ReturnsDefault()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "key1", "value1" }
        };

        var result = dictionary.GetValue<string>(string.Empty);

        Assert.Null(result);
    }

    [Fact]
    public void GetValue_WithNullKey_ReturnsDefault()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "key1", "value1" }
        };

        var result = dictionary.GetValue<string>(null);

        Assert.Null(result);
    }

    [Fact]
    public void GetValue_WithBooleanValue_ConvertsCorrectly()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "isActive", "true" }
        };

        var result = dictionary.GetValue<bool>("isActive");

        Assert.True(result);
    }

    [Fact]
    public void GetValue_WithDoubleValue_ConvertsCorrectly()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "price", "19.99" }
        };

        var result = dictionary.GetValue<double>("price");

        Assert.Equal(19.99, result);
    }

    [Fact]
    public void AddOrNew_MultipleCalls_MaintainsCorrectCount()
    {
        var dictionary = new Dictionary<string, string>();

        dictionary.AddOrNew("key1", "value1");
        dictionary.AddOrNew("key2", "value2");
        dictionary.AddOrNew("key1", "updatedValue1");

        Assert.Equal(2, dictionary.Count);
        Assert.Equal("updatedValue1", dictionary["key1"]);
        Assert.Equal("value2", dictionary["key2"]);
    }
}
