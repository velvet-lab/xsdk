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

namespace xSdk.Extensions.Variable;

internal class VariableRegistration
{
    internal virtual Type Type { get; }

    internal virtual ISetup Create(VariableService service)
    {
        return null;
    }
}

internal class VariableRegistration<TSetup> : VariableRegistration
    where TSetup : class, ISetup, new()
{
    internal Action<TSetup> Configure { get; set; }

    internal TSetup Implementation { get; set; }

    internal override Type Type => typeof(TSetup);

    internal override ISetup Create(VariableService service)
    {
        if (Implementation == null)
        {
            Implementation = new TSetup();
        }

        if (Implementation is Setup abstractSetup)
        {
            abstractSetup.InitializeInternal(service);
        }
        Configure?.Invoke(Implementation);

        return Implementation;
    }
}
