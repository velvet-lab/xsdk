using xSdk.Data.Mocks;

namespace xSdk.Data
{
    public class ModelTests
    {
        [Fact]
        public void ModelShouldCreated()
        {
            var model = new TestModel { Age = 42, Name = "John Doe" };

            Assert.NotNull(model);
            Assert.IsType<string>(model.PrimaryKey.GetValue());
        }

        [Fact]
        public void ModelShouldCreatedWithAutomaticGeneratedPK()
        {
            var model = new TestModel { Age = 42, Name = "John Doe" };

            Assert.NotNull(model);
            Assert.NotNull(model.Id);
            Assert.IsType<string>(model.PrimaryKey.GetValue());
        }
    }
}
