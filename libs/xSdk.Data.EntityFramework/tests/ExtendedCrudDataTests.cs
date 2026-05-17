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
        IDatalayerFactory factory = fixture.Factory;
        ITestRepository testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity, Guid>;

        var entity = new TestEntity
        {
            Id = Guid.Parse("e0000000-0000-0000-0000-000000000001"),
            Name = "Merry",
            Age = 36
        };
        await repo!.InsertAsync(entity, TestContext.Current.CancellationToken);

        TestEntity? result = await repo.SelectAsync(entity.Id, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Equal("Merry", result.Name);
    }

    [Fact]
    public async Task SelectAsync_ByNonExistingKey_ReturnsNull()
    {
        IDatalayerFactory factory = fixture.Factory;
        ITestRepository testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity, Guid>;

        var pk = Guid.Parse("e0000000-ffff-ffff-ffff-000000000001");

        TestEntity? result = await repo!.SelectAsync(pk, TestContext.Current.CancellationToken);

        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_ByPrimaryKey_RemovesEntity()
    {
        IDatalayerFactory factory = fixture.Factory;
        ITestRepository testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity, Guid>;

        var entity = new TestEntity
        {
            Id = Guid.Parse("e0000000-0000-0000-0000-000000000003"),
            Name = "Sauron",
            Age = 99999
        };
        await repo!.InsertAsync(entity, TestContext.Current.CancellationToken);

        bool removed = await repo.RemoveAsync(entity.Id, TestContext.Current.CancellationToken);
        Assert.True(removed);

        TestEntity? result = await repo.SelectAsync(entity.Id, TestContext.Current.CancellationToken);
        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_ByEntity_RemovesEntity()
    {
        IDatalayerFactory factory = fixture.Factory;
        ITestRepository testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity, Guid>;

        var entity = new TestEntity
        {
            Id = Guid.Parse("e0000000-0000-0000-0000-000000000004"),
            Name = "Shelob",
            Age = 5000
        };
        await repo!.InsertAsync(entity, TestContext.Current.CancellationToken);

        bool removed = await repo.RemoveAsync(entity, TestContext.Current.CancellationToken);
        Assert.True(removed);
    }

    [Fact]
    public async Task SelectListAsync_AfterMultipleInserts_ReturnsAll()
    {
        IDatalayerFactory factory = fixture.Factory;
        ITestRepository testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);

        TestEntity[] entities =
        [
            new TestEntity { Id = Guid.Parse("e0000000-0000-0000-0000-000000000010"), Name = "Eowyn", Age = 24 },
            new TestEntity { Id = Guid.Parse("e0000000-0000-0000-0000-000000000011"), Name = "Theoden", Age = 71 },
        ];
        await testRepo.AddDataAsync(entities, TestContext.Current.CancellationToken);

        if (testRepo is IRepository<TestEntity, Guid> repo)
        {
            IEnumerable<TestEntity>? all = await repo.SelectListAsync(TestContext.Current.CancellationToken);
            if (all != null)
            {
                Assert.Contains(all, x => x.Name == "Eowyn");
                Assert.Contains(all, x => x.Name == "Theoden");
            }
            else
            {
                Assert.Null(all);
            }
        }
    }

    [Fact]
    public async Task RemoveAsync_ByPrimaryKeyCollection_ThrowsNotImplementedException()
    {
        IDatalayerFactory factory = fixture.Factory;
        ITestRepository testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity, Guid>;

        Guid[] primaryKeys = [(Guid.NewGuid())];

        await Assert.ThrowsAsync<NotImplementedException>(() => repo!.RemoveAsync(primaryKeys, TestContext.Current.CancellationToken));
    }
}
