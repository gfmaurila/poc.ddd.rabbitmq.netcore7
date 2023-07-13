using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace WebAPI.Configuration;
public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "DEMO.API", Version = "v1" });

            //Definição de segurança do Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header usando o esquema Bearer."
            });

            //
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });


        services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
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
