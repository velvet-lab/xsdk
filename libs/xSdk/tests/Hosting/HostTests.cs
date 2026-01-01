namespace xSdk.Hosting
{
    public class HostTests
    {
        [Fact]
        public void CreateBuilder()
        {
            var builder = Host.CreateBuilder(null);

            Assert.NotNull(builder);
        }
    }
}
