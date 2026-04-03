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

namespace xSdk.Extensions.Variable;

public interface IVariableService
{
    // bool TryReadVariableValue<TPrimaryKeyType>(string name, out TPrimaryKeyType primaryKey);

    // Variable LoadVariable(string name);

    // void SetVariable<TValueType>(string name, TValueType primaryKey);

    ConcurrentBag<IVariable> Variables { get; }

    TType ReadVariableValue<TType>(string name, bool shouldThrowIfNotFound = false);

    bool ExistsVariable(string name);

    IDictionary<string, object> BuildResources();

    TSetup GetSetup<TSetup>()
        where TSetup : ISetup => GetSetup<TSetup>(true, true);

    TSetup GetSetup<TSetup>(bool throwIfFails)
        where TSetup : ISetup => GetSetup<TSetup>(true, throwIfFails);

    TSetup GetSetup<TSetup>(bool validate, bool throwIfFails)
        where TSetup : ISetup;

    IVariableService RegisterSetup<TSetup>()
        where TSetup : class, ISetup, new();

    IVariableService RegisterSetup<TSetup>(Action<TSetup>? configure)
        where TSetup : class, ISetup, new();

    IVariableService RegisterSetup<TSetup>(TSetup? implementation)
        where TSetup : class, ISetup, new();

    void NewVariable(IVariable variable);

    void NewVariable(IVariable variable, bool throwIfAlreadyExists);

    void NewVariable<TValueType>(IVariable variable, TValueType value);

    void NewVariable<TValueType>(IVariable variable, TValueType value, bool throwIfAlreadyExists);

    Dictionary<string, object> ToDictionary();

    void RegisterProvider(Type providerType);
}

//public interface IVariableService<TSetup> : IVariableService
//    where TSetup : class, ISetup
//{
//    TPrimaryKeyType ReadVariableValue<TPrimaryKeyType>(Expression<Func<TSetup, TPrimaryKeyType>> expr, bool shouldThrowIfNotFound = false);

//    bool TryReadVariableValue<TPrimaryKeyType>(Expression<Func<TSetup, TPrimaryKeyType>> expr, out TPrimaryKeyType primaryKey);

//    TSetup GetSetup { get; }
//}
