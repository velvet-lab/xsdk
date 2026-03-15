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

namespace xSdk.Data;

public class GuidPKTests
{
    [Fact]
    public void CreateNewPrimaryKey()
    {
        GuidPK primaryKey = new GuidPK();

        Assert.NotNull(primaryKey);
        Assert.IsType<Guid>(primaryKey.GetValue());
    }

    [Fact]
    public void CreateNewPrimaryKeyFromGuid()
    {
        var id = Guid.NewGuid();
        var expected = id.ToString();
        GuidPK primaryKey = new GuidPK(id);

        Assert.NotNull(primaryKey);
        Assert.Equal(expected, primaryKey.GetValue().ToString());
        Assert.IsType<Guid>(primaryKey.GetValue());
    }

    [Fact]
    public void CreateNewPrimaryKeyFromString()
    {
        var id = Guid.NewGuid();
        var expected = id.ToString();

        GuidPK primaryKey = new GuidPK(expected);

        Assert.NotNull(primaryKey);
        Assert.Equal(expected, Guid.Parse(primaryKey.GetValue().ToString()).ToString());
        Assert.IsType<Guid>(primaryKey.GetValue());
    }
}
