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

internal class AgentBuilderValidator : AbstractValidator<AgentBuilder>
{
    public AgentBuilderValidator()
    {
        When(x => string.IsNullOrEmpty(x.FilePath), () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Agent name must not be empty.")
                .MaximumLength(100).WithMessage("Agent name must not exceed 100 characters.");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Agent model must not be empty.")
                .MaximumLength(100).WithMessage("Agent model must not exceed 100 characters.");

            RuleFor(x => x.Instructions)
                .NotEmpty().WithMessage("Agent instructions must not be empty.")
                .MaximumLength(1000).WithMessage("Agent instructions must not exceed 1000 characters.");
        });

        When(x => string.IsNullOrEmpty(x.FilePath) && string.IsNullOrEmpty(x.Name) && string.IsNullOrEmpty(x.Instructions) && string.IsNullOrEmpty(x.Model), () =>
        {
            RuleFor(x => x.FilePath)
                .NotEmpty().WithMessage("Agent file path must not be empty when name, model, and instructions are not provided.")
                .MaximumLength(200).WithMessage("Agent file path must not exceed 200 characters.");
        });

        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("Agent client name must not be empty.")
            .MaximumLength(100).WithMessage("Agent client name must not exceed 100 characters.");
    }
}
