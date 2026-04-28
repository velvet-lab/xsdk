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

public class ExtendedCrudDataTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    [Fact]
    public async Task SelectAsync_ByPrimaryKey_ReturnsCorrectEntity()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity, Guid>;

        var entity = new TestEntity
        {
            Id = Guid.Parse("e0000000-0000-0000-0000-000000000001"),
            Name = "Merry",
            Age = 36
        };
        await repo!.InsertAsync(entity);

        var result = await repo.SelectAsync(entity.Id);

        Assert.NotNull(result);
        Assert.Equal("Merry", result.Name);
    }

    [Fact]
    public async Task SelectAsync_ByNonExistingKey_ReturnsNull()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity, Guid>;

        var pk = Guid.Parse("e0000000-ffff-ffff-ffff-000000000001");

        var result = await repo!.SelectAsync(pk);

        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_ByPrimaryKey_RemovesEntity()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity, Guid>;

        var entity = new TestEntity
        {
            Id = Guid.Parse("e0000000-0000-0000-0000-000000000003"),
            Name = "Sauron",
            Age = 99999
        };
        await repo!.InsertAsync(entity);

        var removed = await repo.RemoveAsync(entity.Id);

        Assert.True(removed);
        var result = await repo.SelectAsync(entity.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_ByEntity_RemovesEntity()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity, Guid>;

        var entity = new TestEntity
        {
            Id = Guid.Parse("e0000000-0000-0000-0000-000000000004"),
            Name = "Shelob",
            Age = 5000
        };
        await repo!.InsertAsync(entity);

        var removed = await repo.RemoveAsync(entity);

        Assert.True(removed);
    }

    [Fact]
    public async Task SelectListAsync_AfterMultipleInserts_ReturnsAll()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity, Guid>;

        var entities = new[]
        {
            new TestEntity { Id = Guid.Parse("e0000000-0000-0000-0000-000000000010"), Name = "Eowyn", Age = 24 },
            new TestEntity { Id = Guid.Parse("e0000000-0000-0000-0000-000000000011"), Name = "Theoden", Age = 71 },
        };
        await testRepo.AddDataAsync(entities);

        var all = (await repo!.SelectListAsync()).ToList();

        Assert.Contains(all, x => x.Name == "Eowyn");
        Assert.Contains(all, x => x.Name == "Theoden");
    }

    [Fact]
    public async Task RemoveAsync_ByPrimaryKeyCollection_ThrowsNotImplementedException()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity, Guid>;

        var primaryKeys = new[] { (Guid.NewGuid()) };

        await Assert.ThrowsAsync<NotImplementedException>(() => repo!.RemoveAsync(primaryKeys));
    }
}
