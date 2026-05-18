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

namespace xSdk.Hosting;

public static class TestRunnerDetection
{
    private static bool? _areUnitTestsRunning;

    public static bool AreUnitTestsRunning => _areUnitTestsRunning ??= IsTestRunnerFound();

    private static bool IsTestRunnerFound()
    {
        const string testRunnerPrefix = "xunit.runner";
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

#pragma warning disable CS8602 // Dereferenzierung eines möglichen Nullverweises.
        bool runnerFound = assemblies
            .Select(x => x.FullName)
            .Where(x => x != null)
            .Any(x => x.StartsWith(testRunnerPrefix, StringComparison.Ordinal));
#pragma warning restore CS8602 // Dereferenzierung eines möglichen Nullverweises.

        return runnerFound;
    }
}
