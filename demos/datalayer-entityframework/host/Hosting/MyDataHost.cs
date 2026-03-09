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
            var sampleRepo = dbFactory.CreateRepository<ISampleRepository>(DbProviderNames.First);
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
