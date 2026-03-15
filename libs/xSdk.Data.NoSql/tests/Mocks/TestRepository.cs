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

namespace xSdk.Data.Mocks;

internal class TestRepository : NoSqlRepository<TestEntity>, ITestRepository
{
    public Task AddDataAsync(IEnumerable<TestEntity> samples, CancellationToken token = default)
    {
        return this.InsertAsync(samples, token);
    }

    public Task<IEnumerable<TestEntity>> GetDataAsync(CancellationToken token = default)
    {
        return this.SelectListAsync(token);
    }

    public async Task RemoveAll()
    {
        var entities = await this.SelectListAsync();
        foreach (var entity in entities)
        {
            await this.RemoveAsync(entity);
        }
    }
}
