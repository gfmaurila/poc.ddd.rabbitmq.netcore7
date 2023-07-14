using Application.DTOs;
using AutoMapper;
using Domain.Contract.Redis;
using Domain.Contract.Repositories;
using Domain.Contract.Services;

namespace Application.Services;
public class UserRedisService : IUserRedisService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _repo;
    private readonly ICacheRepository _repoCache;

    public UserRedisService(
                           IMapper mapper,
                           IUserRepository repo,
                           ICacheRepository repoCache)
    {
        _mapper = mapper;
        _repo = repo;
        _repoCache = repoCache;
    }

    #region CreateRedis
    public async Task CreateRedis()
    {
        var users = await _repo.Get();
        foreach (var user in users)
        {
            var mapperdto = _mapper.Map<UserDto>(user);
            await _repoCache.CreateBatch($"user_id_{mapperdto.Id}", mapperdto, TimeSpan.FromHours(1), 0);
        }
    }

    public async Task CreateRedis(string key, UserListDto dto, int db)
    {
        await _repoCache.SetAsync(key, dto, db);
    }
    #endregion
}
