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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using xSdk.Hosting;

namespace xSdk.Data;

internal sealed class DatabasePoolPolicy<TDatabase>(IServiceProvider provider) : IPooledObjectPolicy<TDatabase>
    where TDatabase : class
{
    private readonly ILogger _logger = provider.GetService<ILogger<DatabasePoolPolicy<TDatabase>>>() ?? LogManager.GetCurrentClassLogger();
    private readonly ObjectFactory _factory = ActivatorUtilities.CreateFactory(typeof(TDatabase), Type.EmptyTypes);
    private readonly bool _isResettable = typeof(IResettable).IsAssignableFrom(typeof(TDatabase));

    /// <summary>
    /// Create a <typeparamref name="T"/>.
    /// </summary>
    /// <returns>The <typeparamref name="T"/> which was created.</returns>

    public TDatabase Create()
    {
        try
        {
            object objectFactory = _factory(provider, []);
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
