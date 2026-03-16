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

using System.Collections.Concurrent;
using xSdk.Shared;

namespace xSdk.Extensions.Variable;

internal partial class VariableService
{
    private readonly ConcurrentDictionary<Type, ISetup> _setups = new ConcurrentDictionary<Type, ISetup>();
    private readonly List<VariableRegistration> _registrations = new();

    public IVariableService RegisterSetup<TSetup>()
        where TSetup : class, ISetup, new()
    {
        AddSetup(new VariableRegistration<TSetup>());
        return this;
    }

    public IVariableService RegisterSetup<TSetup>(Action<TSetup>? configure)
        where TSetup : class, ISetup, new()
    {
        if (configure != null)
        {
            AddSetup(new VariableRegistration<TSetup>() { Configure = configure });
        }
        else
        {
            RegisterSetup<TSetup>();
        }
        return this;
    }

    public IVariableService RegisterSetup<TSetup>(TSetup? implementation)
        where TSetup : class, ISetup, new()
    {
        if (implementation != null)
        {
            AddSetup(new VariableRegistration<TSetup>() { Implementation = implementation });
        }
        else
        {
            RegisterSetup<TSetup>();
        }
        return this;
    }

    public TSetup GetSetup<TSetup>(bool validate, bool throwIfFails)
        where TSetup : ISetup
    {
        var setupType = typeof(TSetup);

        var filter = _setups.Keys.SingleOrDefault(x => x == setupType);
        if (filter == null && setupType.IsInterface)
        {
            filter = _setups.Keys.SingleOrDefault(x => x.IsAssignableTo(setupType));
        }

        if (filter != null)
        {
            var setup = (TSetup)_setups[filter];
            if (setup != null)
            {
                if (validate)
                {
                    setup.Validate(throwIfFails);
                }
                return setup;
            }
        }

        throw new KeyNotFoundException(string.Format("GetSetup '{0}' could not found", setupType));
    }

    private void AddSetup(VariableRegistration registration)
    {
        if (!_setups.ContainsKey(registration.Type))
        {
            var setup = registration.Create(this);
            _setups.AddOrNew(registration.Type, setup);
        }
    }
}
