using Application.Services;
using Application.Services.Event;
using Application.Validators;
using Data.Repository;
using Data.Repository.MongoDb;
using Data.Repository.Redis;
using Data.Repository.Repositories;
using Data.Repository.Repositories.EF;
using Data.SQLServer.Config;
using Domain.Contract.MongoDb;
using Domain.Contract.Redis;
using Domain.Contract.Repositories;
using Domain.Contract.Services;
using Domain.Contract.Services.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using StackExchange.Redis;

namespace IOC;
public class Config
{
    public static void ConfigDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SQLServerContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
    }

    public static void ConfigMongoDb(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMongoClient>(new MongoClient(configuration.GetConnectionString("MongoDB")));
    }

    public static void ConfigRedis(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));

        services.AddScoped<IDatabase>(x =>
        {
            var multiplexer = x.GetRequiredService<IConnectionMultiplexer>();
            return multiplexer.GetDatabase();
        });

        services.AddScoped<ICacheRepository, CacheRepository>();
    }

    public static void ConfigRepository(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserEFRepository>();
        services.AddSingleton<ILogRepository, LogRepository>();
    }

    public static void ConfigService(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }

    public static void ConfigEvent(IServiceCollection services)
    {
        services.AddScoped<IErrorEvent, BadRequestEvent>();
        services.AddScoped<IErrorEvent, InternalServerErrorEvent>();
        services.AddScoped<IErrorEvent, NotFoundEvent>();
    }

    public static void ConfigValidator(IServiceCollection services)
    {
        services.AddScoped<CreateUserValidator>();
    }

    public static void ConfigBusService(IServiceCollection services)
    {
    }
}
