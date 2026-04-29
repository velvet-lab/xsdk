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

public class DataExtensionsTests
{
    [Fact]
    public void ToEntity_FromModel_MapsCorrectly()
    {
        var model = new TestModel { Name = "Alice", Age = 30 };

        var entity = model.ToEntity<TestMappingProfile, TestEntity>();

        Assert.NotNull(entity);
        Assert.Equal(model.Name, entity.Name);
        Assert.Equal(model.Age, entity.Age);
    }

    [Fact]
    public void ToModel_FromEntity_MapsCorrectly()
    {
        var entity = new TestEntity { Name = "Bob", Age = 25 };

        var model = entity.ToModel<TestMappingProfile, TestModel>();

        Assert.NotNull(model);
        Assert.Equal(entity.Name, model.Name);
        Assert.Equal(entity.Age, model.Age);
    }

    [Fact]
    public void ToJson_SerializesModelToJsonString()
    {
        var model = new TestModel { Name = "Eve", Age = 20 };

        var json = model.ToJson();

        Assert.NotNull(json);
        Assert.Contains("Eve", json);
    }

    [Fact]
    public void ToEntity_Collection_MapsAllItems()
    {
        var models = new List<IModel>
        {
            new TestModel { Name = "Alice", Age = 30 },
            new TestModel { Name = "Bob", Age = 25 },
        };

        var entities = models.ToEntity<TestMappingProfile, TestEntity>();

        Assert.NotNull(entities);
        Assert.Equal(2, entities.Count());
    }

    [Fact]
    public void ToModel_Collection_MapsAllItems()
    {
        var entities = new List<IEntity>
        {
            new TestEntity { Name = "Alice", Age = 30 },
            new TestEntity { Name = "Bob", Age = 25 },
        };

        var models = entities.ToModel<TestMappingProfile, TestModel>();

        Assert.NotNull(models);
        Assert.Equal(2, models.Count());
    }

    [Fact]
    public void CopyToEntity_CopiesProperties()
    {
        var source = new TestEntity { Name = "Source", Age = 10 };
        var dest = new TestEntity { Name = "Dest", Age = 99 };

        var result = source.CopyToEntity(dest);

        Assert.NotNull(result);
        Assert.Equal("Source", result.Name);
        Assert.Equal(10, result.Age);
    }

    [Fact]
    public void CopyToModel_CopiesProperties()
    {
        var source = new TestModel { Name = "Source", Age = 10 };
        var dest = new TestModel { Name = "Dest", Age = 99 };

        var result = source.CopyToModel(dest);

        Assert.NotNull(result);
        Assert.Equal("Source", result.Name);
        Assert.Equal(10, result.Age);
    }

    [Fact]
    public void EnrichEntity_CopiesPropertiesViaMapper()
    {
        var mapper = MappingFactory.CreateMapper<TestMappingProfile>();
        var source = new TestEntity { Name = "Source", Age = 5 };
        var dest = new TestEntity { Name = "Dest", Age = 99 };

        var result = mapper.EnrichEntity(source, dest);

        Assert.NotNull(result);
        Assert.Equal("Source", result.Name);
        Assert.Equal(5, result.Age);
    }
}
