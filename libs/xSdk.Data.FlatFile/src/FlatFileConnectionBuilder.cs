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

using Microsoft.Extensions.Logging;

namespace xSdk.Data;

internal sealed class FlatFileConnectionBuilder : ConnectionBuilder
{
    private readonly ILogger<FlatFileConnectionBuilder> _logger;

    public FlatFileConnectionBuilder(ILogger<FlatFileConnectionBuilder> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override object Create(IDatabaseSetup setup)
    {
        return setup;
    }
}
