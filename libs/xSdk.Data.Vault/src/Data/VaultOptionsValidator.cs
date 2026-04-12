using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using static xSdk.Extensions.Commands.DefaultCommandSettings;

namespace xSdk.Data;

public sealed class VaultOptionsValidator : AbstractValidator<VaultOptions>
{
    public VaultOptionsValidator()
    {
        RuleFor(x => x.Endpoint).NotNull().NotEmpty().WithMessage("Endpoint is required.");
    }
}
