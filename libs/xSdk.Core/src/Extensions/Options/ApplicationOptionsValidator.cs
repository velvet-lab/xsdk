using FluentValidation;

namespace xSdk.Extensions.Options;

public sealed class ApplicationOptionsValidator : AbstractValidator<ApplicationOptions>
{
    public ApplicationOptionsValidator()
    {
        RuleFor(x => x.Prefix).NotEmpty().WithMessage("AppPrefix must not be empty.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name must not be empty.");
        RuleFor(x => x.Company).NotEmpty().WithMessage("Company must not be empty.");
        RuleFor(x => x.AppVersion).NotEmpty().WithMessage("AppVersion must not be empty.");
    }
}
