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

using LiteDB;

namespace xSdk.Data;

public class NoSqlEntityPKTests
{
    [Fact]
    public void CreateNewPrimaryKey()
    {
        PrimaryKey primaryKey = new NoSqlEntityPK();

        Assert.NotNull(primaryKey);
    }

    [Fact]
    public void CreateNewPrimaryKeyFromObjectId()
    {
        var objectId = ObjectId.NewObjectId();
        PrimaryKey primaryKey = new NoSqlEntityPK(objectId);

        Assert.NotNull(primaryKey);
        Assert.Equal(objectId, primaryKey.GetValue<ObjectId>());
    }

    [Fact]
    public void CreateNewPrimaryKeyFromString()
    {
        var objectId = ObjectId.NewObjectId();
        var objectIdAsString = objectId.ToString();

        PrimaryKey primaryKey = new NoSqlEntityPK(objectIdAsString);

        Assert.NotNull(primaryKey);
        Assert.Equal(objectIdAsString, primaryKey.GetValue()?.ToString());
    }
}
