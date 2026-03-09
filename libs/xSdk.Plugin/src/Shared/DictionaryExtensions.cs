using Google.Protobuf.WellKnownTypes;

namespace xSdk.Shared;

public static class DictionaryExtensions
{
    public static void AddOrNew<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (key != null)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);
        }
    }

    public static void AddOrNew<TValue>(this IDictionary<string, string> dictionary, string key, TValue value)
    {
        if (!string.IsNullOrEmpty(key))
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value.ToString();
            else
                dictionary.Add(key, value.ToString());
        }
    }

    public static void AddOrNew<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> item)
    {
        if (dictionary.ContainsKey(item.Key))
            dictionary[item.Key] = item.Value;
        else
            dictionary.Add(item.Key, item.Value);
    }

    public static TValue GetValue<TValue>(this IDictionary<string, string> dictionary, string key)
    {
        if (!string.IsNullOrEmpty(key))
        {
            if (dictionary.TryGetValue(key, out string value))
            {
                if (value.Contains('.'))
                {
                    value = value.Replace('.', ',');
                }

                return TypeConverter.ConvertTo<TValue>(value);
            }
        }

        return default;
    }
}
