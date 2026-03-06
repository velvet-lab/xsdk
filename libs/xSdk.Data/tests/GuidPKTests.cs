namespace xSdk.Data
{
    public class GuidPKTests
    {
        [Fact]
        public void CreateNewPrimaryKey()
        {
            GuidPK primaryKey = new GuidPK();

            Assert.NotNull(primaryKey);
            Assert.IsType<Guid>(primaryKey.GetValue());
        }

        [Fact]
        public void CreateNewPrimaryKeyFromGuid()
        {
            var id = Guid.NewGuid();
            var expected = id.ToString();
            GuidPK primaryKey = new GuidPK(id);

            Assert.NotNull(primaryKey);
            Assert.Equal(expected, primaryKey.GetValue().ToString());
            Assert.IsType<Guid>(primaryKey.GetValue());
        }

        [Fact]
        public void CreateNewPrimaryKeyFromString()
        {
            var id = Guid.NewGuid();
            var expected = id.ToString();

            GuidPK primaryKey = new GuidPK(expected);

            Assert.NotNull(primaryKey);
            Assert.Equal(expected, Guid.Parse(primaryKey.GetValue().ToString()).ToString());
            Assert.IsType<Guid>(primaryKey.GetValue());
        }
    }
}
