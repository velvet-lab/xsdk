using xSdk.Data.Mocks;

namespace xSdk.Data
{
    public class EFModelTests
    {
        [Fact]
        public void CreateNewModel()
        {
            var model = new TestModel();

            Assert.NotNull(model);
            Assert.NotNull(model.PrimaryKey);
            Assert.Equal(model.PrimaryKey.GetValue(), model.Id);
            Assert.IsType<string>(model.PrimaryKey.GetValue());
        }

        [Fact]
        public void CreateNewEntityFromExistingPrimaryKey()
        {
            var pk = Guid.NewGuid().ToString();
            var model = new TestModel();
            model.Id = pk;

            Assert.NotNull(model);
            Assert.NotNull(model.PrimaryKey);
            Assert.Equal(model.PrimaryKey.GetValue(), model.Id);
            Assert.IsType<string>(model.PrimaryKey.GetValue());
        }

        [Fact]
        public void CreateNewEntityByInterface()
        {
            var pk = Guid.NewGuid().ToString();
            IModel model = new TestModel();
            model.Id = pk;

            Assert.NotNull(model);
            Assert.NotNull(model.PrimaryKey);
            Assert.Equal(model.PrimaryKey.GetValue(), model.Id);
            Assert.IsType<string>(model.PrimaryKey.GetValue());
        }
    }
}
