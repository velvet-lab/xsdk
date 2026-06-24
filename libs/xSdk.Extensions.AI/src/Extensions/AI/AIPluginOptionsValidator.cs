using FluentValidation;

namespace xSdk.Extensions.AI;

public sealed class AIPluginOptionsValidator : AbstractValidator<AIPluginOptions>
{
    public AIPluginOptionsValidator()
    {
        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("Default AI model is missing")
            .WithErrorCode(AIPluginOptions.Definitions.Model.Name);
    }
}
