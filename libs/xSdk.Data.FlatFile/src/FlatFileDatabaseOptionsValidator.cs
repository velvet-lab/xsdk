using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace xSdk.Data;

public sealed class FlatFileDatabaseOptionsValidator : AbstractValidator<FlatFileDatabaseOptions>
{
    public FlatFileDatabaseOptionsValidator()
    {
        RuleFor(x => x.FilePath)
            .NotNull()
            .NotEmpty()
            .WithMessage("FilePath is required.")
            .Must(fp => fp.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            .WithMessage("FilePath must end with '.json'.");
    }
}
