using Bogus;
using xSdk.Data;
using Microsoft.AspNetCore.Mvc;

namespace xSdk.Extensions.Web
{
    public sealed class ProblemDetailsExamples : Fakes<ProblemDetails>
    {
        protected override void Build(Faker<ProblemDetails> builder)
        {
            builder
                .RuleFor(x => x.Detail, f => f.Lorem.Sentence())
                .RuleFor(x => x.Title, f => f.System.Exception().Message)
                .RuleFor(x => x.Status, f => f.Random.Int(400, 599));

        }
    }
}
