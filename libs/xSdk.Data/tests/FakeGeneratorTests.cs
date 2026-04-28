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

public class FakeGeneratorTests
{
    [Fact]
    public void Generate_WithStrictMode_ReturnsEntity()
    {
        var entity = FakeGenerator.Generate<TestEntityFakes, TestEntity>(strictMode: false);

        Assert.NotNull(entity);
        Assert.True(entity.Id != Guid.Empty);
    }

    [Fact]
    public void Generate_WithContext_ReturnsEntity()
    {
        var entity = FakeGenerator.Generate<TestEntityFakes, TestEntity>("myContext");

        Assert.NotNull(entity);
        Assert.NotNull(entity.Name);
    }

    [Fact]
    public void Generate_WithContextAndStrictMode_ReturnsEntity()
    {
        var entity = FakeGenerator.Generate<TestEntityFakes, TestEntity>("strictContext", strictMode: false);

        Assert.NotNull(entity);
    }

    [Fact]
    public void Generate_WithTypeParameter_ReturnsEntity()
    {
        var entity = FakeGenerator.Generate<TestEntity>(typeof(TestEntityFakes));

        Assert.NotNull(entity);
    }

    [Fact]
    public void Generate_WithTypeAndStrictMode_ReturnsEntity()
    {
        var entity = FakeGenerator.Generate<TestEntity>(typeof(TestEntityFakes), strictMode: false);

        Assert.NotNull(entity);
    }

    [Fact]
    public void Generate_WithTypeAndContext_ReturnsEntity()
    {
        var entity = FakeGenerator.Generate<TestEntity>(typeof(TestEntityFakes), "typeContext");

        Assert.NotNull(entity);
    }

    [Fact]
    public void Generate_WithTypeContextAndStrictMode_ReturnsEntity()
    {
        var entity = FakeGenerator.Generate<TestEntity>(typeof(TestEntityFakes), "typeCtxStrict", strictMode: false);

        Assert.NotNull(entity);
    }

    [Fact]
    public async Task GenerateAsync_Default_ReturnsEntity()
    {
        var entity = await FakeGenerator.GenerateAsync<TestEntityFakes, TestEntity>();

        Assert.NotNull(entity);
        Assert.True(entity.Id != Guid.Empty);
    }

    [Fact]
    public async Task GenerateAsync_WithStrictMode_ReturnsEntity()
    {
        var entity = await FakeGenerator.GenerateAsync<TestEntityFakes, TestEntity>(strictMode: false);

        Assert.NotNull(entity);
    }

    [Fact]
    public async Task GenerateAsync_WithContext_ReturnsEntity()
    {
        var entity = await FakeGenerator.GenerateAsync<TestEntityFakes, TestEntity>("asyncCtx");

        Assert.NotNull(entity);
    }

    [Fact]
    public async Task GenerateAsync_WithContextAndStrictMode_ReturnsEntity()
    {
        var entity = await FakeGenerator.GenerateAsync<TestEntityFakes, TestEntity>("asyncCtxStrict", strictMode: false);

        Assert.NotNull(entity);
    }

    [Fact]
    public async Task GenerateAsync_WithType_ReturnsEntity()
    {
        var entity = await FakeGenerator.GenerateAsync<TestEntity>(typeof(TestEntityFakes));

        Assert.NotNull(entity);
    }

    [Fact]
    public async Task GenerateAsync_WithTypeAndStrictMode_ReturnsEntity()
    {
        var entity = await FakeGenerator.GenerateAsync<TestEntity>(typeof(TestEntityFakes), strictMode: false);

        Assert.NotNull(entity);
    }

    [Fact]
    public async Task GenerateAsync_WithTypeAndContext_ReturnsEntity()
    {
        var entity = await FakeGenerator.GenerateAsync<TestEntity>(typeof(TestEntityFakes), "asyncTypeCtx");

        Assert.NotNull(entity);
    }

    [Fact]
    public async Task GenerateAsync_WithTypeContextAndStrictMode_ReturnsEntity()
    {
        var entity = await FakeGenerator.GenerateAsync<TestEntity>(typeof(TestEntityFakes), "asyncTypeCtxStrict", strictMode: false);

        Assert.NotNull(entity);
    }

