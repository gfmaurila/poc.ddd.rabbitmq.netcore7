using Domain.Core.Entities;
using Domain.Core.Model;
using Domain.Core.ValueObjects;

namespace Domain.Contract.Repositories;
public interface IUserRepository : IBaseRepository<User>
{
    Task<bool> ExistsByEmailAsync(Email email);
    Task<bool> ExistsByEmailAsync(Email email, int id);
    Task<User> GetUserByEmailAndPasswordAsync(string email, string passwordHash);
    Task<PaginationResult<User>> GetAllAsync(int pageNumber, int pageSize);
}
