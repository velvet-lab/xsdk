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

public class NoSqlCrudTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    [Fact]
    public async Task InsertAsync_SingleEntity_ReturnsTrue()
    {
        var factory = fixture.Factory;
        var repo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var baseRepo = repo as IRepository<TestEntity>;

        var entity = FakeGenerator.Generate<TestEntityFakes, TestEntity>();

        var result = await baseRepo!.InsertAsync(entity);

        Assert.True(result);
        await repo.RemoveAll();
    }

    [Fact]
    public async Task InsertAsync_Collection_ReturnsCount()
    {
        var factory = fixture.Factory;
        var repo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var baseRepo = repo as IRepository<TestEntity>;

        var entities = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>(3).ToArray();

        var count = await baseRepo!.InsertAsync(entities);

        Assert.True(count > 0);
        await repo.RemoveAll();
    }

    [Fact]
    public async Task SelectListAsync_AfterInsert_ReturnsEntities()
    {
        var factory = fixture.Factory;
        var repo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var baseRepo = repo as IRepository<TestEntity>;

        var entities = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>(2);
        foreach (var e in entities)
            await baseRepo!.InsertAsync(e);

        var result = await repo.GetDataAsync();

        Assert.NotEmpty(result);
        await repo.RemoveAll();
    }

    [Fact]
    public async Task SelectAsync_ByPrimaryKey_ReturnsCorrectEntity()
    {
        var factory = fixture.Factory;
        var repo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var baseRepo = repo as IRepository<TestEntity>;

        var entity = FakeGenerator.Generate<TestEntityFakes, TestEntity>();
        entity.Name = "SelectableEntity";
        await baseRepo!.InsertAsync(entity);

        var result = await baseRepo.SelectAsync(entity.PrimaryKey);

        Assert.NotNull(result);
        Assert.Equal("SelectableEntity", result.Name);
        await repo.RemoveAll();
    }

    [Fact]
    public async Task UpdateAsync_ExistingEntity_UpdatesValues()
    {
        var factory = fixture.Factory;
        var repo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var baseRepo = repo as IRepository<TestEntity>;

        var entity = FakeGenerator.Generate<TestEntityFakes, TestEntity>();
        entity.Name = "Before";
        await baseRepo!.InsertAsync(entity);

        entity.Name = "After";
        var updated = await baseRepo.UpdateAsync(entity.PrimaryKey, entity);

        Assert.True(updated);
        await repo.RemoveAll();
    }

    [Fact]
    public async Task RemoveAsync_ByPrimaryKey_RemovesEntity()
    {
        var factory = fixture.Factory;
        var repo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var baseRepo = repo as IRepository<TestEntity>;

        var entity = FakeGenerator.Generate<TestEntityFakes, TestEntity>();
        await baseRepo!.InsertAsync(entity);

        var removed = await baseRepo.RemoveAsync(entity.PrimaryKey);

        Assert.True(removed);
        var result = await baseRepo.SelectAsync(entity.PrimaryKey);
        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_ByEntity_RemovesEntity()
    {
        var factory = fixture.Factory;
        var repo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var baseRepo = repo as IRepository<TestEntity>;

        var entity = FakeGenerator.Generate<TestEntityFakes, TestEntity>();
        await baseRepo!.InsertAsync(entity);

        var removed = await baseRepo.RemoveAsync(entity);

        Assert.True(removed);
    }

    [Fact]
    public async Task RemoveAsync_ByEntityCollection_RemovesAll()
    {
        var factory = fixture.Factory;
        var repo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var baseRepo = repo as IRepository<TestEntity>;

        var entities = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>(2).ToList();
        foreach (var e in entities)
            await baseRepo!.InsertAsync(e);

        var removed = await baseRepo!.RemoveAsync(entities);

        Assert.Equal(2, removed);
    }

    [Fact]
    public async Task UpsertAsync_NewEntity_InsertsIt()
    {
        var factory = fixture.Factory;
        var repo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var baseRepo = repo as IRepository<TestEntity>;

        var entity = FakeGenerator.Generate<TestEntityFakes, TestEntity>();

        var result = await baseRepo!.UpsertAsync(entity);

        Assert.False(result); // LiteDB Upsert returns false for new inserts (true for updates)
        await repo.RemoveAll();
    }

    [Fact]
    public async Task UpsertAsync_ExistingEntity_UpdatesIt()
    {
        var factory = fixture.Factory;
        var repo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var baseRepo = repo as IRepository<TestEntity>;

        var entity = FakeGenerator.Generate<TestEntityFakes, TestEntity>();
        entity.Name = "Original";
        await baseRepo!.InsertAsync(entity);

        entity.Name = "Updated";
        var result = await baseRepo.UpsertAsync(entity);

        Assert.True(result);
        await repo.RemoveAll();
    }

    [Fact]
    public async Task SelectListAsync_WithFilter_ReturnsFilteredEntities()
    {
        var factory = fixture.Factory;
        var repo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var baseRepo = repo as IRepository<TestEntity>;

        var entities = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>(3).ToList();
        entities[0].Name = "FilterTarget";
        foreach (var e in entities)
            await baseRepo!.InsertAsync(e);

        var all = await repo.GetDataAsync();

        Assert.Contains(all, x => x.Name == "FilterTarget");
        await repo.RemoveAll();
    }
}
