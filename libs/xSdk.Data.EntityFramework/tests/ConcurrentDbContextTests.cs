using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xSdk.Data.Mocks;

namespace xSdk.Data;

public class ConcurrentDbContextTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    [Fact]
    public async Task InsertData()
    {
        var factory = fixture.Factory;

        var task1 = Task.Run(async () =>
        {
            var repo1 = factory.CreateRepository<IConcurrentRepositoryOne>(Globals.DatalayerName);
            await repo1.AddDataAsync(Globals.ConcurrentEntitiesOne);
            var entities1 = await repo1.GetDataAsync();

            return entities1;
        });

        var task2 = Task.Run(async () =>
        {
            var repo2 = factory.CreateRepository<IConcurrentRepositoryTwo>(Globals.DatalayerName);
            await repo2.AddDataAsync(Globals.ConcurrentEntitiesTwo);
            var entities2 = await repo2.GetDataAsync();

            return entities2;
        });

        await Task.WhenAll(task1, task2);

        var entities1 = task1.Result;
        var entities2 = task2.Result;

        Assert.NotNull(entities1);
        Assert.Equal(Globals.ConcurrentEntitiesOne.Count(), entities1.Count());

        Assert.NotNull(entities2);
        Assert.Equal(Globals.ConcurrentEntitiesTwo.Count(), entities2.Count());
    }
}
