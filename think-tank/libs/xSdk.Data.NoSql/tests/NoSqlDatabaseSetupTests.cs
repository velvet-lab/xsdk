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

namespace xSdk.Data;

public class NoSqlDatabaseSetupTests
{
    [Fact]
    public void DefaultInitialSize_IsZero()
    {
        var setup = new NoSqlDatabaseOptions();

        Assert.Equal(0L, setup.InitialSize);
    }

    [Fact]
    public void DefaultUpgrade_IsFalse()
    {
        var setup = new NoSqlDatabaseOptions();

        Assert.False(setup.Upgrade);
    }

    [Fact]
    public void DefaultReadOnly_IsFalse()
    {
        var setup = new NoSqlDatabaseOptions();

        Assert.False(setup.ReadOnly);
    }

    [Fact]
    public void DefaultCollation_IsNull()
    {
        var setup = new NoSqlDatabaseOptions();

        Assert.Null(setup.Collation);
    }

    [Fact]
    public void SetFileName_RetainsValue()
    {
        var setup = new NoSqlDatabaseOptions
        {
            FileName = "test.db"
        };

        Assert.Equal("test.db", setup.FileName);
    }

    [Fact]
    public void SetPassword_RetainsValue()
    {
        var setup = new NoSqlDatabaseOptions
        {
            Password = "s3cret"
        };

        Assert.Equal("s3cret", setup.Password);
    }

    [Fact]
    public void SetReadOnly_True_RetainsValue()
    {
        var setup = new NoSqlDatabaseOptions
        {
            ReadOnly = true
        };

        Assert.True(setup.ReadOnly);
    }

    [Fact]
    public void SetUpgrade_True_RetainsValue()
    {
        var setup = new NoSqlDatabaseOptions
        {
            Upgrade = true
        };

        Assert.True(setup.Upgrade);
    }

    [Fact]
    public void SetInitialSize_RetainsValue()
    {
        var setup = new NoSqlDatabaseOptions
        {
            InitialSize = 4096L
        };

        Assert.Equal(4096L, setup.InitialSize);
    }
}
