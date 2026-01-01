using MongoDB.Bson;

namespace xSdk.Data
{
    public class MongoDbEntityPkTests
    {
        [Fact]
        public void CreateNewPrimaryKey()
        {
            PrimaryKey primaryKey = new MongoDbEntityPK();

            Assert.NotNull(primaryKey);
            Assert.IsType<ObjectId>(primaryKey.GetValue());
        }

        [Fact]
        public void CreateNewPrimaryKeyFromObjectId()
        {
            var objectId = ObjectId.GenerateNewId();
            PrimaryKey primaryKey = new MongoDbEntityPK(objectId);

            Assert.NotNull(primaryKey);
            Assert.Equal(objectId, primaryKey.GetValue<ObjectId>());
            Assert.IsType<ObjectId>(primaryKey.GetValue());
        }

        [Fact]
        public void CreateNewPrimaryKeyFromString()
        {
            var objectId = ObjectId.GenerateNewId();
            var objectIdAsString = objectId.ToString();

            PrimaryKey primaryKey = new MongoDbEntityPK(objectIdAsString);

            Assert.NotNull(primaryKey);
            Assert.Equal(objectIdAsString, primaryKey.GetValue()?.ToString());
            Assert.IsType<ObjectId>(primaryKey.GetValue());
        }
    }
}
