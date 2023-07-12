using Application.Services.Middleware;
using AutoMapper;
using HealthChecks.UI.Client;
using IOC;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using WebAPI.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Config.ConfigDbContext(builder.Services, builder.Configuration);
Config.ConfigMongoDb(builder.Services, builder.Configuration);
Config.ConfigRedis(builder.Services, builder.Configuration);
Config.ConfigRepository(builder.Services);
Config.ConfigService(builder.Services);
Config.ConfigValidator(builder.Services);
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(builder.Configuration);
});

builder.Services.AddHttpClient();
Config.ConfigBusService(builder.Services);

builder.Services.AddSingleton(
        new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        }).CreateMapper());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration(builder.Configuration);
builder.Services.AddHealthChecks(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ErrorLoggingMiddleware>();

app.MapControllers();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker") || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Gera o endpoint que retornará os dados utilizados no dashboard
app.UseHealthChecks("/healthchecks-data-ui", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Ativa o dashboard para a visualização da situação de cada Health Check
app.UseHealthChecksUI(options =>
{
    options.UIPath = "/monitor";
});

app.Run();
