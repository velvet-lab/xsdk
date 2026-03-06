using xSdk.Data.Mocks;

namespace xSdk.Data
{
    public class EFEntityTests
    {
        [Fact]
        public void CreateNewEntity()
        {
            var entity = new TestEntity();

            Assert.NotNull(entity);
            Assert.NotNull(entity.PrimaryKey);
            Assert.Equal(entity.PrimaryKey.GetValue(), entity.Id);
            Assert.IsType<Guid>(entity.PrimaryKey.GetValue());
        }

        [Fact]
        public void CreateNewEntityFromExistingPrimaryKey()
        {
            var pk = Guid.NewGuid();
            var entity = new TestEntity();
            entity.Id = pk;

            Assert.NotNull(entity);
            Assert.NotNull(entity.PrimaryKey);
            Assert.Equal(entity.PrimaryKey.GetValue(), entity.Id);
            Assert.IsType<Guid>(entity.PrimaryKey.GetValue());
        }

        [Fact]
        public void CreateNewEntityByInterface()
        {
            var pk = Guid.NewGuid();
            IEntity entity = new TestEntity();
            entity.Id = pk;

            Assert.NotNull(entity);
            Assert.NotNull(entity.PrimaryKey);
            Assert.Equal(entity.PrimaryKey.GetValue(), entity.Id);
            Assert.IsType<Guid>(entity.PrimaryKey.GetValue());
        }
    }
}
