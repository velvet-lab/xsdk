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

using Bogus;

namespace xSdk.Data.Models;

public sealed class VariableModelExamples : Fakes<VariableModel>
{
    protected override void Build(Faker<VariableModel> builder)
    {
        builder
            .RuleFor(x => x.Name, f => f.Lorem.Word())
            .RuleFor(x => x.HelpText, f => f.Lorem.Sentence())
            .RuleFor(x => x.Prefix, f => f.Lorem.Word())
            .RuleFor(x => x.IsHidden, f => f.Random.Bool())
            .RuleFor(x => x.NoPrefix, f => f.Random.Bool())
            .RuleFor(x => x.IsProtected, f => f.Random.Bool());
    }
}
