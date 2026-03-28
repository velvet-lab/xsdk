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
using MapsterMapper;
using Microsoft.Extensions.Logging;
using xSdk.Hosting;

namespace xSdk.Data;

public static class MappingFactory
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public static IMapper CreateMapper<TProfile>()
        where TProfile : MappingProfile, new()
    {
        _logger.LogTrace("Create Mapper for Profile '{0}'", typeof(TProfile));

        var profile = new TProfile();
        var config = profile.CreateConfig();
        var mapper = new Mapper(config);

        return mapper;
    }

    public static IMapper CreateMapper<TProfile>(Action<TypeAdapterConfig> configure)
        where TProfile : MappingProfile, new()
    {
        _logger.LogTrace("Create Mapper for Profile '{0}' with custom configuration", typeof(TProfile));

        var profile = new TProfile();
        var config = profile.CreateConfig(configure);
        var mapper = new Mapper(config);

        return mapper;
    }
}
