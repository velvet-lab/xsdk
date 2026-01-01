using xSdk.Hosting;

namespace xSdk.Extensions.Variable
{
    public class VariableTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
    {
        private const string PREFIX = "MyPrefix";
        private const string NAME = "my_variable";

        private const string PREFIX_SEPERATOR = xSdk.Extensions.Variable.Globals.Constants.PREFIX_SEPERATOR;
        private const string SEPERATOR = xSdk.Extensions.Variable.Globals.Constants.VARIABLE_SEPERATOR;

        [Fact]
        public void CreateVariable()
        {
            var name = "my_variable";
            var variable = Variable.Create(name, typeof(string));

            Assert.NotNull(variable);
            Assert.Equal(name, variable.Name);
            Assert.Equal(typeof(string), variable.ValueType);
        }

        [Fact]
        public void CreateVariableWithPrefix()
        {
            var variable = Variable.Create(NAME, typeof(string));

            variable.SetPrefix(PREFIX);

            Assert.NotNull(variable);
            Assert.Equal(PREFIX.ToLower(), variable.Prefix);

            Assert.Equal($"{fixture.AppPrefix}{PREFIX_SEPERATOR}{PREFIX}{PREFIX_SEPERATOR}{NAME}{PREFIX_SEPERATOR}FILE".ToUpper(), variable.KeyForFile);
            Assert.Equal($"--{PREFIX}{SEPERATOR}{NAME}".Replace(SEPERATOR, "-").ToLower(), variable.KeyForCommandline);
        }

        [Fact]
        public void CreateVariableWithDisabledPrefix()
        {
            var variable = Variable.Create(NAME, typeof(string));

            variable.SetPrefix(PREFIX).DisablePrefix();

            Assert.NotNull(variable);
            Assert.Equal(PREFIX.ToLower(), variable.Prefix);

            Assert.Equal($"{NAME}{PREFIX_SEPERATOR}FILE".ToUpper(), variable.KeyForFile);
            Assert.Equal($"--{NAME}".Replace(SEPERATOR, "-").ToLower(), variable.KeyForCommandline);
        }
    }
}
