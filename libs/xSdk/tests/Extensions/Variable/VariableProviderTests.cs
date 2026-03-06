using xSdk.Extensions.Variable.Fakes;
using xSdk.Hosting;

namespace xSdk.Extensions.Variable
{
    public class VariableProviderTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
    {
        [Fact]
        public void RegisterVariableProvider()
        {
            var service = fixture
                .ConfigureServices(services => services.AddVariableServices())
                .GetService<IVariableService>();

            var ex = Record.Exception(() => service.RegisterProvider(typeof(TestVariableProvider)));

            Assert.Null(ex);
        }
    }
}
