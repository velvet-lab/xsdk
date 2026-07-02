using FluentValidation;
using xSdk.Plugins.AI;

namespace xSdk.Extensions.AI;

public sealed class AIPluginOptionsValidator : AbstractValidator<PluginOptions>
{
    public AIPluginOptionsValidator()
    {
        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("Default AI model is missing")
            .WithErrorCode(PluginOptions.Definitions.Model.Name);
    }
}
