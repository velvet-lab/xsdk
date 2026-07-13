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

namespace xSdk.Extensions.Commands;

public abstract class CommandHandler : ICommandHandler
{
    internal bool IsAsyncOverridden => HasMethodBeenOverridden(GetType().GetMethod(nameof(ICommandHandler.ExecuteAsync)));

    protected internal CommandContext? Context { get; internal set; }

    public virtual int Execute() => 0;

    public virtual Task<int> ExecuteAsync(CancellationToken cancellationToken)
        => Task.FromResult(0);

    /// <summary>
    /// Internal method to check if a method has been overridden by a derived class
    /// </summary>
    /// <param name="method">The method to check</param>
    /// <returns>True if the method has been overridden</returns>
    private static bool HasMethodBeenOverridden(MethodInfo? method)
    {
        if (method == null)
        {
            return false;
        }

        // Check if this is the actual implementation in CommandHandler
        // If a child class overrides this method, it won't be the same implementation
        if (method.DeclaringType == typeof(CommandHandler))
        {
            // This is the base implementation - we don't consider this as overridden
            return false;
        }

        // If we got here, it means this method has been overridden in a derived class
        return true;
    }


}
