using Data.SQLServer.Config;
using Domain.Contract.Repositories;
using Domain.Core.Entities;
using Domain.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Repositories.EF;

public class UserEFRepository : BaseRepository<User>, IUserRepository
{
    private readonly SQLServerContext _context;
    public UserEFRepository(SQLServerContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByEmailAsync(Email email)
        => await _context.User.AsNoTracking().AnyAsync(f => f.Email.Address == email.Address);

    public async Task<bool> ExistsByEmailAsync(Email email, int id)
        => await _context.User.AsNoTracking().AnyAsync(f => f.Email.Address == email.Address && f.Id != id);

    public async Task<User> GetUserByEmailAndPasswordAsync(string email, string passwordHash)
    {
        var find = await _context.User.AsNoTracking().Where(u => u.Email.Address == email && u.Password == passwordHash).ToListAsync();
        return find.FirstOrDefault();
    }
}
