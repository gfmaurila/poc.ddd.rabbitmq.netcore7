namespace Domain.Contract.Redis;
public interface ICacheRepository
{
    Task SetAsyncAll<T>(string key, List<T> entity, TimeSpan tempo);
    Task SetAsync<T>(string key, T entity, TimeSpan tempo);
    Task SetAsync<T>(string key, T entity);
    Task<T> StringGetAsync<T>(string key);
    Task<List<T>> StringGetAllByKeyAsync<T>(string key);
    Task<List<T>> StringGetAllAsync<T>();
}