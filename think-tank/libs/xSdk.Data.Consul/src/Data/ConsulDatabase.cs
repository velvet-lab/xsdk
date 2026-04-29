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

using System;
using Consul;
using Microsoft.Extensions.Logging;

namespace xSdk.Data;

public class ConsulDatabase : Database
{
    private readonly ILogger<ConsulDatabase> _logger;

    public ConsulDatabase(ILogger<ConsulDatabase> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override TConnection Open<TConnection>(Func<object> connectionStringBuilder) => Open<TConnection>(null, connectionStringBuilder);

    protected override TConnection Open<TConnection>(object connection, Func<object> connectionStringBuilder)
    {
        IConsulClient client = connection as IConsulClient;

        try
        {
            if (client == null)
            {
                client = connectionStringBuilder() as IConsulClient;
            }
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "A DBConnection could not created. Is the State Server running?");
            throw;
        }

        return client as TConnection;
    }
}
