using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using FluentValidation;

namespace xSdk.Data;

public sealed class NoSqlDatabaseOptionsValidator : AbstractValidator<NoSqlDatabaseOptions>
{
    public NoSqlDatabaseOptionsValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .NotNull()
            .WithMessage("FileName is required.");
    }
}