    [Fact]
    public void GenerateList_Default_ReturnsNonEmptyList()
    {
        var entities = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>();

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public void GenerateList_WithRepeatableData_ReturnsConsistentList()
    {
        var entities1 = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>(repeatableData: true);
        var entities2 = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>(repeatableData: true);

        Assert.NotNull(entities1);
        Assert.NotNull(entities2);
        Assert.Equal(entities1.Count(), entities2.Count());
    }

    [Fact]
    public void GenerateList_WithRepeatableDataAndStrictMode_ReturnsNonEmptyList()
    {
        var entities = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>(repeatableData: false, strictMode: false);

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public void GenerateList_WithContext_ReturnsNonEmptyList()
    {
        var entities = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>("listCtx");

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public void GenerateList_WithContextAndRepeatable_ReturnsNonEmptyList()
    {
        var entities = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>("listCtxRep", repeatableData: false);

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public void GenerateList_WithContextRepeatableAndStrictMode_ReturnsNonEmptyList()
    {
        var entities = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>("listCtxRepStrict", repeatableData: false, strictMode: false);

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public void GenerateList_WithType_ReturnsNonEmptyList()
    {
        var entities = FakeGenerator.GenerateList<TestEntity>(typeof(TestEntityFakes));

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public void GenerateList_WithTypeAndRepeatable_ReturnsNonEmptyList()
    {
        var entities = FakeGenerator.GenerateList<TestEntity>(typeof(TestEntityFakes), repeatableData: false);

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public void GenerateList_WithTypeRepeatableAndStrictMode_ReturnsNonEmptyList()
    {
        var entities = FakeGenerator.GenerateList<TestEntity>(typeof(TestEntityFakes), repeatableData: false, strictMode: false);

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public void GenerateList_WithTypeAndContext_ReturnsNonEmptyList()
    {
        var entities = FakeGenerator.GenerateList<TestEntity>(typeof(TestEntityFakes), "typeListCtx");

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public void GenerateList_WithTypeContextAndRepeatable_ReturnsNonEmptyList()
    {
        var entities = FakeGenerator.GenerateList<TestEntity>(typeof(TestEntityFakes), "typeListCtxRep", repeatableData: false);

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public void GenerateList_WithTypeContextRepeatableAndStrictMode_ReturnsNonEmptyList()
    {
        var entities = FakeGenerator.GenerateList<TestEntity>(typeof(TestEntityFakes), "typeListCtxRepStrict", repeatableData: false, strictMode: false);

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public void GenerateList_WithCountAndRepeatable_ReturnsExactCount()
    {
        var entities = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>(5, repeatableData: false);

        Assert.NotNull(entities);
        Assert.Equal(5, entities.Count());
    }

    [Fact]
    public void GenerateList_WithCountRepeatableAndStrictMode_ReturnsExactCount()
    {
        var entities = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>(3, repeatableData: false, strictMode: false);

        Assert.Equal(3, entities.Count());
    }

    [Fact]
    public void GenerateList_WithCountAndContext_ReturnsExactCount()
    {
        var entities = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>(4, "countCtx");

        Assert.Equal(4, entities.Count());
    }

    [Fact]
    public void GenerateList_WithCountContextAndRepeatable_ReturnsExactCount()
    {
        var entities = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>(4, "countCtxRep", repeatableData: false);

        Assert.Equal(4, entities.Count());
    }

    [Fact]
    public void GenerateList_WithCountContextRepeatableAndStrictMode_ReturnsExactCount()
    {
        var entities = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>(4, "countCtxRepStrict", repeatableData: false, strictMode: false);

        Assert.Equal(4, entities.Count());
    }

    [Fact]
    public void GenerateList_WithTypeAndCount_ReturnsExactCount()
    {
        var entities = FakeGenerator.GenerateList<TestEntity>(typeof(TestEntityFakes), 5);

        Assert.Equal(5, entities.Count());
    }

    [Fact]
    public void GenerateList_WithTypeCountAndRepeatable_ReturnsExactCount()
    {
        var entities = FakeGenerator.GenerateList<TestEntity>(typeof(TestEntityFakes), 3, repeatableData: false);

        Assert.Equal(3, entities.Count());
    }

    [Fact]
    public void GenerateList_WithTypeCountRepeatableAndStrictMode_ReturnsExactCount()
    {
        var entities = FakeGenerator.GenerateList<TestEntity>(typeof(TestEntityFakes), 3, repeatableData: false, strictMode: false);

        Assert.Equal(3, entities.Count());
    }

    [Fact]
    public void GenerateList_WithTypeCountAndContext_ReturnsExactCount()
    {
        var entities = FakeGenerator.GenerateList<TestEntity>(typeof(TestEntityFakes), 3, "typeCountCtx");

        Assert.Equal(3, entities.Count());
    }

    [Fact]
    public void GenerateList_WithTypeCountContextAndRepeatable_ReturnsExactCount()
    {
        var entities = FakeGenerator.GenerateList<TestEntity>(typeof(TestEntityFakes), 3, "typeCountCtxRep", repeatableData: false);

        Assert.Equal(3, entities.Count());
    }

    [Fact]
    public void GenerateList_WithTypeCountContextRepeatableAndStrictMode_ReturnsExactCount()
    {
        var entities = FakeGenerator.GenerateList<TestEntity>(typeof(TestEntityFakes), 3, "typeCountCtxRepStrict", repeatableData: false, strictMode: false);

        Assert.Equal(3, entities.Count());
    }

    [Fact]
    public async Task GenerateListAsync_Default_ReturnsNonEmptyList()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntityFakes, TestEntity>();

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public async Task GenerateListAsync_WithRepeatableData_ReturnsNonEmptyList()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntityFakes, TestEntity>(repeatableData: true);

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public async Task GenerateListAsync_WithRepeatableDataAndStrictMode_ReturnsNonEmptyList()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntityFakes, TestEntity>(repeatableData: false, strictMode: false);

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public async Task GenerateListAsync_WithContext_ReturnsNonEmptyList()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntityFakes, TestEntity>("asyncListCtx");

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public async Task GenerateListAsync_WithContextAndRepeatable_ReturnsNonEmptyList()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntityFakes, TestEntity>("asyncListCtxRep", repeatableData: false);

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public async Task GenerateListAsync_WithContextRepeatableAndStrictMode_ReturnsNonEmptyList()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntityFakes, TestEntity>("asyncListCtxRepStrict", repeatableData: false, strictMode: false);

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public async Task GenerateListAsync_WithType_ReturnsNonEmptyList()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntity>(typeof(TestEntityFakes));

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public async Task GenerateListAsync_WithTypeAndRepeatable_ReturnsNonEmptyList()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntity>(typeof(TestEntityFakes), repeatableData: false);

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public async Task GenerateListAsync_WithTypeRepeatableAndStrictMode_ReturnsNonEmptyList()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntity>(typeof(TestEntityFakes), repeatableData: false, strictMode: false);

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public async Task GenerateListAsync_WithTypeAndContext_ReturnsNonEmptyList()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntity>(typeof(TestEntityFakes), "asyncTypeListCtx");

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public async Task GenerateListAsync_WithTypeContextAndRepeatable_ReturnsNonEmptyList()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntity>(typeof(TestEntityFakes), "asyncTypeListCtxRep", repeatableData: false);

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public async Task GenerateListAsync_WithTypeContextRepeatableAndStrictMode_ReturnsNonEmptyList()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntity>(typeof(TestEntityFakes), "asyncTypeListCtxRepStrict", repeatableData: false, strictMode: false);

        Assert.NotNull(entities);
        Assert.NotEmpty(entities);
    }

    [Fact]
    public async Task GenerateListAsync_WithCount_ReturnsExactCount()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntityFakes, TestEntity>(5);

        Assert.Equal(5, entities.Count());
    }

    [Fact]
    public async Task GenerateListAsync_WithCountAndRepeatable_ReturnsExactCount()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntityFakes, TestEntity>(3, repeatableData: false);

        Assert.Equal(3, entities.Count());
    }

    [Fact]
    public async Task GenerateListAsync_WithCountRepeatableAndStrictMode_ReturnsExactCount()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntityFakes, TestEntity>(3, repeatableData: false, strictMode: false);

        Assert.Equal(3, entities.Count());
    }

    [Fact]
    public async Task GenerateListAsync_WithCountAndContext_ReturnsExactCount()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntityFakes, TestEntity>(4, "asyncCountCtx");

        Assert.Equal(4, entities.Count());
    }

    [Fact]
    public async Task GenerateListAsync_WithCountContextAndRepeatable_ReturnsExactCount()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntityFakes, TestEntity>(4, "asyncCountCtxRep", repeatableData: false);

        Assert.Equal(4, entities.Count());
    }

    [Fact]
    public async Task GenerateListAsync_WithCountContextRepeatableAndStrictMode_ReturnsExactCount()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntityFakes, TestEntity>(4, "asyncCountCtxRepStrict", repeatableData: false, strictMode: false);

        Assert.Equal(4, entities.Count());
    }

    [Fact]
    public async Task GenerateListAsync_WithTypeAndCount_ReturnsExactCount()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntity>(typeof(TestEntityFakes), 5);

        Assert.Equal(5, entities.Count());
    }

    [Fact]
    public async Task GenerateListAsync_WithTypeCountAndRepeatable_ReturnsExactCount()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntity>(typeof(TestEntityFakes), 3, repeatableData: false);

        Assert.Equal(3, entities.Count());
    }

    [Fact]
    public async Task GenerateListAsync_WithTypeCountRepeatableAndStrictMode_ReturnsExactCount()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntity>(typeof(TestEntityFakes), 3, repeatableData: false, strictMode: false);

        Assert.Equal(3, entities.Count());
    }

    [Fact]
    public async Task GenerateListAsync_WithTypeCountAndContext_ReturnsExactCount()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntity>(typeof(TestEntityFakes), 3, "asyncTypeCountCtx");

        Assert.Equal(3, entities.Count());
    }

    [Fact]
    public async Task GenerateListAsync_WithTypeCountContextAndRepeatable_ReturnsExactCount()
    {
        var entities = await FakeGenerator.GenerateListAsync<TestEntity>(typeof(TestEntityFakes), 3, "asyncTypeCountCtxRep", repeatableData: false);

        Assert.Equal(3, entities.Count());
    }
}
