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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xSdk.Data.Mocks;

namespace xSdk.Data;

public class ConcurrentDbContextTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    [Fact]
    public async Task InsertData()
    {
        var factory = fixture.Factory;

        var task1 = Task.Run(async () =>
        {
            var repo1 = factory.CreateRepository<IConcurrentRepositoryOne>(Globals.DatalayerName);
            await repo1.AddDataAsync(Globals.ConcurrentEntitiesOne);
            var entities1 = await repo1.GetDataAsync();

            return entities1;
        });

        var task2 = Task.Run(async () =>
        {
            var repo2 = factory.CreateRepository<IConcurrentRepositoryTwo>(Globals.DatalayerName);
            await repo2.AddDataAsync(Globals.ConcurrentEntitiesTwo);
            var entities2 = await repo2.GetDataAsync();

            return entities2;
        });

        await Task.WhenAll(task1, task2);

        var entities1 = await task1;
        var entities2 = await task2;

        Assert.NotNull(entities1);
        Assert.Equal(Globals.ConcurrentEntitiesOne.Count(), entities1.Count());

        Assert.NotNull(entities2);
        Assert.Equal(Globals.ConcurrentEntitiesTwo.Count(), entities2.Count());
    }
}
