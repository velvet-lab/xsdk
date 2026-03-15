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

using xSdk.Data.Mocks;

namespace xSdk.Data;

public class MappingTests
{
    [Fact]
    public void MapEntityToModel()
    {
        var entity = new TestEntity { Age = 42, Name = "John Doe" };

        var model = entity.ToModel<TestMappingProfile, TestModel>();

        Assert.NotNull(model);
        Assert.Equal(entity.Name, model.Name);
        Assert.Equal(entity.Age, model.Age);
        Assert.Equal(entity.Id.ToString(), model.Id);
        Assert.IsType<string>(model.PrimaryKey.GetValue());
    }

    [Fact]
    public void MapModelToEntity()
    {
        var model = new TestModel { Age = 42, Name = "John Doe" };

        var entity = model.ToEntity<TestMappingProfile, TestEntity>();

        Assert.NotNull(entity);
        Assert.Equal(model.Name, entity.Name);
        Assert.Equal(model.Age, entity.Age);
        Assert.Equal(model.Id, entity.Id.ToString());
        Assert.IsType<Guid>(entity.PrimaryKey.GetValue());
    }
}
