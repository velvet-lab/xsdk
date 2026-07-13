using FluentValidation;

namespace xSdk.Extensions.AI;

internal class ClientBuilderValidator : AbstractValidator<ClientBuilder>
{
    public ClientBuilderValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Client name must not be empty.")
            .MaximumLength(100).WithMessage("Client name must not exceed 100 characters.");

        RuleFor(x => x.ApiKey)
            .NotEmpty().WithMessage("API key must not be empty.")
            .MaximumLength(200).WithMessage("API key must not exceed 200 characters.");

        RuleFor(x => x.Endpoint)
            .NotEmpty().WithMessage("Endpoint must not be empty.")
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute));
    }
}
