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

using xSdk.Extensions.Links;

namespace xSdk.Extensions.AspNetCore.Links.Tests;

public class HateoasItemTests
{
    [Fact]
    public void HateoasItem_DefaultConstructor_HasEmptyProperties()
    {
        var item = new HateoasItem();

        Assert.Equal(string.Empty, item.Rel);
        Assert.Equal(string.Empty, item.Href);
        Assert.Equal(string.Empty, item.Method);
    }

    [Fact]
    public void HateoasItem_SetRel_StoresValue()
    {
        var item = new HateoasItem { Rel = "self" };

        Assert.Equal("self", item.Rel);
    }

    [Fact]
    public void HateoasItem_SetHref_StoresValue()
    {
        var item = new HateoasItem { Href = "https://example.com/api/items/1" };

        Assert.Equal("https://example.com/api/items/1", item.Href);
    }

    [Fact]
    public void HateoasItem_SetMethod_StoresValue()
    {
        var item = new HateoasItem { Method = "GET" };

        Assert.Equal("GET", item.Method);
    }

    [Fact]
    public void HateoasItem_ImplementsIHateoasItem()
    {
        var item = new HateoasItem();

        Assert.IsAssignableFrom<IHateoasItem>(item);
    }
}
