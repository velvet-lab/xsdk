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

using Mapster;
using Microsoft.Extensions.Logging;
using xSdk.Hosting;

namespace xSdk.Data;

public abstract class MappingProfile
{
    private static readonly ILogger _logger = LogManager.CreateLogger<MappingProfile>();

    protected static TypeAdapterSetter<TSource, TDestination> CreateMap<TSource, TDestination>()
    {
        TypeAdapterConfig<TSource, TDestination>.Clear();
        return TypeAdapterConfig<TSource, TDestination>.NewConfig();
    }

    internal TypeAdapterConfig CreateConfig(Action<TypeAdapterConfig>? configure)
    {
        // default configuration
        Action<TypeAdapterConfig> defaultConfig = config =>
        {
            config.Default.EnableNonPublicMembers(false);
            config.Default.IgnoreNullValues(true);
            config.Default.IgnoreNonMapped(true);
            config.Default.EnumMappingStrategy(EnumMappingStrategy.ByName);
            config.Default.PreserveReference(true);           

            config.RequireExplicitMapping = false;
            config.RequireDestinationMemberSource = false;
            config.RequireExplicitMappingPrimitive = false;
        };

        _logger.LogDebug("Creating TypeAdapterConfig for Profile '{0}'", GetType());
        var globalConfig = TypeAdapterConfig.GlobalSettings;

        if (configure == null)
        {
            _logger.LogDebug("Using default configuration for Profile '{0}'", GetType());
            configure = defaultConfig;
        }

        _logger.LogDebug("Applying global configuration for Profile '{0}'", GetType());
        configure(globalConfig);

        _logger.LogDebug("Applying profile configuration for Profile '{0}'", GetType());
        Configure(globalConfig);

        _logger.LogDebug("Compiling TypeAdapterConfig for Profile '{0}'", GetType());
        globalConfig.Compile();

        return globalConfig;
    }

    protected virtual void Configure()
    {

    }

    protected virtual void Configure(TypeAdapterConfig config)
    {
        Configure();
    }
}
