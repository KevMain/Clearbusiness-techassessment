using TechnicalAssessment.Application;
using TechnicalAssessment.Domain;
using TechnicalAssessment.Infrastructure;
using System.IO;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Application & Infrastructure DI
builder.Services.AddScoped<IHelloAppService, HelloAppService>();
builder.Services.AddScoped<IGreeter, Greeter>();
// In-memory data store for parsed CSVs
builder.Services.AddSingleton<TechnicalAssessment.Infrastructure.IDataStore, TechnicalAssessment.Infrastructure.InMemoryDataStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Initialize in-memory datastore from CSV files on startup
var dataStore = app.Services.GetRequiredService<TechnicalAssessment.Infrastructure.IDataStore>();

// DataFolder can be configured via configuration; fallback to tests TestData in repo
var dataFolder = builder.Configuration["DataFolder"];
if (string.IsNullOrWhiteSpace(dataFolder))
{
    // Try common relative paths to include test data shipped in the repo (go up two levels from web project's content root)
    dataFolder = Path.Combine(builder.Environment.ContentRootPath, "..", "..", "tests", "TechnicalAssessment.Integration.Tests", "TestData");
}

dataFolder = Path.GetFullPath(dataFolder);
    app.Logger.LogInformation("Loading CSV data from {DataFolder}", dataFolder);
    dataStore.LoadFromFolder(dataFolder);
    app.Logger.LogInformation("Loaded customers={Customers} orders={Orders} items={Items}", dataStore.Report.Customers.Successes.Count, dataStore.Report.Orders.Successes.Count, dataStore.Report.Items.Successes.Count);

app.Run();
