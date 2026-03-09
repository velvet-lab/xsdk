using System.Text.Json;
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
            var json = @"
[
  {
    ""Name"": ""John Doe"",
    ""Age"": 25
  },
  {
    ""Name"": ""Jane Smith"",
    ""Age"": 30,
    ""Email"": ""jane.smith@example.com"",
    ""Department"": ""Marketing"",
    ""EmployeeId"": 12346,
    ""IsActive"": true,
    ""StartDate"": ""2020-01-15""
  },
  {
    ""Name"": ""Bob Johnson"",
    ""Age"": 45,
    ""EmployeeId"": 12347,
    ""IsActive"": false,
    ""location"": {
        ""Email"": ""bob.johnson@example.com"",
        ""Department"": ""Sales"",
        ""EndDate"": ""2023-12-31""
    }
  }
]
";
            // Load the the Repository
            var sampleRepo = dbFactory.CreateRepository<ISampleRepository>();

            // Convert the Models to Entities
            var models = JsonSerializer.Deserialize<IEnumerable<SampleModel>>(json, JsonHelper.GetSerializerOptions());
            var entities = models.ToEntity<SampleMappingProfile, SampleEntity>();

            // Add this Samples to Database
            await sampleRepo.AddSamplesAsync(entities);
        }
        catch
        {
            throw;
        }
    }
}
