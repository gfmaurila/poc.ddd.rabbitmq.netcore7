using Data.SQLServer.Config;
using Domain.Contract.Repositories;

namespace Data.Repository;


public class UnitOfWork : IUnitOfWork
{
    private readonly SQLServerContext _context;

    public UnitOfWork(SQLServerContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}