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

public interface IVaultRepository : IReadOnlyVaultRepository
{

    private const string DefaultPath = "default";

    Task<bool> AddSecretAsync(string key, object data, CancellationToken token = default)
        => AddSecretAsync(null, DefaultPath, key, data, token);

    Task<bool> AddSecretAsync(string path, string key, object data, CancellationToken token = default)
        => AddSecretAsync(null, path, key, data, token);

    Task<bool> AddSecretAsync(string? mountpoint, string path, string key, object data, CancellationToken token = default)
        => AddSecretAsync(mountpoint, path, new Dictionary<string, object> { { key, data } }, token);



    Task<bool> AddSecretAsync(Dictionary<string, object> data, CancellationToken token = default)
        => AddSecretAsync(null, DefaultPath, data, token);

    Task<bool> AddSecretAsync(string path, Dictionary<string, object> data, CancellationToken token = default)
        => AddSecretAsync(null, path, data, token);

    Task<bool> AddSecretAsync(string? mountpoint, string path, Dictionary<string, object> data, CancellationToken token = default);
}
