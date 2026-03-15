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

public class MappingTests
{
    [Fact]
    public void Map2Model()
    {
        var fake = FakeGenerator.Generate<TestEntityFakes, TestEntity>();
        var model = fake.ToModel<TestMappingProfile, TestModel>();

        Assert.NotNull(model);
        Assert.Equal(fake.Id.ToString(), model.Id);
        Assert.Equal(fake.Name, model.MyName);
        Assert.Equal(fake.Age, model.MyAge);
        Assert.IsType<string>(model.PrimaryKey.GetValue());
    }

    [Fact]
    public void Map2Entity()
    {
        var fake = FakeGenerator.Generate<TestEntityFakes, TestEntity>();
        var model = new TestModel
        {
            Id = ObjectId.GenerateNewId().ToString(),
            MyName = fake.Name,
            MyAge = fake.Age,
        };
        var entity = model.ToEntity<TestMappingProfile, TestEntity>();

        Assert.NotNull(entity);
        Assert.Equal(model.Id, entity.Id.ToString());
        Assert.Equal(fake.Name, entity.Name);
        Assert.Equal(fake.Age, entity.Age);
        Assert.IsType<ObjectId>(entity.PrimaryKey.GetValue());
    }
}
