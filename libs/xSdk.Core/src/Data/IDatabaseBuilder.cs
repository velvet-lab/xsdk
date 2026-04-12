namespace xSdk.Data;

public interface IDatabaseBuilder
{
    IDatabaseBuilder ConfigureRepository<TImplementation>()
        where TImplementation : class, IRepository;

    IDatabaseBuilder ConfigureRepository<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : class, IRepository, TInterface;
}
