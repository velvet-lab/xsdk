using Bogus;

namespace xSdk.Data.Models
{
    public sealed class VariableModelExamples : Fakes<VariableModel>
    {
        protected override void Build(Faker<VariableModel> builder)
        {
            builder
                .RuleFor(x => x.Name, f => f.Lorem.Word())
                .RuleFor(x => x.HelpText, f => f.Lorem.Sentence())
                .RuleFor(x => x.Prefix, f => f.Lorem.Word())
                .RuleFor(x => x.IsHidden, f => f.Random.Bool())
                .RuleFor(x => x.NoPrefix, f => f.Random.Bool())
                .RuleFor(x => x.IsProtected, f => f.Random.Bool());
        }
    }
}
