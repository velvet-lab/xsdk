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

public sealed class CertAuthOptionsValidator : AbstractValidator<CertAuthOptions>
{
    public CertAuthOptionsValidator()
    {
        RuleFor(x => x.Certificate).NotNull().NotEmpty().WithMessage("Client certificate for vault certificate auth is missing");
        RuleFor(x => x.Key).NotNull().NotEmpty().WithMessage("Client certificate key for vault certificate auth is missing");
    }
}
