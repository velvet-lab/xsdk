using FluentValidation;

namespace xSdk.Data;

public sealed class TokenAuthOptionsValidator : AbstractValidator<TokenAuthOptions>
{
    public TokenAuthOptionsValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token must not be empty.");
    }
}
