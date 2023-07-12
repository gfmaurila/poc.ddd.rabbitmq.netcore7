using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace WebAPI.Configuration;
public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Projeto DDD .NET Core 7",
                Version = "v1",
                Description = "Projeto DDD .NET Core 7",
                Contact = new OpenApiContact
                {
                    Name = "Guilherme F Maurila",
                    Email = "gfmaurila@gmail.com",
                    Url = new Uri("https://github.com/gfmaurila"),
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        return services;
    }

    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoConnection = configuration.GetSection("ConnectionStrings:MongoDB").Value;
        var sqlserverConnection = configuration.GetSection("ConnectionStrings:SqlConnection").Value;
        var redisConnection = configuration.GetSection("ConnectionStrings:Redis").Value;

        var rabbitMqConnection = configuration.GetSection("ConnectionStrings:RabbitMQ:Host").Value.Replace("rabbitmq", "amqp").Replace("//", $"//{configuration.GetSection("ConnectionStrings:RabbitMQ:Username").Value}:{configuration.GetSection("ConnectionStrings:RabbitMQ:Password").Value}@");

        // Cerviços através de Health Checks
        services.AddHealthChecks()

            // RabbitMQ
            .AddRabbitMQ(rabbitConnectionString: rabbitMqConnection,
                        name: "rabbitmq",
                        failureStatus: HealthStatus.Unhealthy,
                        tags: new string[] { "rabbitmq", "WebAPI" })

            // SqlServer
            .AddSqlServer(connectionString: sqlserverConnection,
                      healthQuery: "SELECT 1;",
                      name: "sqlserver",
                      failureStatus: HealthStatus.Unhealthy,
                      tags: new string[] { "db", "sqlserver", "WebAPI" })

            // MongoDb
            .AddMongoDb(mongodbConnectionString: mongoConnection,
                        name: "mongoserver",
                        timeout: new System.TimeSpan(0, 0, 0, 5),
                        failureStatus: HealthStatus.Unhealthy,
                        tags: new string[] { "db", "MongoDb", "mongoserver", "WebAPI" })

            // Redis
            .AddRedis(redisConnectionString: redisConnection,
                        name: "redis",
                        tags: new string[] { "redis", "cache", "WebAPI" },
                        timeout: new System.TimeSpan(0, 0, 0, 5),
                        failureStatus: HealthStatus.Unhealthy);

        services.AddHealthChecksUI().AddInMemoryStorage();

        return services;
    }
}
