using Application.DTOs;
using Application.Responses;
using Ardalis.Result;
using Domain.Core.Model;

namespace Domain.Contract.Services;

public interface IUserService
{
    Task<Result<CreatedUserResponse>> Create(UserDto dto);
    Task<Result> Update(UserDto dto);
    Task<Result> Remove(int id);
    Task<Result<UserListDto>> Get(int id);
    Task<Result<List<UserListDto>>> Get();
    Task<Result<PaginationResult<UserListDto>>> GetAllAsync(int pageNumber, int pageSize);
}
