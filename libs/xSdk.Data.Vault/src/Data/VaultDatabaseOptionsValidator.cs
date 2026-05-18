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

namespace xSdk.Data;

public sealed class VaultDatabaseOptionsValidator : AbstractValidator<VaultDatabaseOptions>
{
    public VaultDatabaseOptionsValidator()
    {
#pragma warning disable CS8602 // Dereferenzierung eines möglichen Nullverweises.
        RuleFor(x => x.Endpoint)
            .NotNull()
            .NotEmpty()
            .WithMessage("Endpoint is required.")
            .Must(x => x.StartsWith("http://") || x.StartsWith("https://")) // DevSkim: ignore DS137138
            .WithMessage("Endpoint must start with 'http://' or 'https://'."); // DevSkim: ignore DS137138
#pragma warning restore CS8602 // Dereferenzierung eines möglichen Nullverweises.

        RuleFor(x => x.AuthMethod).NotEqual(AuthMethods.None).WithMessage("Authentication method is required.");
    }
}
