using FluentValidation;

namespace xSdk.Extensions.AI;

internal class AgentBuilderValidator : AbstractValidator<AgentBuilder>
{
    public AgentBuilderValidator()
    {
        When(x => string.IsNullOrEmpty(x.FilePath), () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Agent name must not be empty.")
                .MaximumLength(100).WithMessage("Agent name must not exceed 100 characters.");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Agent model must not be empty.")
                .MaximumLength(100).WithMessage("Agent model must not exceed 100 characters.");

            RuleFor(x => x.Instructions)
                .NotEmpty().WithMessage("Agent instructions must not be empty.")
                .MaximumLength(1000).WithMessage("Agent instructions must not exceed 1000 characters.");
        });

        When(x => string.IsNullOrEmpty(x.FilePath) && string.IsNullOrEmpty(x.Name) && string.IsNullOrEmpty(x.Instructions) && string.IsNullOrEmpty(x.Model), () =>
        {
            RuleFor(x => x.FilePath)
                .NotEmpty().WithMessage("Agent file path must not be empty when name, model, and instructions are not provided.")
                .MaximumLength(200).WithMessage("Agent file path must not exceed 200 characters.");

        });
    }
}
