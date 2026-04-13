using FluentValidation;

namespace xSdk.Data;

public sealed class VaultDatabaseOptionsValidator : AbstractValidator<VaultDatabaseOptions>
{
    public VaultDatabaseOptionsValidator()
    {
        RuleFor(x => x.Endpoint)
            .NotNull()
            .NotEmpty()
            .WithMessage("Endpoint is required.")
            .Must(x => x.StartsWith("http://") || x.StartsWith("https://"))
            .WithMessage("Endpoint must start with 'http://' or 'https://'.");

        RuleFor(x => x.AuthMethod).NotEqual(AuthMethods.None).WithMessage("Authentication method is required.");
    }
}
