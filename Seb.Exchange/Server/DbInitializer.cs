using Microsoft.EntityFrameworkCore;
using Seb.Server.Infrastructure;

namespace Seb.Server.Server;

public class DbInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DbInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync(cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}