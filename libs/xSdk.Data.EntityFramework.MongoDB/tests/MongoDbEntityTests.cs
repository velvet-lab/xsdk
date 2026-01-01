using xSdk.Data.Mocks;
using MongoDB.Bson;

namespace xSdk.Data
{
    public class MongoDbEntityTests
    {
        [Fact]
        public void CreateNewEntity()
        {
            var entity = new TestEntity();

            Assert.NotNull(entity);
            Assert.NotNull(entity.PrimaryKey);
            Assert.Equal(entity.PrimaryKey.GetValue(), entity.Id);
            Assert.IsType<ObjectId>(entity.PrimaryKey.GetValue());
        }

        [Fact]
        public void CreateNewEntityFromExistingPrimaryKey()
        {
            var pk = ObjectId.GenerateNewId();
            var entity = new TestEntity();
            entity.Id = pk;

            Assert.NotNull(entity);
            Assert.NotNull(entity.PrimaryKey);
            Assert.Equal(entity.PrimaryKey.GetValue(), entity.Id);
            Assert.IsType<ObjectId>(entity.PrimaryKey.GetValue());
        }

        [Fact]
        public void CreateNewEntityByInterface()
        {
            var pk = ObjectId.GenerateNewId();
            IEntity entity = new TestEntity();
            entity.Id = pk;

            Assert.NotNull(entity);
            Assert.NotNull(entity.PrimaryKey);
            Assert.Equal(entity.PrimaryKey.GetValue(), entity.Id);
            Assert.IsType<ObjectId>(entity.PrimaryKey.GetValue());
        }
    }
}
