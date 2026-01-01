using LiteDB;

namespace xSdk.Data
{
    public class NoSqlEntityPKTests
    {
        [Fact]
        public void CreateNewPrimaryKey()
        {
            PrimaryKey primaryKey = new NoSqlEntityPK();

            Assert.NotNull(primaryKey);
        }

        [Fact]
        public void CreateNewPrimaryKeyFromObjectId()
        {
            var objectId = ObjectId.NewObjectId();
            PrimaryKey primaryKey = new NoSqlEntityPK(objectId);

            Assert.NotNull(primaryKey);
            Assert.Equal(objectId, primaryKey.GetValue<ObjectId>());
        }

        [Fact]
        public void CreateNewPrimaryKeyFromString()
        {
            var objectId = ObjectId.NewObjectId();
            var objectIdAsString = objectId.ToString();

            PrimaryKey primaryKey = new NoSqlEntityPK(objectIdAsString);

            Assert.NotNull(primaryKey);
            Assert.Equal(objectIdAsString, primaryKey.GetValue()?.ToString());
        }
    }
}
