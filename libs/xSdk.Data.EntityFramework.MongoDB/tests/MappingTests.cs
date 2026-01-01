using xSdk.Data.Mocks;
using MongoDB.Bson;

namespace xSdk.Data
{
    public class MappingTests
    {
        [Fact]
        public void Map2Model()
        {
            var fake = FakeGenerator.Generate<TestEntityFakes, TestEntity>();
            var model = fake.ToModel<TestMapper, TestModel>();

            Assert.NotNull(model);
            Assert.Equal(fake.Id.ToString(), model.Id);
            Assert.Equal(fake.Name, model.MyName);
            Assert.Equal(fake.Age, model.MyAge);
            Assert.IsType<string>(model.PrimaryKey.GetValue());
        }

        [Fact]
        public void Map2Entity()
        {
            var fake = FakeGenerator.Generate<TestEntityFakes, TestEntity>();
            var model = new TestModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                MyName = fake.Name,
                MyAge = fake.Age,
            };
            var entity = model.ToEntity<TestMapper, TestEntity>();

            Assert.NotNull(entity);
            Assert.Equal(model.Id, entity.Id.ToString());
            Assert.Equal(fake.Name, entity.Name);
            Assert.Equal(fake.Age, entity.Age);
            Assert.IsType<ObjectId>(entity.PrimaryKey.GetValue());
        }
    }
}
