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

namespace xSdk.Extensions.Options;

public sealed class EnvironmentOptionsValidator : AbstractValidator<EnvironmentOptions>
{
    public EnvironmentOptionsValidator()
    {
        RuleFor(x => x.ServiceName).NotEmpty().WithMessage("Unique Service Name is missing");
        RuleFor(x => x.ServiceNamespace).NotEmpty().WithMessage("Unique Service Namespace is missing");
        RuleFor(x => x.ServiceVersion).NotEmpty().WithMessage("Unique Service Version is missing");
        RuleFor(x => x.ServiceFullName).NotEmpty().WithMessage("Unique Service Fullname is missing");
    }
}
