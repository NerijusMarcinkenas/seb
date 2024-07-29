using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Seb.Server.Application;
using Seb.Server.Infrastructure;
using Seb.Server.Infrastructure.Clients;

namespace Seb.Server.Server;

public static class ProgramExtensions
{
    public static IServiceCollection AddMediatr(this IServiceCollection services)
    {
        services.AddMediatR(x =>
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }

    public static IServiceCollection AddPersistance(this IServiceCollection services, string connectionString)
    {
        services.AddDatabase(connectionString);
        services.AddRepositories();

        return services;
    }

    public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ICurrencyExchangeClient, CurrencyExchangeClient>(client =>
            client.BaseAddress = new Uri(configuration["ExchangeRatesApi:SourceUrl"]!));

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();

        return services;
    }
}