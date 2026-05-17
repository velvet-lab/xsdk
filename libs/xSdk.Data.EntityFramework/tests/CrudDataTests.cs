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
        IDatalayerFactory factory = fixture.Factory;
        ITestRepository testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);

        var entity = new TestEntity
        {
            Id = Guid.Parse("c0000000-0000-0000-0000-000000000001"),
            Name = "Pippin",
            Age = 18
        };

        if (testRepo is IRepository<TestEntity, Guid> repo)
        {
            bool result = await repo.InsertAsync(entity, TestContext.Current.CancellationToken);
            Assert.True(result);
        }
    }

    [Fact]
    public async Task InsertAsync_Collection_ReturnsCount()
    {
        IDatalayerFactory factory = fixture.Factory;
        ITestRepository testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);

        TestEntity[] entities =
        [
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000010"), Name = "Aragorn", Age = 87 },
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000011"), Name = "Legolas", Age = 2931 },
        ];

        if (testRepo is IRepository<TestEntity, Guid> repo)
        {
            int count = await repo.InsertAsync(entities, TestContext.Current.CancellationToken);
            Assert.Equal(2, count);
        }
    }

    [Fact]
    public async Task SelectListAsync_AfterInsert_ReturnsAllEntities()
    {
        IDatalayerFactory factory = fixture.Factory;
        ITestRepository testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity, Guid>;

        TestEntity[] entities =
        [
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000020"), Name = "Gimli", Age = 139 },
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000021"), Name = "Boromir", Age = 41 },
        ];
        await testRepo.AddDataAsync(entities, TestContext.Current.CancellationToken);

        var all = (await repo!.SelectListAsync(TestContext.Current.CancellationToken))?.ToList();

        if (all != null)
        {
            Assert.Contains(all, x => x.Name == "Gimli");
            Assert.Contains(all, x => x.Name == "Boromir");
        }
        else
        {
            Assert.NotNull(all);
        }
    }

    [Fact]
    public async Task RemoveAsync_ByCollection_RemovesEntities()
    {
        IDatalayerFactory factory = fixture.Factory;
        ITestRepository testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);

        TestEntity[] entities =
        [
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000030"), Name = "Wormtongue", Age = 60 },
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000031"), Name = "Saruman", Age = 1000 },
        ];
        await testRepo.AddDataAsync(entities, TestContext.Current.CancellationToken);

        if (testRepo is IRepository<TestEntity, Guid> repo)
        {
            int removed = await repo.RemoveAsync(entities, TestContext.Current.CancellationToken);
            Assert.Equal(2, removed);

            IEnumerable<TestEntity>? all = await repo.SelectListAsync(TestContext.Current.CancellationToken);
            if (all != null)
            {
                Assert.DoesNotContain(all, x => x.Name == "Wormtongue");
                Assert.DoesNotContain(all, x => x.Name == "Saruman");
            }
        }
    }

    [Fact]
    public async Task UpsertAsync_NewEntity_InsertsAndIsListable()
    {
        IDatalayerFactory factory = fixture.Factory;
        ITestRepository testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);

        var entity = new TestEntity
        {
            Id = Guid.Parse("c0000000-0000-0000-0000-000000000040"),
            Name = "Treebeard",
            Age = 3000
        };

        if (testRepo is IRepository<TestEntity, Guid> repo)
        {
            bool result = await repo.UpsertAsync(entity, TestContext.Current.CancellationToken);
            Assert.True(result);

            IEnumerable<TestEntity>? all = await repo.SelectListAsync(TestContext.Current.CancellationToken);
            if (all != null)
            {
                Assert.Contains(all, x => x.Name == "Treebeard");
            }
        }
    }
}
