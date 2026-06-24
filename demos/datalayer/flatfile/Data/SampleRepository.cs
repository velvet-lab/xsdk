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

using xSdk.Data;

namespace xSdk.Demos.Data;

internal class SampleRepository : FlatFileRepository<SampleEntity>, ISampleRepository
{
    public Task AddSamplesAsync(SampleEntity[] samples, CancellationToken token = default) =>
        ExecuteAsDemoIfEnabledAsync(repo => repo.InsertAsync(samples, token), token);

    public Task<IEnumerable<SampleEntity>?> GetSamplesAsync(CancellationToken token = default) =>
        ExecuteAsDemoIfEnabledAsync(repo => repo.SelectListAsync(token), token);

    protected override Task<IEnumerable<SampleEntity>> CreateFakesAsync(CancellationToken token = default) =>
        FakeGenerator.GenerateListAsync<SampleEntityFakes, SampleEntity>(10);
}
