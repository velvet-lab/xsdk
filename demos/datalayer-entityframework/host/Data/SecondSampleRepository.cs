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

internal class SecondSampleRepository : EntityFrameworkRepository<SecondDbContext, SecondEntity>, ISecondSampleRepository
{
    public Task AddSamplesAsync(SecondEntity[] samples, CancellationToken token = default)
    {
        return this.InsertAsync(samples, token);
    }

    public Task<IEnumerable<SecondEntity>> GetSamplesAsync(CancellationToken token = default)
    {
        return this.SelectListAsync(token);
    }
}
