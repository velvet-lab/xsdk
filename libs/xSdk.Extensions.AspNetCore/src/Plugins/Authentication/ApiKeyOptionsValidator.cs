using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace xSdk.Plugins.Authentication;

public sealed class ApiKeyOptionsValidator : AbstractValidator<ApiKeyOptions>
{
    public ApiKeyOptionsValidator()
    {
        RuleFor(x => x.Realm)
            .NotEmpty()
            .WithMessage("Authentication realm is missing")
            .WithErrorCode(ApiKeyOptions.Definitions.Realm.Name);
    }
}
