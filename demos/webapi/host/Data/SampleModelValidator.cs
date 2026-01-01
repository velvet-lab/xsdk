using FluentValidation;

namespace xSdk.Demos.Data
{
    public sealed class SampleModelValidator : AbstractValidator<SampleModel>
    {
        public SampleModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name)
                .NotEmpty().WithSeverity(Severity.Warning);

        }
    }
}
