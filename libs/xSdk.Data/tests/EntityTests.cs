using xSdk.Data.Mocks;

namespace xSdk.Data
{
    public class EntityTests
    {
        [Fact]
        public void EntityShouldCreated()
        {
            var entity = new TestEntity { Age = 42, Name = "John Doe" };

            Assert.NotNull(entity);
            Assert.IsType<Guid>(entity.PrimaryKey.GetValue());
        }

        [Fact]
        public void EntityShouldCreatedWithAutomaticGeneratedPK()
        {
            var entity = new TestEntity { Age = 42, Name = "John Doe" };

            Assert.NotNull(entity);
            Assert.True(entity.Id != Guid.Empty);
            Assert.IsType<Guid>(entity.PrimaryKey.GetValue());
        }
    }
}
