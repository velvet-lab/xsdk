using Bogus;
using xSdk.Data;

namespace xSdk.Demos.Data
{
    public sealed class SampleEntityFakes : Fakes<SampleEntity>
    {
        protected override void Build(Faker<SampleEntity> builder)
        {
            builder.RuleFor(x => x.Age, f => f.Random.Int(1, 100)).RuleFor(x => x.Name, f => f.Name.FullName());
        }
    }
}
