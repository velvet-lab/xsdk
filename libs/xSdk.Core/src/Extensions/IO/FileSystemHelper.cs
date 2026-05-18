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

using System.Reflection;
using Zio;
using Zio.FileSystems;

namespace xSdk.Extensions.IO;

public static class FileSystemHelper
{
    public static string GetExecutingFolder()
    {
        string folder = Environment.CurrentDirectory;
        var assembly = Assembly.GetEntryAssembly();
        if (assembly != null)
        {
            folder = Path.GetDirectoryName(assembly.Location) ?? folder;
        }

        return folder;
    }

    public static string? NormalizePath(string? path)
    {
        if (!string.IsNullOrEmpty(path))
        {
            if (!path.StartsWith('/'))
            {
                path = "/" + path;
            }
        }

        return path;
    }

    public static UPath GetFullPath(string path)
    {
        var fs = new PhysicalFileSystem();
        return fs.ConvertPathFromInternal(path);
    }

    public static string CreateSpecificDataFolder(IFileSystemResult fileSystem, string folder) => CreateSpecificDataFolder(fileSystem.Data, folder);

    public static string CreateSpecificDataFolder(IFileSystem fileSystem, string folder)
    {
        if (!fileSystem.DirectoryExists(folder))
        {
            fileSystem.CreateDirectory(folder);
        }

        return fileSystem.GetFullPath(folder);
    }

    public static string SearchGitRoot(string? root)
    {
        if (string.IsNullOrEmpty(root) || new DirectoryInfo(root).Parent == null)
        {
            return System.Environment.CurrentDirectory;
        }

        if (IsGitRoot(root))
        {
            return root;
        }

        return SearchGitRoot(Path.Combine(root, ".."));
    }

    private static bool IsGitRoot(string root)
    {
        string current = Path.Combine(root, ".git");
        if (!string.IsNullOrEmpty(current) && Directory.Exists(current))
        {
            return true;
        }

        return false;
    }

    public static bool IsDirectoryWritable(DirectoryInfo dir)
        => IsDirectoryWritable(dir.FullName);

    public static bool IsDirectoryWritable(string dirPath)
    {
        try
        {
            if (Directory.Exists(dirPath))
            {
                using FileStream fs = File.Create(Path.Combine(dirPath, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose);

                // Nothing to do here, just testing if we can create a file in the directory

                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    public static bool IsDirectoryReadable(DirectoryInfo dir)
        => IsDirectoryReadable(dir.FullName);

    public static bool IsDirectoryReadable(string dirPath)
    {
        try
        {
            if (Directory.Exists(dirPath))
            {
                Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
}
