using xSdk.Data.Mocks;

namespace xSdk.Data;

public class CrudDataTests(CrudDatabaseFixture fixture) : IClassFixture<CrudDatabaseFixture>
{
    [Fact]
    public async Task InsertAsync_SingleEntity_ReturnsTrue()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity>;

        var entity = new TestEntity
        {
            Id = Guid.Parse("c0000000-0000-0000-0000-000000000001"),
            Name = "Pippin",
            Age = 18
        };

        var result = await repo!.InsertAsync(entity);

        Assert.True(result);
    }

    [Fact]
    public async Task InsertAsync_Collection_ReturnsCount()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity>;

        var entities = new[]
        {
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000010"), Name = "Aragorn", Age = 87 },
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000011"), Name = "Legolas", Age = 2931 },
        };

        var count = await repo!.InsertAsync(entities);

        Assert.Equal(2, count);
    }

    [Fact]
    public async Task SelectListAsync_AfterInsert_ReturnsAllEntities()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity>;

        var entities = new[]
        {
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000020"), Name = "Gimli", Age = 139 },
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000021"), Name = "Boromir", Age = 41 },
        };
        await testRepo.AddDataAsync(entities);

        var all = (await repo!.SelectListAsync()).ToList();

        Assert.Contains(all, x => x.Name == "Gimli");
        Assert.Contains(all, x => x.Name == "Boromir");
    }

    [Fact]
    public async Task RemoveAsync_ByCollection_RemovesEntities()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity>;

        var entities = new[]
        {
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000030"), Name = "Wormtongue", Age = 60 },
            new TestEntity { Id = Guid.Parse("c0000000-0000-0000-0000-000000000031"), Name = "Saruman", Age = 1000 },
        };
        await testRepo.AddDataAsync(entities);

        var removed = await repo!.RemoveAsync(entities);

        Assert.Equal(2, removed);
        var all = (await repo.SelectListAsync()).ToList();
        Assert.DoesNotContain(all, x => x.Name == "Wormtongue");
        Assert.DoesNotContain(all, x => x.Name == "Saruman");
    }

    [Fact]
    public async Task UpsertAsync_NewEntity_InsertsAndIsListable()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity>;

        var entity = new TestEntity
        {
            Id = Guid.Parse("c0000000-0000-0000-0000-000000000040"),
            Name = "Treebeard",
            Age = 3000
        };

        var result = await repo!.UpsertAsync(entity);

        Assert.True(result);
        var all = (await repo.SelectListAsync()).ToList();
        Assert.Contains(all, x => x.Name == "Treebeard");
    }
}
