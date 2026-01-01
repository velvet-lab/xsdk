using xSdk.Data.Mocks;

namespace xSdk.Data
{
    public class InsertDataTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
    {
        [Fact]
        public async Task InsertData()
        {
            var factory = fixture.Factory;
            var repo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);

            await repo.AddDataAsync(Globals.Entities);

            var entities = await repo.GetDataAsync();

            Assert.NotNull(entities);
            Assert.Equal(Globals.Entities.Count(), entities.Count());
        }
    }
}
