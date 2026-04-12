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

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using xSdk.Data;
using xSdk.Demos.Data;

namespace xSdk.Demos.Hosting;

public class MyDataHost(IDatalayerFactory dbFactory, ILogger<MyDataHost> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("First add some data to database");
        await AddData(cancellationToken);

        logger.LogInformation("Load data from database");
        await LoadData(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task LoadData(CancellationToken token)
    {
        try
        {
            // Load the the Repository
            var sampleRepo = dbFactory.CreateRepository<ISampleRepository>();

            // Load the Samples from the Database
            var entities = await sampleRepo.GetSamplesAsync();

            // Map the result to Modesl
            var models = entities.ToModel<SampleMappingProfile, SampleModel>();

            // Write Results tu Console
            var table = new Table();
            table.AddColumn("Name");
            table.AddColumn("Age");
            foreach (var model in models)
            {
                table.AddRow(model.Name, model.Age.ToString());
            }
            AnsiConsole.Write(table);
        }
        catch
        {
            throw;
        }
    }

    private async Task AddData(CancellationToken token)
    {
        try
        {
            // Load the the Repository
            var sampleRepo = dbFactory.CreateRepository<ISampleRepository>();
            var secondRepo = dbFactory.CreateRepository<ISecondSampleRepository>(DbProviderNames.Second);

            // Create some Sample Entities
            var samples = new SampleEntity[]
            {
                new SampleEntity { Name = "Customer 1", Age = 10 },
                new SampleEntity { Name = "Customer 2", Age = 10 },
                new SampleEntity { Name = "Customer 3", Age = 10 },
            };

            // Add this Samples to Database
            await sampleRepo.AddSamplesAsync(samples);
        }
        catch
        {
            throw;
        }
    }
}
