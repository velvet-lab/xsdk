using xSdk.Hosting;

namespace xSdk.Extensions.Variable
{
    public class SetupTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
    {
        [Fact]
        public void LoadSetup()
        {
            var service = fixture
                .ConfigureServices(services => services.AddVariableServices())
                .GetService<IVariableService>();

            var setup = service.GetSetup<EnvironmentSetup>();

            Assert.NotNull(setup);
        }

        [Fact]
        public void LoadSetupFromInterface()
        {
            var service = fixture
                .ConfigureServices(services => services.AddVariableServices())
                .GetService<IVariableService>();

            var setup = service.GetSetup<IEnvironmentSetup>();

            Assert.NotNull(setup);
        }

        [Fact]
        public void LoadSetupWithoutSlimHost()
        {
            var setup = new EnvironmentSetup();

            setup.AppCompany = "MyCompany";

            Assert.NotNull(setup);
            Assert.Equal("MyCompany", setup.AppCompany);
        }
    }
}
