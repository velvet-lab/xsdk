using FluentValidation;

namespace xSdk.Data;

public sealed class CertAuthOptionsValidator : AbstractValidator<CertAuthOptions>
{
    public CertAuthOptionsValidator()
    {
        RuleFor(x => x.Certificate).NotNull().NotEmpty().WithMessage("Client certificate for vault certificate auth is missing");
        RuleFor(x => x.Key).NotNull().NotEmpty().WithMessage("Client certificate key for vault certificate auth is missing");
    }
}
