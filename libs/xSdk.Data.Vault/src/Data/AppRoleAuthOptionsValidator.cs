using FluentValidation;

namespace xSdk.Data;

public sealed class AppRoleAuthOptionsValidator : AbstractValidator<AppRoleAuthOptions>
{
    public AppRoleAuthOptionsValidator()
    {
        RuleFor(x => x.RoleId).NotNull().NotEmpty().WithMessage("RoleId for vault approle auth is missing");
        RuleFor(x => x.Secret).NotNull().NotEmpty().WithMessage("Secret for vault approle auth is missing");
    }
}
