namespace xSdk.Data.Converters.Mapper
{
    public class SemVerConverterTests
    {
        private readonly SemVer Version = new SemVer("1.2.3");

        private readonly string VersionString = "1.2.3";

        [Fact]
        public void ConvertSemVerToString()
        {
            var actual = SemVerConverter.Convert(Version);

            Assert.NotNull(actual);
            Assert.IsType<string>(actual);
        }

        [Fact]
        public void ConvertStringToSemVer()
        {
            var actual = SemVerConverter.Convert(VersionString);

            Assert.NotNull(actual);
            Assert.IsType<SemVer>(actual);
        }
    }
}
