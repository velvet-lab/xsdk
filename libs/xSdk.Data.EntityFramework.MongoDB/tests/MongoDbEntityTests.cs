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
using xSdk.Data.Mocks;

namespace xSdk.Data;

public class MongoDbEntityTests
{
    [Fact]
    public void CreateNewEntity()
    {
        var entity = new TestEntity();

        Assert.NotNull(entity);
        Assert.NotNull(entity.Id);
        Assert.Equal(entity.Id, entity.Id);
        Assert.IsType<ObjectId>(entity.Id);
    }

    [Fact]
    public void CreateNewEntityFromExistingPrimaryKey()
    {
        var pk = ObjectId.GenerateNewId();
        var entity = new TestEntity();
        entity.Id = pk;

        Assert.NotNull(entity);
        Assert.NotNull(entity.Id);
        Assert.Equal(entity.Id, entity.Id);
        Assert.IsType<ObjectId>(entity.Id);
    }    
}
