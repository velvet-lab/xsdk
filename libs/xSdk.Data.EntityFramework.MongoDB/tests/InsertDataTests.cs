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

#if NET8_0
public class InsertDataTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    [Fact]
    public async Task InsertData()
    {
        var factory = fixture.Factory;
        var repo = factory.CreateRepository<ITestRepository>(Globals.DatalayerName);

        var fakes = FakeGenerator.GenerateList<TestEntityFakes, TestEntity>(10);
        await repo.AddDataAsync(fakes);

        var entities = await repo.GetDataAsync();

        Assert.NotNull(entities);
        Assert.Equal(fakes.Count(), entities.Count());

        await repo.RemoveAll();
    }
}
#endif
