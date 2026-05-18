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
using xSdk.Extensions.Telemetry;
using xSdk.Hosting;

namespace xSdk.Plugins.Telemetry;

public class TelemetryServiceTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    private ITelemetryService GetService()
    {
        var host = fixture
            .ConfigureBuilder(builder => builder.EnableTelemetry())
            .BuildHost();
        return host.Services.GetRequiredService<ITelemetryService>();
    }

    [Fact]
    public void GetService_ITelemetryService_IsRegistered()
    {
        var service = GetService();

        Assert.NotNull(service);
    }

    [Fact]
    public void MainActivitySource_ReturnsNonNull()
    {
        var service = GetService();

        var source = service.MainActivitySource;

        Assert.NotNull(source);
    }

    [Fact]
    public void MainActivitySource_CalledTwice_ReturnsSameInstance()
    {
        var service = GetService();

        var first = service.MainActivitySource;
        var second = service.MainActivitySource;

        Assert.Same(first, second);
    }

    [Fact]
    public void MainMeter_ReturnsNonNull()
    {
        var service = GetService();

        var meter = service.MainMeter;

        Assert.NotNull(meter);
    }

    [Fact]
    public void MainMeter_CalledTwice_ReturnsSameInstance()
    {
        var service = GetService();

        var first = service.MainMeter;
        var second = service.MainMeter;

        Assert.Same(first, second);
    }

    [Fact]
    public void CreateCounter_WithValidName_ReturnsCounter()
    {
        var service = GetService();

        var counter = service.CreateCounter<long>("test.counter", "requests", "A test counter");

        Assert.NotNull(counter);
    }

    [Fact]
    public void CreateHistogram_WithValidName_ReturnsHistogram()
    {
        var service = GetService();

        var histogram = service.CreateHistogram<double>("test.histogram", "ms", "A test histogram");

        Assert.NotNull(histogram);
    }

    [Fact]
    public void CreateUpDownCounter_WithValidName_ReturnsCounter()
    {
        var service = GetService();

        var counter = service.CreateUpDownCounter<int>("test.updown", "items", "A test updown counter");

        Assert.NotNull(counter);
    }

    [Fact]
    public void CreateObservableCounter_WithValidName_ReturnsCounter()
    {
        var service = GetService();

        var counter = service.CreateObservableCounter<long>("test.obs.counter", () => 42L);

        Assert.NotNull(counter);
    }

    [Fact]
    public void CreateObservableGauge_WithValidName_ReturnsGauge()
    {
        var service = GetService();

        var gauge = service.CreateObservableGauge<double>("test.gauge", () => 3.14);

        Assert.NotNull(gauge);
    }

    [Fact]
    public void CreateObservableUpDownCounter_WithValidName_ReturnsCounter()
    {
        var service = GetService();

        var counter = service.CreateObservableUpDownCounter<int>("test.obs.updown", () => 10);

        Assert.NotNull(counter);
    }
}
