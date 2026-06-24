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

namespace xSdk.Extensions.Telemetry;

public class VariableResourceDetectorTests
{
    [Fact]
    public void Detect_WithResources_ReturnsResourceWithAttributes()
    {
        var resources = new Dictionary<string, object>
        {
            { "service.name", "test-service" },
            { "service.version", "1.0.0" },
        };

        var detector = new VariableResourceDetector(resources);
        var result = detector.Detect();

        Assert.NotNull(result);
        var attrs = result.Attributes.ToDictionary(kv => kv.Key, kv => kv.Value);
        Assert.Equal("test-service", attrs["service.name"]);
        Assert.Equal("1.0.0", attrs["service.version"]);
    }

    [Fact]
    public void Detect_WithEmptyResources_ReturnsEmptyResource()
    {
        var resources = new Dictionary<string, object>();

        var detector = new VariableResourceDetector(resources);
        var result = detector.Detect();

        Assert.NotNull(result);
        Assert.Empty(result.Attributes);
    }

    [Fact]
    public void Constructor_NullResources_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new VariableResourceDetector(null!));
    }
}
