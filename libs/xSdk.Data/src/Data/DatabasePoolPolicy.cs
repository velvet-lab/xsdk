using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;

namespace xSdk.Data;

internal sealed class DatabasePoolPolicy<TDatabase> : IPooledObjectPolicy<TDatabase>
    where TDatabase : class
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<DatabasePoolPolicy<TDatabase>>? _logger;
    private readonly ObjectFactory _factory;
    private readonly bool _isResettable;

    public DatabasePoolPolicy(IServiceProvider provider)
    {
        _provider = provider;
        _logger = provider.GetService<ILogger<DatabasePoolPolicy<TDatabase>>>();
        _factory = ActivatorUtilities.CreateFactory(typeof(TDatabase), Type.EmptyTypes);
        _isResettable = typeof(IResettable).IsAssignableFrom(typeof(TDatabase));
    }

    /// <summary>
    /// Create a <typeparamref name="T"/>.
    /// </summary>
    /// <returns>The <typeparamref name="T"/> which was created.</returns>

    public TDatabase Create()
    {
        //var database = ActivatorUtilities.CreateInstance<TDatabase>(_provider);
        //return database;

        try
        {
            var objectFactory = _factory(_provider, Array.Empty<object?>());
            return (TDatabase)objectFactory;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating instance of type '{TypeName}': {ExceptionMessage}", typeof(TDatabase).FullName, ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Runs some processing when an object was returned to the pool. Can be used to reset the state of an object and indicate if the object should be returned to the pool.
    /// </summary>
    /// <param name="obj">The object to return to the pool.</param>
    /// <returns><see langword="true" /> if the object should be returned to the pool. <see langword="false" /> if it's not possible/desirable for the pool to keep the object.</returns>
    public bool Return(TDatabase obj)
    {
        if (_isResettable)
        {
            return ((IResettable)obj).TryReset();
        }

        return true;
    }
}
