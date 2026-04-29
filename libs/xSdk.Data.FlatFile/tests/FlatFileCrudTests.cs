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

public class FlatFileCrudTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    private ITestRepository CreateRepo() =>
        fixture.Factory.CreateRepository<ITestRepository>(Globals.DatalayerName);

    [Fact]
    public async Task SelectListAsync_EmptyStore_ReturnsEmptyCollection()
    {
        var repo = CreateRepo();
        await repo.RemoveAll();

        Assert.NotNull(repo);
        var result = await repo.GetDataAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task InsertAsync_SingleEntity_CanBeSelected()
    {
        var repo = CreateRepo();
        var entity = new TestEntity { Name = "Alice", Age = 30 };

        var inserted = await repo.InsertAsync(entity);
        Assert.True(inserted);

        var result = await repo.SelectAsync(entity.Id);
        Assert.NotNull(result);
        Assert.Equal("Alice", result.Name);
    }

    [Fact]
    public async Task InsertAsync_MultipleEntities_AllInserted()
    {
        var repo = CreateRepo();
        var entities = new List<TestEntity>
        {
            new TestEntity { Name = "Bob", Age = 25 },
            new TestEntity { Name = "Carol", Age = 35 },
        };

        var count = await repo.InsertAsync(entities);
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task UpdateAsync_Entity_ReflectsChange()
    {
        var repo = CreateRepo();
        var entity = new TestEntity { Name = "Dave", Age = 40 };
        await repo.InsertAsync(entity);

        entity.Name = "Dave Updated";
        var updated = await repo.UpdateAsync(entity.Id, entity);

        Assert.True(updated);
        var result = await repo.SelectAsync(entity.Id);
        Assert.Equal("Dave Updated", result?.Name);
    }

    [Fact]
    public async Task RemoveAsync_ByEntity_IsRemoved()
    {
        var repo = CreateRepo();
        var entity = new TestEntity { Name = "Eve", Age = 22 };
        await repo.InsertAsync(entity);

        var removed = await repo.RemoveAsync(entity);
        Assert.True(removed);

        var result = await repo.SelectAsync(entity.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_ByPrimaryKey_IsRemoved()
    {
        var repo = CreateRepo();
        var entity = new TestEntity { Name = "Frank", Age = 28 };
        await repo.InsertAsync(entity);

        var removed = await repo.RemoveAsync(entity.Id);
        Assert.True(removed);
    }

    [Fact]
    public async Task UpsertAsync_NewEntity_IsInserted()
    {
        var repo = CreateRepo();
        var entity = new TestEntity { Name = "Grace", Age = 33 };

        var result = await repo.UpsertAsync(entity);
        Assert.True(result);
    }

    [Fact]
    public async Task UpsertAsync_ExistingEntity_IsUpdated()
    {
        var repo = CreateRepo();
        var entity = new TestEntity { Name = "Hank", Age = 44 };
        await repo.InsertAsync(entity);

        entity.Age = 55;
        var result = await repo.UpsertAsync(entity);
        Assert.True(result);

        var updated = await repo.SelectAsync(entity.Id);
        Assert.Equal(55, updated?.Age);
    }

    [Fact]
    public async Task RemoveAsync_MultipleEntities_AllRemoved()
    {
        var repo = CreateRepo();
        var e1 = new TestEntity { Name = "Irene", Age = 11 };
        var e2 = new TestEntity { Name = "John", Age = 12 };
        await repo.InsertAsync(new[] { e1, e2 });

        var removed = await repo.RemoveAsync(new[] { e1, e2 });
        Assert.Equal(2, removed);
    }
}
