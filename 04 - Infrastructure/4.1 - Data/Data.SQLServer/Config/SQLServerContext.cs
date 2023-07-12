using Data.SQLServer.Mappings;
using Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.SQLServer.Config;
public class SQLServerContext : DbContext
{
    public SQLServerContext()
    { }

    public SQLServerContext(DbContextOptions<SQLServerContext> options) : base(options)
    { }

    public virtual DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfigurations());
    }
}
