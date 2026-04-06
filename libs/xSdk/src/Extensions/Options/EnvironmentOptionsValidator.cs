using FluentValidation;

namespace xSdk.Extensions.Options;

public sealed class EnvironmentOptionsValidator : AbstractValidator<EnvironmentOptions>
{
    public EnvironmentOptionsValidator()
    {
        RuleFor(x => x.ServiceName).NotEmpty().WithMessage("Unique Service Name is missing");
        RuleFor(x => x.ServiceNamespace).NotEmpty().WithMessage("Unique Service Namespace is missing");
        RuleFor(x => x.ServiceVersion).NotEmpty().WithMessage("Unique Service Version is missing");
        RuleFor(x => x.ServiceFullName).NotEmpty().WithMessage("Unique Service Fullname is missing");
    }
}
