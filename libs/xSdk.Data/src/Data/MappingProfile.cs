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
using NLog;

namespace xSdk.Data;

public abstract class MappingProfile
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    protected static TypeAdapterSetter<TSource, TDestination> CreateMap<TSource, TDestination>()
    {
        TypeAdapterConfig<TSource, TDestination>.Clear();
        return TypeAdapterConfig<TSource, TDestination>.NewConfig();
    }

    internal TypeAdapterConfig CreateConfig(Action<TypeAdapterConfig>? configure = null)
    {
        // default configuration
        Action<TypeAdapterConfig> defaultConfig = config =>
        {
            config.Default.EnableNonPublicMembers(false);
            config.Default.IgnoreNullValues(true);
            config.Default.EnumMappingStrategy(EnumMappingStrategy.ByName);
            config.Default.PreserveReference(true);
            config.Default.Settings.EnableNonPublicMembers = false;

            config.RequireExplicitMapping = true;
            config.RequireDestinationMemberSource = false;
        };

        _logger.Debug("Creating TypeAdapterConfig for Profile '{0}'", GetType());
        var globalConfig = TypeAdapterConfig.GlobalSettings;

        if (configure == null)
        {
            _logger.Debug("Using default configuration for Profile '{0}'", GetType());
            configure = defaultConfig;
        }

        _logger.Debug("Applying global configuration for Profile '{0}'", GetType());
        configure(globalConfig);

        _logger.Debug("Applying profile configuration for Profile '{0}'", GetType());
        Configure(globalConfig);

        _logger.Debug("Compiling TypeAdapterConfig for Profile '{0}'", GetType());
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
