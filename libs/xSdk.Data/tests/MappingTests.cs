using xSdk.Data.Mocks;

namespace xSdk.Data
{
    public class MappingTests
    {
        [Fact]
        public void MapEntityToModel()
        {
            var entity = new TestEntity { Age = 42, Name = "John Doe" };

            var model = entity.ToModel<TestMappingProfile, TestModel>();

            Assert.NotNull(model);
            Assert.Equal(entity.Name, model.Name);
            Assert.Equal(entity.Age, model.Age);
            Assert.Equal(entity.Id.ToString(), model.Id);
            Assert.IsType<string>(model.PrimaryKey.GetValue());
        }

        [Fact]
        public void MapModelToEntity()
        {
            var model = new TestModel { Age = 42, Name = "John Doe" };

            var entity = model.ToEntity<TestMappingProfile, TestEntity>();

            Assert.NotNull(entity);
            Assert.Equal(model.Name, entity.Name);
            Assert.Equal(model.Age, entity.Age);
            Assert.Equal(model.Id, entity.Id.ToString());
            Assert.IsType<Guid>(entity.PrimaryKey.GetValue());
        }
    }
}
