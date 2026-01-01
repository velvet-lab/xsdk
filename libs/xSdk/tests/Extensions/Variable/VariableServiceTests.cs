using xSdk.Extensions.Variable.Fakes;
using xSdk.Hosting;

namespace xSdk.Extensions.Variable
{
    public class VariableServiceTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
    {
        [Fact]
        public void LoadServiceWithVariables()
        {
            var service = fixture
                .ConfigureServices(services => services.AddVariableServices())
                .GetService<IVariableService>();

            service.RegisterSetup<EnvironmentSetup>();

            Assert.NotNull(service);
            Assert.NotEmpty(service.Variables);
        }

        [Fact]
        public void LoadEnvironemtVariablesWithNoValidationErrors()
        {
            var service = fixture
                .ConfigureServices(services => services.AddVariableServices())
                .GetService<IVariableService>();

            Assert.NotNull(service);
            service.RegisterSetup<SetupWithNoPrefix>();

            Assert.NotEmpty(service.Variables);
            var setup = service.GetSetup<SetupWithNoPrefix>(false);
            Assert.NotNull(setup);
        }
    }
}
