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

namespace xSdk.Extensions.IO;

public interface IFileSystemService
{
    IFileSystemResult RequestFileSystem(FileSystemContext context = FileSystemContext.None) => RequestFileSystemAsync(context).GetAwaiter().GetResult();

    Task<IFileSystemResult> RequestFileSystemAsync(FileSystemContext context = FileSystemContext.None, CancellationToken token = default);

    IFileSystemResult Local { get; }

    IFileSystemResult User { get; }

    IFileSystemResult Machine { get; }
}
