using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using TestCosmo;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(configuration =>
{
    configuration.UseDashboardMetrics().UseMongoStorage("mongodb://localhost:27017/hangfire", new MongoStorageOptions
    {
        MigrationOptions = new MongoMigrationOptions
        {
            MigrationStrategy = new MigrateMongoMigrationStrategy(),
            BackupStrategy = new CollectionMongoBackupStrategy()
        },
        Prefix = "hangfire.mongo",
        CheckConnection = true
    });
});
builder.Services.AddTransient<ICodeGenerator, CodeGenerator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

string[] stringArray = new string[] {
    "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", 
    "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", 
    "AU", "AV", "AW", "AX", "AY", "AZ", "A0", "A1", "A2", "A3", 
    "A4", "A5", "A6", "A7", "A8", "A9", "BA", "BB", "BC", "BD"
};


app.MapGet("/GenerateCodes", (ICodeGenerator codeGen) =>
    {
        codeGen.AddBGJob(stringArray);
    })
    .WithName("GenerateCodes")
    .WithOpenApi();

app.Run();

