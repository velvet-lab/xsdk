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
