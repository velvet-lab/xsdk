/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
