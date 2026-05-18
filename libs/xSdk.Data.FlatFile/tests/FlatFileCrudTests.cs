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
        ITestRepository repo = CreateRepo();
        await repo.RemoveAll();

        Assert.NotNull(repo);
        IEnumerable<TestEntity>? result = await repo.GetDataAsync(TestContext.Current.CancellationToken);

        if (result is not null)
        {
            Assert.Empty(result);
        }
        else
        {
            Assert.NotNull(result);
        }
    }

    [Fact]
    public async Task InsertAsync_SingleEntity_CanBeSelected()
    {
        ITestRepository repo = CreateRepo();
        var entity = new TestEntity { Name = "Alice", Age = 30 };

        bool inserted = await repo.InsertAsync(entity, TestContext.Current.CancellationToken);
        Assert.True(inserted);

        TestEntity? result = await repo.SelectAsync(entity.Id, TestContext.Current.CancellationToken);
        Assert.NotNull(result);
        Assert.Equal("Alice", result.Name);
    }

    [Fact]
    public async Task InsertAsync_MultipleEntities_AllInserted()
    {
        ITestRepository repo = CreateRepo();
        var entities = new List<TestEntity>
        {
            new() { Name = "Bob", Age = 25 },
            new() { Name = "Carol", Age = 35 },
        };

        int count = await repo.InsertAsync(entities, TestContext.Current.CancellationToken);
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task UpdateAsync_Entity_ReflectsChange()
    {
        ITestRepository repo = CreateRepo();
        var entity = new TestEntity { Name = "Dave", Age = 40 };
        await repo.InsertAsync(entity, TestContext.Current.CancellationToken);

        entity.Name = "Dave Updated";
        bool updated = await repo.UpdateAsync(entity.Id, entity, TestContext.Current.CancellationToken);

        Assert.True(updated);
        TestEntity? result = await repo.SelectAsync(entity.Id, TestContext.Current.CancellationToken);
        Assert.Equal("Dave Updated", result?.Name);
    }

    [Fact]
    public async Task RemoveAsync_ByEntity_IsRemoved()
    {
        ITestRepository repo = CreateRepo();
        var entity = new TestEntity { Name = "Eve", Age = 22 };
        await repo.InsertAsync(entity, TestContext.Current.CancellationToken);

        bool removed = await repo.RemoveAsync(entity, TestContext.Current.CancellationToken);
        Assert.True(removed);

        TestEntity? result = await repo.SelectAsync(entity.Id, TestContext.Current.CancellationToken);
        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_ByPrimaryKey_IsRemoved()
    {
        ITestRepository repo = CreateRepo();
        var entity = new TestEntity { Name = "Frank", Age = 28 };
        await repo.InsertAsync(entity, TestContext.Current.CancellationToken);

        bool removed = await repo.RemoveAsync(entity.Id, TestContext.Current.CancellationToken);
        Assert.True(removed);
    }

    [Fact]
    public async Task UpsertAsync_NewEntity_IsInserted()
    {
        ITestRepository repo = CreateRepo();
        var entity = new TestEntity { Name = "Grace", Age = 33 };

        bool result = await repo.UpsertAsync(entity, TestContext.Current.CancellationToken);
        Assert.True(result);
    }

    [Fact]
    public async Task UpsertAsync_ExistingEntity_IsUpdated()
    {
        ITestRepository repo = CreateRepo();
        var entity = new TestEntity { Name = "Hank", Age = 44 };
        await repo.InsertAsync(entity, TestContext.Current.CancellationToken);

        entity.Age = 55;
        bool result = await repo.UpsertAsync(entity, TestContext.Current.CancellationToken);
        Assert.True(result);

        TestEntity? updated = await repo.SelectAsync(entity.Id, TestContext.Current.CancellationToken);
        Assert.Equal(55, updated?.Age);
    }

    [Fact]
    public async Task RemoveAsync_MultipleEntities_AllRemoved()
    {
        ITestRepository repo = CreateRepo();
        var e1 = new TestEntity { Name = "Irene", Age = 11 };
        var e2 = new TestEntity { Name = "John", Age = 12 };
        await repo.InsertAsync([e1, e2], TestContext.Current.CancellationToken);

        int removed = await repo.RemoveAsync([e1, e2], TestContext.Current.CancellationToken);
        Assert.Equal(2, removed);
    }

    [Fact]
    public async Task RemoveAsync_NullEntities_ReturnsZero()
    {
        ITestRepository repo = CreateRepo();

        int removed = await ((IRepository<TestEntity, int>)repo).RemoveAsync(
            (IEnumerable<TestEntity>?)null, TestContext.Current.CancellationToken);

        Assert.Equal(0, removed);
    }

    [Fact]
    public async Task RemoveAsync_ByPrimaryKeys_ThrowsNotImplementedException()
    {
        ITestRepository repo = CreateRepo();
        var entity = new TestEntity { Name = "Kevin", Age = 50 };
        await repo.InsertAsync(entity, TestContext.Current.CancellationToken);

        await Assert.ThrowsAsync<NotImplementedException>(
            () => ((IRepository<TestEntity, int>)repo).RemoveAsync(
                [entity.Id], TestContext.Current.CancellationToken));
    }
}
