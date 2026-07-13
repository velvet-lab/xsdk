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

namespace xSdk.Extensions.AI;

internal class ClientBuilderValidator : AbstractValidator<ClientBuilder>
{
    public ClientBuilderValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Client name must not be empty.")
            .MaximumLength(100).WithMessage("Client name must not exceed 100 characters.");

        RuleFor(x => x.ApiKey)
            .NotEmpty().WithMessage("API key must not be empty.")
            .MaximumLength(200).WithMessage("API key must not exceed 200 characters.");

        RuleFor(x => x.Endpoint)
            .NotEmpty().WithMessage("Endpoint must not be empty.")
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute));
    }
}
