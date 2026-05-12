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

public class FakeRepositoryTests
{
    private static FakeRepository<TestEntity, Guid> CreateRepository() =>
        new([]);

    [Fact]
    public async Task InsertAsync_SingleEntity_ReturnsTrue()
    {
        FakeRepository<TestEntity, Guid> repo = CreateRepository();
        var entity = new TestEntity { Name = "Alice", Age = 30 };

        bool result = await repo.InsertAsync(entity, TestContext.Current.CancellationToken);

        Assert.True(result);
    }

    [Fact]
    public async Task InsertAsync_SingleEntity_CanBeRetrieved()
    {
        FakeRepository<TestEntity, Guid> repo = CreateRepository();
        var entity = new TestEntity { Name = "Bob", Age = 25 };
        await repo.InsertAsync(entity, TestContext.Current.CancellationToken);

        TestEntity? found = await repo.SelectAsync(entity.Id, TestContext.Current.CancellationToken);

        Assert.NotNull(found);
        Assert.Equal(entity.Name, found.Name);
    }

    [Fact]
    public async Task InsertAsync_MultipleEntities_ReturnsCount()
    {
        FakeRepository<TestEntity, Guid> repo = CreateRepository();
        List<TestEntity> entities =
        [        
            new TestEntity { Name = "Alice", Age = 30 },
            new TestEntity { Name = "Bob", Age = 25 },
            new TestEntity { Name = "Charlie", Age = 35 }
        ];

        int count = await repo.InsertAsync(entities, TestContext.Current.CancellationToken);

        Assert.Equal(3, count);
    }

    [Fact]
    public async Task SelectListAsync_AfterInsert_ReturnsAllEntities()
    {
        FakeRepository<TestEntity, Guid> repo = CreateRepository();
        List<TestEntity> entities =
        [
            new TestEntity { Name = "Alice", Age = 30 },
            new TestEntity { Name = "Bob", Age = 25 }
        ];

        await repo.InsertAsync(entities, TestContext.Current.CancellationToken);

        IEnumerable<TestEntity> result = await repo.SelectListAsync(TestContext.Current.CancellationToken);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task RemoveAsync_ByPrimaryKey_RemovesEntity()
    {
        FakeRepository<TestEntity, Guid> repo = CreateRepository();
        var entity = new TestEntity { Name = "ToRemove", Age = 20 };
        await repo.InsertAsync(entity, TestContext.Current.CancellationToken);

        bool result = await repo.RemoveAsync(entity.Id, TestContext.Current.CancellationToken);
        IEnumerable<TestEntity> all = await repo.SelectListAsync(TestContext.Current.CancellationToken);

        Assert.True(result);
        Assert.Empty(all);
    }

    [Fact]
    public async Task RemoveAsync_ByEntity_RemovesEntity()
    {
        FakeRepository<TestEntity, Guid> repo = CreateRepository();
        var entity = new TestEntity { Name = "ToRemove", Age = 20 };
        await repo.InsertAsync(entity, TestContext.Current.CancellationToken);

        bool result = await repo.RemoveAsync(entity, TestContext.Current.CancellationToken);
        IEnumerable<TestEntity> all = await repo.SelectListAsync(TestContext.Current.CancellationToken);

        Assert.True(result);
        Assert.Empty(all);
    }

    [Fact]
    public async Task UpdateAsync_ExistingEntity_UpdatesValue()
    {
        FakeRepository<TestEntity, Guid> repo = CreateRepository();
        var entity = new TestEntity { Name = "Original", Age = 20 };
        await repo.InsertAsync(entity, TestContext.Current.CancellationToken);
        var updatedEntity = new TestEntity
        {
            Name = "Updated",
            Age = 21,
            Id = entity.Id
        };

        bool updateResult = await repo.UpdateAsync(entity.Id, updatedEntity, TestContext.Current.CancellationToken);
        Assert.True(updateResult);
        IEnumerable<TestEntity> all = await repo.SelectListAsync(TestContext.Current.CancellationToken);
        Assert.Single(all);
        Assert.Equal("Updated", all.First().Name);
    }

    [Fact]
    public async Task UpsertAsync_ExistingEntity_UpdatesValue()
    {
        FakeRepository<TestEntity, Guid> repo = CreateRepository();
        var entity = new TestEntity { Name = "Original", Age = 20 };
        await repo.InsertAsync(entity, TestContext.Current.CancellationToken);
        entity.Name = "Upserted";

        await repo.UpsertAsync(entity, TestContext.Current.CancellationToken);
        TestEntity? found = await repo.SelectAsync(entity.Id, TestContext.Current.CancellationToken);

        Assert.NotNull(found);
        Assert.Equal("Upserted", found.Name);
    }

    [Fact]
    public async Task SelectAsync_WithNonExistentKey_ReturnsNull()
    {
        FakeRepository<TestEntity, Guid> repo = CreateRepository();

        TestEntity? result = await repo.SelectAsync(Guid.NewGuid(), TestContext.Current.CancellationToken);

        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_MultipleEntities_RemovesAll()
    {
        FakeRepository<TestEntity, Guid> repo = CreateRepository();
        List<TestEntity> entities =
        [
            new TestEntity { Name = "Alice", Age = 30 },
            new TestEntity { Name = "Bob", Age = 25 }
        ];
        await repo.InsertAsync(entities, TestContext.Current.CancellationToken);

        int result = await repo.RemoveAsync(entities, TestContext.Current.CancellationToken);
        IEnumerable<TestEntity> all = await repo.SelectListAsync(TestContext.Current.CancellationToken);

        Assert.Equal(2, result);
        Assert.Empty(all);
    }

    [Fact]
    public async Task RemoveAsync_ByPrimaryKeys_RemovesAll()
    {
        FakeRepository<TestEntity, Guid> repo = CreateRepository();
        List<TestEntity> entities =
        [
            new TestEntity { Name = "Alice", Age = 30 },
            new TestEntity { Name = "Bob", Age = 25 }
        ];
        await repo.InsertAsync(entities, TestContext.Current.CancellationToken);
        IEnumerable<Guid> keys = entities.Select(e => e.Id);

        int result = await repo.RemoveAsync(keys, TestContext.Current.CancellationToken);
        IEnumerable<TestEntity> all = await repo.SelectListAsync(TestContext.Current.CancellationToken);
        Assert.Equal(2, result);
        Assert.Empty(all);
    }
}
