using Application.DTOs;
using Application.Responses;
using Ardalis.Result;

namespace Domain.Contract.Services;

public interface IUserService
{
    Task<Result<CreatedUserResponse>> Create(UserDto dto);
    Task<Result> Update(UserDto dto);
    Task<Result> Remove(int id);
    Task<Result<UserDto>> Get(int id);
    Task<Result<List<UserDto>>> Get();
}
