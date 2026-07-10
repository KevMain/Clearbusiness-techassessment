using TechnicalAssessment.Application;
using TechnicalAssessment.Domain;
using TechnicalAssessment.Infrastructure;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

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

app.UseAuthentication();
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
