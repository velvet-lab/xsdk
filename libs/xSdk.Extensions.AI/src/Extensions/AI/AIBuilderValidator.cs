using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using xSdk.Plugins.AI;

namespace xSdk.Extensions.AI;

internal class AIBuilderValidator : AbstractValidator<AIBuilder>
{
    public AIBuilderValidator()
    {
        When(x => x.Options.IsTelemetryEnabled, () =>
        {
            RuleFor(x => x.DiagnosticsSourceName)
                .NotEmpty().WithMessage("Diagnostics source name must not be empty when telemetry is enabled.")
                .MaximumLength(100).WithMessage("Diagnostics source name must not exceed 100 characters.");
        });

        When(x => x.EnableLogging, () =>
        {
            RuleFor(x => x.LoggerFactory)
                .NotNull().WithMessage("LoggerFactory must not be null when logging is enabled.");
        });
    }
}
