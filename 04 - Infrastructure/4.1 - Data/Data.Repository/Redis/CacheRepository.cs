using Domain.Contract.Redis;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Data.Repository.Redis;

public class CacheRepository : ICacheRepository
{
    private readonly IDatabase _database;
    private readonly IConnectionMultiplexer _multiplexer;

    public CacheRepository(IConnectionMultiplexer multiplexer)
    {
        _multiplexer = multiplexer;
        _database = _multiplexer.GetDatabase();
    }

    public async Task SetAsync<T>(string key, T entity)
    {
        var serializedEntity = JsonConvert.SerializeObject(entity);
        await _database.StringSetAsync(key, serializedEntity);
    }

    public async Task SetAsync<T>(string key, T entity, TimeSpan tempo)
    {
        var serializedEntity = JsonConvert.SerializeObject(entity);
        await _database.StringSetAsync(key, serializedEntity, tempo);
    }

    public async Task SetAsyncAll<T>(string key, List<T> entity, TimeSpan tempo)
    {
        var serializedEntity = JsonConvert.SerializeObject(entity);
        await _database.StringSetAsync(key, serializedEntity, tempo);
    }

    public async Task<List<T>> StringGetAllAsync<T>()
    {
        var keys = _multiplexer.GetServer(_multiplexer.GetEndPoints().First()).Keys();
        var results = new List<T>();

        foreach (var key in keys)
        {
            var value = await _database.StringGetAsync(key);
            if (!value.IsNullOrEmpty)
            {
                var result = JsonConvert.DeserializeObject<T>(value);
                results.Add(result);
            }
        }

        return results;
    }

    public async Task<List<T>> StringGetAllByKeyAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return null;
        return JsonConvert.DeserializeObject<List<T>>(value);
    }

    public async Task<T> StringGetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return default;
        return JsonConvert.DeserializeObject<T>(value);
    }
}
