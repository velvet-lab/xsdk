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

using Zio;

namespace xSdk.Extensions.IO;

[CLSCompliant(false)]
public static class FileSystemExtensions
{
    public static string GetFullPath(this IFileSystem fileSystem) => fileSystem.GetFullPath("/");

    public static string GetFullPath(this IFileSystem fileSystem, UPath path)
    {
        var (fs, fullPath) = fileSystem.ResolvePath(path);
        return fs.ConvertPathToInternal(fullPath);
    }

    public static string GetFullPath(this IFileSystem fileSystem, string path)
    {
        var (fs, fullPath) = fileSystem.ResolvePath(path);
        return fs.ConvertPathToInternal(fullPath);
    }
}
