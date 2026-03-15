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

using System.Globalization;
using LiteDB;
using xSdk.Extensions.Variable;

namespace xSdk.Data;

public sealed class NoSqlDatabaseSetup : DatabaseSetup
{
    public NoSqlDatabaseSetup()
    {
        Path = System.Environment.CurrentDirectory;
        InitialSize = 0;
        Upgrade = false;
        ReadOnly = false;
        Collation = new Collation(CultureInfo.CurrentCulture.LCID, CompareOptions.IgnoreCase);
    }

    public string Path { get; set; } = System.Environment.CurrentDirectory;

    public string FileName { get; set; }

    public string Password { get; set; }

    public long InitialSize { get; set; }

    public bool Upgrade { get; set; }

    public bool ReadOnly { get; set; }

    public Collation Collation { get; set; }

    protected override void ValidateSetup()
    {
        if (string.IsNullOrEmpty(this.Path))
            this.Path = System.Environment.CurrentDirectory;

        this.ValidateMember(x => string.IsNullOrEmpty(x.FileName));
    }
}
