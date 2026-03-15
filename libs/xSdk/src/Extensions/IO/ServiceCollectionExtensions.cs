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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using xSdk.Hosting;

namespace xSdk.Extensions.IO;

public static class ServiceCollectionExtensions
{
    private static bool _isLocked;

    public static IServiceCollection AddFileServices(this IServiceCollection services)
    {
        services.TryAddSingleton(provider =>
        {
            _isLocked = true;
            return SlimHost.Instance.FileSystem;
        });

        return services;
    }

    internal static IServiceCollection AddSlimFileServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IFileSystemService>(provider =>
        {
            if (!_isLocked)
            {
                return ActivatorUtilities.CreateInstance<FileSystemService>(provider);
            }
            else
            {
                throw new SdkException("FileServices are locked and cannot be used anymore over SlimHost");
            }
        });

        return services;
    }
}
