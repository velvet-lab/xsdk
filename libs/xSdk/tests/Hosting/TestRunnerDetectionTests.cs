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

namespace xSdk.Hosting;

public class TestRunnerDetectionTests
{
    [Fact]
    public void AreUnitTestsRunning_ReturnsBooleanValue()
    {
        // Validates the property is accessible and returns a boolean
        var result = TestRunnerDetection.AreUnitTestsRunning;

        Assert.IsType<bool>(result);
    }

    [Fact]
    public void AreUnitTestsRunning_CalledTwice_ReturnsSameValue()
    {
        var first = TestRunnerDetection.AreUnitTestsRunning;
        var second = TestRunnerDetection.AreUnitTestsRunning;

        Assert.Equal(first, second);
    }
}
