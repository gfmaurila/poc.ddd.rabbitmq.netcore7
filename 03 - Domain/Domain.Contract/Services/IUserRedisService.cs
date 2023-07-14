using Application.DTOs;

namespace Domain.Contract.Services;

public interface IUserRedisService
{
    Task CreateRedis();
    Task CreateRedis(string key, UserListDto dto, int db);
}
