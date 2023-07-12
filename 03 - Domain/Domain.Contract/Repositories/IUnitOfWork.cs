namespace Domain.Contract.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task SaveChangesAsync();
}