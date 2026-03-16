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

using MongoDB.Bson;

namespace xSdk.Data;

public class MongoDbModelPKTests
{
    [Fact]
    public void CreateNewPrimaryKey()
    {
        PrimaryKey primaryKey = new MongoDbModelPK();

        Assert.NotNull(primaryKey);
        Assert.IsType<string>(primaryKey.GetValue());
    }

    [Fact]
    public void CreateNewPrimaryKeyFromObjectId()
    {
        var objectId = ObjectId.GenerateNewId();
        PrimaryKey primaryKey = new MongoDbModelPK(objectId);

        Assert.NotNull(primaryKey);
        Assert.Equal(objectId, primaryKey.GetValue<ObjectId>());
        Assert.IsType<string>(primaryKey.GetValue());
    }

    [Fact]
    public void CreateNewPrimaryKeyFromString()
    {
        var objectId = ObjectId.GenerateNewId();
        var objectIdAsString = objectId.ToString();

        PrimaryKey primaryKey = new MongoDbModelPK(objectIdAsString);

        Assert.NotNull(primaryKey);
        Assert.Equal(objectIdAsString, primaryKey.GetValue()?.ToString());
        Assert.IsType<string>(primaryKey.GetValue());
    }
}
