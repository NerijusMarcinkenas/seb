using Microsoft.EntityFrameworkCore;
using Seb.Server.Domain;
using Seb.Server.Infrastructure.Persistence.EntityTypeConfigurations;

namespace Seb.Server.Infrastructure;

public interface IUnitOfWork
{
    Task SaveChanges(CancellationToken cancellationToken = default);
}

public class AppDbContext : DbContext, IUnitOfWork
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CurrencyDataConfiguration).Assembly);
    }

    public DbSet<CurrencyData> Currencies => Set<CurrencyData>();

    public Task SaveChanges(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}