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
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Logging;

namespace xSdk.Extensions.Builder;

public abstract class BuilderBase : IBuilder
{
    private ILogger Logger { get => field ??= LogManager.CreateLogger<BuilderBase>(); }

    public IServiceProvider Services
    {
        get => field ?? throw new InvalidOperationException("SlimServices has not been initialized.");
        internal set;
    }

    protected bool IsValid<TBuilder, TValidator>()
        where TValidator : AbstractValidator<TBuilder>, new()
        where TBuilder : BuilderBase
    {
        TBuilder instance = (TBuilder)this;
        var validator = new TValidator();

        var result = validator.Validate(instance);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
            {
                Logger.LogError(error.ErrorMessage);
            }
            return false;
        }

        return true;
    }
}
