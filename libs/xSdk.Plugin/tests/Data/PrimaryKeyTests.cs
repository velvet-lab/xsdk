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

namespace xSdk.Plugin.Tests.Data;

public class PrimaryKeyTests
{
    [Fact]
    public void GuidPK_DefaultConstructor_GeneratesNewGuid()
    {
        var pk = new GuidPK();

        var value = pk.GetValue<Guid>();
        Assert.NotEqual(Guid.Empty, value);
    }

    [Fact]
    public void GuidPK_GuidConstructor_StoresValue()
    {
        var guid = Guid.NewGuid();

        var pk = new GuidPK(guid);

        Assert.Equal(guid, pk.GetValue<Guid>());
    }

    [Fact]
    public void GuidPK_StringConstructor_ParsesGuid()
    {
        var guid = Guid.NewGuid();

        var pk = new GuidPK(guid.ToString());

        Assert.Equal(guid, pk.GetValue<Guid>());
    }

    [Fact]
    public void GuidPK_GetValueAsString_ReturnsStringRepresentation()
    {
        var guid = Guid.NewGuid();
        var pk = new GuidPK(guid);

        var result = pk.GetValue<string>();

        Assert.NotNull(result);
        Assert.Equal(guid.ToString(), result);
    }

    [Fact]
    public void GuidPK_EqualityOperator_SameGuids_ReturnsTrue()
    {
        var guid = Guid.NewGuid();
        var pk1 = new GuidPK(guid);
        var pk2 = new GuidPK(guid);

        Assert.True(pk1 == pk2);
    }

    [Fact]
    public void GuidPK_EqualityOperator_DifferentGuids_ReturnsFalse()
    {
        var pk1 = new GuidPK(Guid.NewGuid());
        var pk2 = new GuidPK(Guid.NewGuid());

        Assert.False(pk1 == pk2);
    }

    [Fact]
    public void GuidPK_InequalityOperator_DifferentGuids_ReturnsTrue()
    {
        var pk1 = new GuidPK(Guid.NewGuid());
        var pk2 = new GuidPK(Guid.NewGuid());

        Assert.True(pk1 != pk2);
    }

    [Fact]
    public void GuidPK_SetValue_UpdatesValue()
    {
        var pk = new GuidPK();
        var newGuid = Guid.NewGuid();

        pk.SetValue(newGuid);

        Assert.Equal(newGuid, pk.GetValue<Guid>());
    }

    [Fact]
    public void GuidPK_SetValue_WithNull_DoesNotUpdate()
    {
        var pk = new GuidPK();
        var original = pk.GetValue<Guid>();

        pk.SetValue(null!);

        Assert.Equal(original, pk.GetValue<Guid>());
    }

    [Fact]
    public void KeyValuePK_DefaultConstructor_HasEmptyValue()
    {
        var pk = new KeyValuePK();

        Assert.Equal(string.Empty, pk.GetValue<string>());
    }

    [Fact]
    public void KeyValuePK_StringConstructor_StoresValue()
    {
        var pk = new KeyValuePK("my-key");

        Assert.Equal("my-key", pk.GetValue<string>());
    }

    [Fact]
    public void KeyValuePK_EqualityOperator_SameKeys_ReturnsTrue()
    {
        var pk1 = new KeyValuePK("key1");
        var pk2 = new KeyValuePK("key1");

        Assert.True(pk1 == pk2);
    }

    [Fact]
    public void KeyValuePK_EqualityOperator_DifferentKeys_ReturnsFalse()
    {
        var pk1 = new KeyValuePK("key1");
        var pk2 = new KeyValuePK("key2");

        Assert.False(pk1 == pk2);
    }

    [Fact]
    public void KeyValuePK_ToString_ReturnsStringValue()
    {
        var pk = new KeyValuePK("mykey");

        Assert.Equal("mykey", pk.ToString());
    }

    [Fact]
    public void GuidPK_Equals_SameInstance_ReturnsTrue()
    {
        var guid = Guid.NewGuid();
        var pk = new GuidPK(guid);

        Assert.True(pk.Equals(pk));
    }

    [Fact]
    public void GuidPK_Equals_Null_ReturnsFalse()
    {
        var pk = new GuidPK(Guid.NewGuid());

        Assert.False(pk.Equals(null));
    }

    [Fact]
    public void GuidPK_EqualityOperator_BothNull_ReturnsTrue()
    {
        GuidPK? pk1 = null;
        GuidPK? pk2 = null;

#pragma warning disable CS8604
        Assert.True(pk1 == pk2);
#pragma warning restore CS8604
    }

    [Fact]
    public void GuidPK_EqualityOperator_LeftNull_ReturnsFalse()
    {
        GuidPK? pk1 = null;
        var pk2 = new GuidPK(Guid.NewGuid());

#pragma warning disable CS8604
        Assert.False(pk1 == pk2);
#pragma warning restore CS8604
    }

    [Fact]
    public void GuidPK_EqualityOperator_RightNull_ReturnsFalse()
    {
        var pk1 = new GuidPK(Guid.NewGuid());
        GuidPK? pk2 = null;

#pragma warning disable CS8604
        Assert.False(pk1 == pk2);
#pragma warning restore CS8604
    }
}
