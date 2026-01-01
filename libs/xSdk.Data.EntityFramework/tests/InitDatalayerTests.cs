using xSdk.Data.Mocks;

namespace xSdk.Data
{
    public class InitDatalayerTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
    {
        [Fact]
        public void CreateDatalayer()
        {
            var factory = fixture.Factory;
            var repo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);

            Assert.NotNull(factory);
            Assert.NotNull(repo);
        }
    }
}
