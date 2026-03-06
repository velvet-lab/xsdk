namespace xSdk.Data
{
    public class GuidStringPKTests
    {
        [Fact]
        public void CreateNewPrimaryKey()
        {
            GuidStringPK primaryKey = new GuidStringPK();

            Assert.NotNull(primaryKey);
            Assert.IsType<string>(primaryKey.GetValue());
        }

        [Fact]
        public void CreateNewPrimaryKeyFromGuid()
        {
            var id = Guid.NewGuid();
            var expected = id.ToString();
            GuidStringPK primaryKey = new GuidStringPK(id);

            Assert.NotNull(primaryKey);
            Assert.Equal(expected, primaryKey.GetValue<string>());
            Assert.IsType<string>(primaryKey.GetValue());
        }

        [Fact]
        public void CreateNewPrimaryKeyFromString()
        {
            var id = Guid.NewGuid();
            var expected = id.ToString();

            GuidStringPK primaryKey = new GuidStringPK(expected);

            Assert.NotNull(primaryKey);
            Assert.Equal(expected, Guid.Parse(primaryKey.GetValue<string>()).ToString());
            Assert.IsType<string>(primaryKey.GetValue());
        }
    }
}
