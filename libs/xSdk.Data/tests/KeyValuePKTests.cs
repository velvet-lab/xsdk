namespace xSdk.Data
{
    public class KeyValuePKTests
    {
        [Fact]
        public void CreateNewPrimaryKey()
        {
            KeyValuePK primaryKey = new KeyValuePK();

            Assert.NotNull(primaryKey);
            Assert.IsType<string>(primaryKey.GetValue());
        }

        [Fact]
        public void CreateNewPrimaryKeyFromString()
        {
            var id = Guid.NewGuid();
            var expected = id.ToString();

            KeyValuePK primaryKey = new KeyValuePK(expected);

            Assert.NotNull(primaryKey);
            Assert.Equal(expected, Guid.Parse(primaryKey.GetValue().ToString()).ToString());
            Assert.IsType<string>(primaryKey.GetValue());
        }
    }
}
