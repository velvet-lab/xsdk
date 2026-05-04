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

using Microsoft.Extensions.Logging.Abstractions;

namespace xSdk.Data.Mocks;

internal class TestDatabase : Database
{
    public TestDatabase() : base(NullLogger<Database>.Instance)
    {
    }

    public override TDatabaseObject? Open<TDatabaseObject>() where TDatabaseObject : class => null;

    public new void AddConnectionProperty(string name, string value) => base.AddConnectionProperty(name, value);

    public new void RemoveConnectionProperty(string name) => base.RemoveConnectionProperty(name);

    public new string ResolvePlaceholders(string content) => base.ResolvePlaceholders(content);
}
