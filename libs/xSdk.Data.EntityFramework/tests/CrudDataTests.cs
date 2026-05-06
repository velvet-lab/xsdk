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

public class CrudDataTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    [Fact]
    public async Task InsertAsync_SingleEntity_ReturnsTrue()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity, Guid>;

        var entity = new TestEntity
        {
            Id = Guid.Parse("c0000000-0000-0000-0000-000000000001"),
            Name = "Pippin",
            Age = 18
        };

        var result = await repo.InsertAsync(entity, TestContext.Current.CancellationToken);

        Assert.True(result);
    }

    [Fact]
    public async Task InsertAsync_Collection_ReturnsCount()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity, Guid>;

        var entities = new[]
        {
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000010"), Name = "Aragorn", Age = 87 },
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000011"), Name = "Legolas", Age = 2931 },
        };

        var count = await repo.InsertAsync(entities, TestContext.Current.CancellationToken);

        Assert.Equal(2, count);
    }

    [Fact]
    public async Task SelectListAsync_AfterInsert_ReturnsAllEntities()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity, Guid>;

        var entities = new[]
        {
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000020"), Name = "Gimli", Age = 139 },
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000021"), Name = "Boromir", Age = 41 },
        };
        await testRepo.AddDataAsync(entities, TestContext.Current.CancellationToken);

        var all = (await repo!.SelectListAsync(TestContext.Current.CancellationToken)).ToList();

        Assert.Contains(all, x => x.Name == "Gimli");
        Assert.Contains(all, x => x.Name == "Boromir");
    }

    [Fact]
    public async Task RemoveAsync_ByCollection_RemovesEntities()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity, Guid>;

        var entities = new[]
        {
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000030"), Name = "Wormtongue", Age = 60 },
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000031"), Name = "Saruman", Age = 1000 },
        };
        await testRepo.AddDataAsync(entities, TestContext.Current.CancellationToken);

        var removed = await repo!.RemoveAsync(entities, TestContext.Current.CancellationToken);

        Assert.Equal(2, removed);
        var all = (await repo.SelectListAsync(TestContext.Current.CancellationToken)).ToList();
        Assert.DoesNotContain(all, x => x.Name == "Wormtongue");
        Assert.DoesNotContain(all, x => x.Name == "Saruman");
    }

    [Fact]
    public async Task UpsertAsync_NewEntity_InsertsAndIsListable()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity, Guid>;

        var entity = new TestEntity
        {
            Id = Guid.Parse("c0000000-0000-0000-0000-000000000040"),
            Name = "Treebeard",
            Age = 3000
        };

        var result = await repo!.UpsertAsync(entity, TestContext.Current.CancellationToken);

        Assert.True(result);
        var all = (await repo.SelectListAsync(TestContext.Current.CancellationToken)).ToList();
        Assert.Contains(all, x => x.Name == "Treebeard");
    }
}
