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

using xSdk.Data.Mocks;

namespace xSdk.Data;

public class DatabaseTests
{
    [Fact]
    public void Database_Close_ReturnsFalse()
    {
        var db = new TestDatabase();

        var result = db.Close();

        Assert.False(result);
    }

    [Fact]
    public void Database_TryReset_ReturnsFalse()
    {
        var db = new TestDatabase();

        var result = db.TryReset();

        Assert.False(result);
    }

    [Fact]
    public void Database_DatalayerName_DefaultIsNull()
    {
        var db = new TestDatabase();

        Assert.Null(db.DatalayerName);
    }

    [Fact]
    public void Database_Services_DefaultIsNull()
    {
        var db = new TestDatabase();

        Assert.Null(db.Services);
    }

    [Fact]
    public void Database_ResolvePlaceholders_WithNamedPlaceholder_ReplacesCorrectly()
    {
        var db = new TestDatabase();
        db.AddConnectionProperty("host", "localhost");

        var result = db.ResolvePlaceholders("Server={host};Port=5432");

        Assert.Equal("Server=localhost;Port=5432", result);
    }

    [Fact]
    public void Database_ResolvePlaceholders_WithNoMatchingPlaceholder_ReturnsOriginal()
    {
        var db = new TestDatabase();
        db.AddConnectionProperty("host", "localhost");

        var result = db.ResolvePlaceholders("no placeholders here");

        Assert.Equal("no placeholders here", result);
    }

    [Fact]
    public void Database_ResolvePlaceholders_AfterRemoveConnectionProperty_DoesNotReplace()
    {
        var db = new TestDatabase();
        db.AddConnectionProperty("host", "localhost");
        db.RemoveConnectionProperty("host");

        var result = db.ResolvePlaceholders("Server={host}");

        Assert.Equal("Server={host}", result);
    }

    [Fact]
    public void Database_Dispose_DoesNotThrow()
    {
        var db = new TestDatabase();

        var exception = Record.Exception(() => db.Dispose());

        Assert.Null(exception);
    }

    [Fact]
    public void Database_Open_ReturnsNull()
    {
        var db = new TestDatabase();

        var result = db.Open<object>();

        Assert.Null(result);
    }
}
