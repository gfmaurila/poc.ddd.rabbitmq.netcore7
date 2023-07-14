using Domain.Contract.Redis;
using Domain.Core.Model;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Data.Repository.Redis;

public class CacheRepository : ICacheRepository
{
    private readonly IConnectionMultiplexer _multiplexer;

    public CacheRepository(IConnectionMultiplexer multiplexer)
    {
        _multiplexer = multiplexer;
    }

    public async Task CreateBatch<T>(string key, T entity, int database = 0)
    {
        IDatabase db = _multiplexer.GetDatabase(database);
        var serializedEntity = JsonConvert.SerializeObject(entity);

        var tran = db.CreateTransaction();
        tran.StringSetAsync(key, serializedEntity);

        await tran.ExecuteAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="entity"></param>
    /// <param name="tempo"></param>
    /// <param name="database"></param>
    /// <returns></returns>
    public async Task CreateBatch<T>(string key, T entity, TimeSpan tempo, int database = 0)
    {
        IDatabase db = _multiplexer.GetDatabase(database);
        var serializedEntity = JsonConvert.SerializeObject(entity);

        var tran = db.CreateTransaction();
        tran.StringSetAsync(key, serializedEntity, tempo);

        await tran.ExecuteAsync();
    }

    /// <summary>
    /// Exemplo de uso
    /// var mapperdto = _mapper.Map<UserDto>(objEntity);
    /// await _repoCache.SetAsync($"user_id_{mapperdto.Id}", mapperdto, 0);
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="entity"></param>
    /// <param name="database"></param>
    /// <returns></returns>
    public async Task SetAsync<T>(string key, T entity, int database = 0)
    {
        IDatabase _database = _multiplexer.GetDatabase(database);
        var serializedEntity = JsonConvert.SerializeObject(entity);
        await _database.StringSetAsync(key, serializedEntity);
    }

    public async Task SetAsync<T>(string key, T entity, TimeSpan tempo, int database = 0)
    {
        IDatabase _database = _multiplexer.GetDatabase(database);
        var serializedEntity = JsonConvert.SerializeObject(entity);
        await _database.StringSetAsync(key, serializedEntity, tempo);
    }

    public async Task SetAsyncAll<T>(string key, List<T> entity, TimeSpan tempo, int database = 0)
    {
        IDatabase _database = _multiplexer.GetDatabase(database);
        var serializedEntity = JsonConvert.SerializeObject(entity);
        await _database.StringSetAsync(key, serializedEntity, tempo);
    }

    public async Task<PaginationResult<T>> StringGetAllAsync<T>(int pageNumber, int pageSize, int database = 0)
    {
        IDatabase _database = _multiplexer.GetDatabase(database);
        var keys = _multiplexer.GetServer(_multiplexer.GetEndPoints().First()).Keys().ToArray();
        var totalCount = keys.Length;
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var startIndex = (pageNumber - 1) * pageSize;
        var endIndex = startIndex + pageSize - 1;
        var results = new List<T>();

        for (int i = startIndex; i <= endIndex && i < keys.Length; i++)
        {
            var key = keys[i];
            var value = await _database.StringGetAsync(key);
            if (!value.IsNullOrEmpty)
            {
                var result = JsonConvert.DeserializeObject<T>(value);
                results.Add(result);
            }
        }

        return new PaginationResult<T>
        {
            Data = results,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            QtdPge = totalPages
        };
    }

    public async Task<List<T>> StringGetAllAsync<T>(int database = 0)
    {
        IDatabase _database = _multiplexer.GetDatabase(database);
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

    public async Task<List<T>> StringGetAllByKeyAsync<T>(string key, int database = 0)
    {
        IDatabase _database = _multiplexer.GetDatabase(database);
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return null;
        return JsonConvert.DeserializeObject<List<T>>(value);
    }

    public async Task<T> StringGetAsync<T>(string key, int database = 0)
    {
        IDatabase _database = _multiplexer.GetDatabase(database);
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return default;
        return JsonConvert.DeserializeObject<T>(value);
    }
}
