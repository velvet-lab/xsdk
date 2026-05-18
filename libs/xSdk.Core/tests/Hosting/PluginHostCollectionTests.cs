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

namespace xSdk.Hosting;

public class PluginHostCollectionTests
{
    private readonly IReadOnlyList<Type> _types = [typeof(string), typeof(int), typeof(bool)];

    [Fact]
    public void Count_ReturnsExpectedCount()
    {
        var collection = new PluginHostCollection(_types);

        Assert.Equal(3, collection.Count);
    }

    [Fact]
    public void Indexer_ReturnsCorrectType()
    {
        var collection = new PluginHostCollection(_types);

        Assert.Equal(typeof(int), collection[1]);
    }

    [Fact]
    public void GetEnumerator_IteratesAllItems()
    {
        var collection = new PluginHostCollection(_types);

        var result = new List<Type>();
        foreach (Type type in collection)
        {
            result.Add(type);
        }

        Assert.Equal(_types.Count, result.Count);
        Assert.Contains(typeof(string), result);
        Assert.Contains(typeof(int), result);
        Assert.Contains(typeof(bool), result);
    }

    [Fact]
    public void GetEnumerator_NonGeneric_IteratesAllItems()
    {
        var collection = new PluginHostCollection(_types);

        int count = 0;
        var enumerable = (System.Collections.IEnumerable)collection;
        foreach (object? item in enumerable)
        {
            count++;
        }

        Assert.Equal(3, count);
    }

    [Fact]
    public void EmptyCollection_HasZeroCount()
    {
        var collection = new PluginHostCollection([]);

        Assert.Empty(collection);
    }
}
