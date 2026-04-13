using xSdk.Extensions.Variable;

namespace xSdk.Data;

public interface IDatabaseBuilder
{
    IDatabaseBuilder ConfigureOptions<TOptions>(string? name, Action<TOptions>? factory)
        where TOptions : class, IVariableSetup;

    IDatabaseBuilder ConfigureRepository<TImplementation>()
        where TImplementation : class, IRepository;

    IDatabaseBuilder ConfigureRepository<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : class, IRepository, TInterface;
}
