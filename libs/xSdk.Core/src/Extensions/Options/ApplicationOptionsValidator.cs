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

public sealed class ApplicationOptionsValidator : AbstractValidator<ApplicationOptions>
{
    public ApplicationOptionsValidator()
    {
        RuleFor(x => x.Prefix).NotEmpty().WithMessage("AppPrefix must not be empty.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name must not be empty.");
        RuleFor(x => x.Company).NotEmpty().WithMessage("Company must not be empty.");
        RuleFor(x => x.AppVersion).NotEmpty().WithMessage("AppVersion must not be empty.");
    }
}
