using xSdk.Data;

namespace xSdk.Demos.Data
{
    public sealed class SampleModel : MongoDbModel
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
