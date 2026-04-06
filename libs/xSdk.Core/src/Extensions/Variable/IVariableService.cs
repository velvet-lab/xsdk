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
    ConcurrentBag<IVariable> Variables { get; }

    TType ReadVariableValue<TType>(string name, bool shouldThrowIfNotFound = false);

    void SetVariable<TValueType>(string name, TValueType value);

    bool ExistsVariable(string name);

    IDictionary<string, object> BuildResources();

    void NewVariable(IVariable variable);

    void NewVariable(IVariable variable, bool throwIfAlreadyExists);

    void NewVariable<TValueType>(IVariable variable, TValueType value);

    void NewVariable<TValueType>(IVariable variable, TValueType value, bool throwIfAlreadyExists);

    IVariable LoadVariable(string name);

    Dictionary<string, object> ToDictionary();

    void RegisterProvider(Type providerType);    
}
