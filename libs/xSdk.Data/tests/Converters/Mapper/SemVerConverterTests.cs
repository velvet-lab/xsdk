namespace xSdk.Data.Converters.Mapper
{
    public class SemVerConverterTests
    {
        private readonly SemVer Version = new SemVer("1.2.3");

        private readonly string VersionString = "1.2.3";

        [Fact]
        public void ConvertSemVerToString()
        {
            var converter = new SemVerConverter.ToModelProperty();
            var actual = converter.Convert(Version, default);

            Assert.NotNull(actual);
            Assert.IsType<string>(actual);
        }

        [Fact]
        public void ConvertStringToSemVer()
        {
            var converter = new SemVerConverter.ToEntityProperty();
            var actual = converter.Convert(VersionString, default);

            Assert.NotNull(actual);
            Assert.IsType<SemVer>(actual);
        }
    }
}
