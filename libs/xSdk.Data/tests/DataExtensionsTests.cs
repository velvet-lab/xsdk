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

using MapsterMapper;
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

}
