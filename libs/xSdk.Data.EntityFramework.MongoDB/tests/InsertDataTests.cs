using xSdk.Data.Mocks;

namespace xSdk.Data
{
#if NET8_0
    public class InsertDataTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
    {
        [Fact]
        public async Task InsertData()
        {
            var factory = fixture.Factory;
            var repo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);

            var fakes = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>(10);
            await repo.AddDataAsync(fakes);

            var entities = await repo.GetDataAsync();

            Assert.NotNull(entities);
            Assert.Equal(fakes.Count(), entities.Count());

            await repo.RemoveAll();
        }
    }
#endif
}
