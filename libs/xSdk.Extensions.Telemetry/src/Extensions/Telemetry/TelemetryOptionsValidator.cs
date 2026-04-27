using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace xSdk.Extensions.Telemetry;

public sealed class TelemetryOptionsValidator : AbstractValidator<TelemetryOptions>
{
    public TelemetryOptionsValidator()
    {   
        When(x => !x.IsDisabled && !x.IsOtlpExporterDisabled, () =>
        {
            RuleFor(x => x.Endpoint)
                .NotEmpty()
                .WithMessage("No MaaS endpoint configured")
                .WithName(TelemetryOptions.Definitions.Endpoint.Name);
        });
    }
}
