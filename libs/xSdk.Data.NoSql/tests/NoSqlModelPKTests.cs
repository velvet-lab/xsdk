using LiteDB;

namespace xSdk.Data
{
    public class NoSqlModelPkTests
    {
        [Fact]
        public void CreateNewPrimaryKey()
        {
            PrimaryKey primaryKey = new NoSqlModelPK();

            Assert.NotNull(primaryKey);
        }

        [Fact]
        public void CreateNewPrimaryKeyFromObjectId()
        {
            var objectId = ObjectId.NewObjectId();
            PrimaryKey primaryKey = new NoSqlModelPK(objectId);

            Assert.NotNull(primaryKey);
            Assert.Equal(objectId, primaryKey.GetValue<ObjectId>());
        }

        [Fact]
        public void CreateNewPrimaryKeyFromString()
        {
            var objectId = ObjectId.NewObjectId();
            var objectIdAsString = objectId.ToString();

            PrimaryKey primaryKey = new NoSqlModelPK(objectIdAsString);

            Assert.NotNull(primaryKey);
            Assert.Equal(objectIdAsString, primaryKey.GetValue()?.ToString());
        }
    }
}
