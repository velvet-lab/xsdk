using MongoDB.Bson;

namespace xSdk.Data
{
    public class MongoDbModelPKTests
    {
        [Fact]
        public void CreateNewPrimaryKey()
        {
            PrimaryKey primaryKey = new MongoDbModelPK();

            Assert.NotNull(primaryKey);
            Assert.IsType<string>(primaryKey.GetValue());
        }

        [Fact]
        public void CreateNewPrimaryKeyFromObjectId()
        {
            var objectId = ObjectId.GenerateNewId();
            PrimaryKey primaryKey = new MongoDbModelPK(objectId);

            Assert.NotNull(primaryKey);
            Assert.Equal(objectId, primaryKey.GetValue<ObjectId>());
            Assert.IsType<string>(primaryKey.GetValue());
        }

        [Fact]
        public void CreateNewPrimaryKeyFromString()
        {
            var objectId = ObjectId.GenerateNewId();
            var objectIdAsString = objectId.ToString();

            PrimaryKey primaryKey = new MongoDbModelPK(objectIdAsString);

            Assert.NotNull(primaryKey);
            Assert.Equal(objectIdAsString, primaryKey.GetValue()?.ToString());
            Assert.IsType<string>(primaryKey.GetValue());
        }
    }
}
