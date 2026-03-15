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
    private static FakeRepository<TestEntity> CreateRepository() =>
        new FakeRepository<TestEntity>(new List<TestEntity>());

    [Fact]
    public async Task InsertAsync_SingleEntity_ReturnsTrue()
    {
        var repo = CreateRepository();
        var entity = new TestEntity { Name = "Alice", Age = 30 };

        var result = await repo.InsertAsync(entity);

        Assert.True(result);
    }

    [Fact]
    public async Task InsertAsync_SingleEntity_CanBeRetrieved()
    {
        var repo = CreateRepository();
        var entity = new TestEntity { Name = "Bob", Age = 25 };
        await repo.InsertAsync(entity);

        var found = await repo.SelectAsync(entity.PrimaryKey);

        Assert.NotNull(found);
        Assert.Equal(entity.Name, found.Name);
    }

    [Fact]
    public async Task InsertAsync_MultipleEntities_ReturnsCount()
    {
        var repo = CreateRepository();
        var entities = new List<TestEntity>
        {
            new TestEntity { Name = "Alice", Age = 30 },
            new TestEntity { Name = "Bob", Age = 25 },
            new TestEntity { Name = "Charlie", Age = 35 }
        };

        var count = await repo.InsertAsync(entities);

        Assert.Equal(3, count);
    }

    [Fact]
    public async Task SelectListAsync_AfterInsert_ReturnsAllEntities()
    {
        var repo = CreateRepository();
        var entities = new List<TestEntity>
        {
            new TestEntity { Name = "Alice", Age = 30 },
            new TestEntity { Name = "Bob", Age = 25 }
        };
        await repo.InsertAsync(entities);

        var result = await repo.SelectListAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task RemoveAsync_ByPrimaryKey_RemovesEntity()
    {
        var repo = CreateRepository();
        var entity = new TestEntity { Name = "ToRemove", Age = 20 };
        await repo.InsertAsync(entity);

        var result = await repo.RemoveAsync(entity.PrimaryKey);
        var all = await repo.SelectListAsync();

        Assert.True(result);
        Assert.Empty(all);
    }

    [Fact]
    public async Task RemoveAsync_ByEntity_RemovesEntity()
    {
        var repo = CreateRepository();
        var entity = new TestEntity { Name = "ToRemove", Age = 20 };
        await repo.InsertAsync(entity);

        var result = await repo.RemoveAsync(entity);
        var all = await repo.SelectListAsync();

        Assert.True(result);
        Assert.Empty(all);
    }

    [Fact]
    public async Task UpdateAsync_ExistingEntity_UpdatesValue()
    {
        var repo = CreateRepository();
        var entity = new TestEntity { Name = "Original", Age = 20 };
        await repo.InsertAsync(entity);
        var updatedEntity = new TestEntity { Name = "Updated", Age = 21 };
        updatedEntity.Id = entity.Id;

        var updateResult = await repo.UpdateAsync(entity.PrimaryKey, updatedEntity);

        Assert.True(updateResult);
        var all = await repo.SelectListAsync();
        Assert.Single(all);
        Assert.Equal("Updated", all.First().Name);
    }

    [Fact]
    public async Task UpsertAsync_ExistingEntity_UpdatesValue()
    {
        var repo = CreateRepository();
        var entity = new TestEntity { Name = "Original", Age = 20 };
        await repo.InsertAsync(entity);
        entity.Name = "Upserted";

        await repo.UpsertAsync(entity);
        var found = await repo.SelectAsync(entity.PrimaryKey);

        Assert.NotNull(found);
        Assert.Equal("Upserted", found.Name);
    }

    [Fact]
    public async Task SelectAsync_WithNonExistentKey_ReturnsNull()
    {
        var repo = CreateRepository();
        var nonExistentKey = new GuidPK();

        var result = await repo.SelectAsync(nonExistentKey);

        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_MultipleEntities_RemovesAll()
    {
        var repo = CreateRepository();
        var entities = new List<TestEntity>
        {
            new TestEntity { Name = "Alice", Age = 30 },
            new TestEntity { Name = "Bob", Age = 25 }
        };
        await repo.InsertAsync(entities);

        var result = await repo.RemoveAsync(entities);
        var all = await repo.SelectListAsync();

        Assert.Equal(2, result);
        Assert.Empty(all);
    }

    [Fact]
    public async Task RemoveAsync_ByPrimaryKeys_RemovesAll()
    {
        var repo = CreateRepository();
        var entities = new List<TestEntity>
        {
            new TestEntity { Name = "Alice", Age = 30 },
            new TestEntity { Name = "Bob", Age = 25 }
        };
        await repo.InsertAsync(entities);
        var keys = entities.Select(e => e.PrimaryKey);

        var result = await repo.RemoveAsync(keys);
        var all = await repo.SelectListAsync();

        Assert.Equal(2, result);
        Assert.Empty(all);
    }
}
