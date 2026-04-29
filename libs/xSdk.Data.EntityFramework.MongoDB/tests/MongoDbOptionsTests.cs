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

namespace xSdk.Data;

public class MongoDbOptionsTests
{
    [Fact]
    public void MongoDbOptions_DefaultValues_AreNull()
    {
        var options = new MongoDbOptions();

        Assert.Null(options.Database);
        Assert.Null(options.Username);
        Assert.Null(options.Password);
        Assert.Null(options.Hosts);
    }

    [Fact]
    public void MongoDbOptions_SetDatabase_StoresValue()
    {
        var options = new MongoDbOptions();
        options.Database = "testdb";

        Assert.Equal("testdb", options.Database);
    }

    [Fact]
    public void MongoDbOptions_SetUsername_StoresValue()
    {
        var options = new MongoDbOptions();
        options.Username = "admin";

        Assert.Equal("admin", options.Username);
    }

    [Fact]
    public void MongoDbOptions_SetPassword_StoresValue()
    {
        var options = new MongoDbOptions();
        options.Password = "secret";

        Assert.Equal("secret", options.Password);
    }

    [Fact]
    public void MongoDbOptions_SetHosts_StoresValue()
    {
        var options = new MongoDbOptions();
        options.Hosts = ["localhost", "replica1"];

        Assert.NotNull(options.Hosts);
        Assert.Equal(2, options.Hosts.Length);
        Assert.Contains("localhost", options.Hosts);
    }
}
