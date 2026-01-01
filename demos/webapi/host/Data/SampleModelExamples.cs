using Bogus;
using xSdk.Data;

namespace xSdk.Demos.Data
{
    public sealed class SampleModelExamples : Fakes<SampleModel>
    {
        protected override void Build(Faker<SampleModel> builder)
        {
            builder.RuleFor(x => x.Name, f => f.Name.FullName());
        }
    }
}
