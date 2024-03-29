using Hangfire;
using MongoDB.Bson;
using MongoDB.Driver;
using TestCosmo.Data;

namespace TestCosmo;

public  class CodeGenerator : ICodeGenerator
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly ILogger<CodeGenerator> _logger;
    public CodeGenerator(IBackgroundJobClient backgroundJobClient, ILogger<CodeGenerator> logger)
    {
        _backgroundJobClient = backgroundJobClient;
        _logger = logger;
    }

    public void AddCodeGenerationBGJobs(string[] stringArray, int count)
    {
        foreach (var prefix in stringArray)
        {
            var jobId = _backgroundJobClient.Schedule(() => GenerateCodesAsync(prefix, count), new TimeSpan(0,0,0,1));
            _logger.LogInformation($"Job id: {jobId}");
        }
    }

    public async Task GenerateCodesAsync(string prefix, int count)
    {
        try
        {
            _logger.LogInformation($"Entered GenerateCodesAsync\nDate:{DateTime.Now}");

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("test");
            var collection = database.GetCollection<Code>("Codes");
        
            _logger.LogInformation($"Started generating codes for {prefix}\nDate:{DateTime.Now}");

            for (int i = 0; i < count; i++)
            {
                var guid = $"{prefix}-{Guid.NewGuid()}";

                var newCode = new Data.Code { Id = Guid.NewGuid(), CodeString = guid };
                await collection.InsertOneAsync(newCode);
            }

            _logger.LogInformation($"Ended generating codes for {prefix}\\nDate:{DateTime.Now}");
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.ToString());
            throw;
        }
    }
}

public interface ICodeGenerator
{
    public void AddCodeGenerationBGJobs(string[] stringArray, int count);
    public Task GenerateCodesAsync(string prefix, int count);
}