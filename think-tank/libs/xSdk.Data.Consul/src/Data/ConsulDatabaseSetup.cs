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
using xSdk.Extensions.Variable;

namespace xSdk.Data;

public sealed class ConsulDatabaseSetup : DatabaseSetup
{
    public ConsulDatabaseSetup()
    {
        Port = 8500;
    }

    public string Token { get; set; }

    public string Host { get; set; }

    public int Port { get; set; }

    public string DataCenter { get; set; }

    public TimeSpan? WaitTime { get; set; }

    public bool UseTls { get; set; }

    public bool UseVaultAuth { get; set; }

    protected override void ValidateSetup()
    {
        this.ValidateMember(x => string.IsNullOrEmpty(x.Host));
        this.ValidateMember(x => !UseVaultAuth && string.IsNullOrEmpty(Token));
    }
}
