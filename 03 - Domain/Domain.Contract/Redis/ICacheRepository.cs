namespace Domain.Contract.Redis;
public interface ICacheRepository
{
    Task SetAsyncAll<T>(string key, List<T> entity, TimeSpan tempo, int database);
    Task SetAsync<T>(string key, T entity, TimeSpan tempo, int database);
    Task CreateBatch<T>(string key, T entity, int database = 0);
    Task CreateBatch<T>(string key, T entity, TimeSpan tempo, int database = 0);
    Task SetAsync<T>(string key, T entity, int database);
    Task<T> StringGetAsync<T>(string key, int database);
    Task<List<T>> StringGetAllByKeyAsync<T>(string key, int database);
    Task<List<T>> StringGetAllAsync<T>(int database);
}