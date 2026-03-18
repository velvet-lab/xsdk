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

public class FlatFileCrudTests(LocalDatabaseFixture fixture) : IClassFixture<LocalDatabaseFixture>
{
    [Fact]
    public async Task SelectListAsync_EmptyStore_ReturnsEmptyCollection()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity>;

        var result = await repo!.SelectListAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task RemoveAsync_ByPrimaryKeyCollection_ThrowsWrappedNotImplementedException()
    {
        var factory = fixture.Factory;
        var testRepo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);
        var repo = testRepo as IRepository<TestEntity>;

        var primaryKeys = new[] { (IPrimaryKey)new GuidPK(Guid.NewGuid()) };

        await Assert.ThrowsAsync<NotImplementedException>(() => repo!.RemoveAsync(primaryKeys));
    }
}
