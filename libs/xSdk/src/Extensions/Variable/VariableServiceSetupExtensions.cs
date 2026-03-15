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

using xSdk.Extensions.Variable.Providers;

namespace xSdk.Extensions.Variable;

public static class VariableServiceSetupExtensions
{
    public static VariableServiceSetup AddEnvironmentVariablesWithoutSetup(this VariableServiceSetup setup)
    {
        setup.AddEnvironmentVariablesWithoutSetup = true;
        return setup;
    }

    //public static VariableServiceSetup AddCommandlineVariablesWithoutSetup(this VariableServiceSetup setup)
    //{
    //    setup.AddCommanlineVariablesWithoutSetup = true;
    //    return setup;
    //}

    public static VariableServiceSetup RegisterProvider<TProvider>(this VariableServiceSetup setup)
        where TProvider : VariableProvider, new()
    {
        setup.Providers.Add(typeof(TProvider));

        return setup;
    }
}
