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

public class InsertDataTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    [Fact]
    public async Task InsertDataWithDefaultMountPointAndPath()
    {
        var repo = fixture.Factory.CreateRepository<IVaultRepository>();

        var fake = FakeGenerator.Generate<TestEntityFakes, TestEntity>();
        await repo.AddSecretAsync(fake.Key, fake.Value, TestContext.Current.CancellationToken);

        var data = await repo.GetSecretsAsync(TestContext.Current.CancellationToken);
        var entity = data.FirstOrDefault();

        Assert.Equal(fake.Key, entity.Key);
        Assert.Equal(fake.Value, entity.Value);
    }
}
