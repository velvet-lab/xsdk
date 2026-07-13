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

using Microsoft.Extensions.AI;
using xSdk.Extensions.Builder;
using xSdk.Plugins.AI;

namespace xSdk.Extensions.AI;

public sealed class ToolBuilder(AIBuilder parent) : BuilderBase
{
    public Action<ToolBuilder>? ConfigureBuilderAction { get; internal set; }

    internal string? Name { get; set; }

    internal string? Description { get; set; }

    internal Delegate? InProcessDelegate { get; set; }

    internal AIFunctionFactoryOptions? InProcessOptions { get; set; }


    internal AIFunction? BuildInProcess()
    {
        ConfigureBuilderAction?.Invoke(this);

        if (InProcessDelegate is not null)
        {
            if (InProcessOptions is null)
            {
                InProcessOptions = new AIFunctionFactoryOptions();
            }

            InProcessOptions.Name = Name;
            InProcessOptions.Description = Description;

            return AIFunctionFactory.Create(InProcessDelegate, InProcessOptions);
        }
        return default;
    }
}
