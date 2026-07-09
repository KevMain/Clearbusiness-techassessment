using TechnicalAssessment.Application;
using TechnicalAssessment.Domain;
using TechnicalAssessment.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Application & Infrastructure DI
builder.Services.AddScoped<IHelloAppService, HelloAppService>();
builder.Services.AddScoped<IGreeter, Greeter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
