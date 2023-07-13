using Application.DTOs;
using Application.Responses;
using Ardalis.Result;

namespace Domain.Contract.Services;

public interface IUserService
{
    Task<Result<CreatedUserResponse>> Create(UserDto dto);
    Task CreateRedis();
    Task CreateRedis(UserListDto dto);
    Task CreateRedisDelete(UserListDto dto);
    Task<Result> Update(UserDto dto);
    Task<Result> Remove(int id);
    Task<Result<UserListDto>> Get(int id);
    Task<Result<List<UserListDto>>> Get();
}
