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

namespace xSdk.Extensions.Links;

public class LinksOptionsTests
{
    [Fact]
    public void Policies_IsNotNull()
    {
        var options = new LinksOptions();

        Assert.NotNull(options.Policies);
    }

    [Fact]
    public void Policies_IsEmptyByDefault()
    {
        var options = new LinksOptions();

        Assert.Empty(options.Policies);
    }

    [Fact]
    public void Policies_CanAddEntry()
    {
        var options = new LinksOptions();

        // The list is a mutable List<IPolicy>
        Assert.IsType<List<IPolicy>>(options.Policies);
    }
}
