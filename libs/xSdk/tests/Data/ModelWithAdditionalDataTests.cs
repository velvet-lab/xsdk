using xSdk.Data.Mocks;
using System.Text.Json;

namespace xSdk.Data
{
    public sealed class ModelWithAdditionalDataTests
    {
        [Fact]
        public void CreateModelWithoutAdditionalData()
        {
            var model = new TestModel
            {
                Name = "TestName"
            };

            Assert.Equal("TestName", model.Name);
        }

        [Fact]
        public void SerializeModelWithoutAdditionalData()
        {
            var model = new TestModel
            {
                Name = "TestName"
            };

            var json = JsonSerializer.Serialize(model);

            Assert.NotNull(json);
            Assert.Equal("{\"Name\":\"TestName\"}", json);
        }

        [Fact]
        public void DeserializeModelWithoutAdditionalData()
        {
            var model = new TestModel
            {
                Name = "TestName"
            };

            var json = JsonSerializer.Serialize(model);
            var actual = JsonSerializer.Deserialize<TestModel>(json);

            Assert.NotNull(actual);
            Assert.Equal(actual.Name, model.Name);
        }

        [Fact]
        public void DeserializeModelWithoutAdditionalDataWithValidation()
        {
            var model = new TestModel
            {
                Name = "TestName"
            };

            var json = JsonSerializer.Serialize(model);
            var actual = JsonSerializer.Deserialize<TestModel>(json);

            var validator = new TestModelValidation();
            var result = validator.Validate(actual);

            Assert.NotNull(actual);
            Assert.Equal(actual.Name, model.Name);
            Assert.True(result.IsValid);
        }
    }
}
