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

namespace xSdk.Shared;

public static class DictionaryExtensions
{
    private static object _lock = new object();

    public static void AddOrNew<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (key != null)
        {
            lock (_lock)
            {
                if (dictionary.ContainsKey(key))
                    dictionary[key] = value;
                else
                    dictionary.Add(key, value);
            }
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
                return TypeConverter.ConvertTo<TValue>(value);
            }
        }

        return default;
    }
}
