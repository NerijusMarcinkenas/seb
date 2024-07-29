using Microsoft.EntityFrameworkCore;
using Seb.Server.Domain;
using Seb.Server.Infrastructure;

namespace Seb.Server.Application;

public interface ICurrencyRepository
{
    void Add(CurrencyData currencyData);
    Task<CurrencyData?> GetLatestRates();
    Task<DateTime?> GetLatestFetchDate();
}

public class CurrencyRepository : ICurrencyRepository
{
    private readonly AppDbContext _context;

    public CurrencyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CurrencyData?> GetLatestRates()
    {
        return await _context.Currencies
            .OrderByDescending(x => x.DateStamp)
            .FirstOrDefaultAsync();
    }

    public async Task<DateTime?> GetLatestFetchDate()
    {
        return await _context.Currencies
            .OrderByDescending(x => x.DateStamp)
            .Select(x => x.DateStamp)
            .FirstOrDefaultAsync();
    }

    public void Add(CurrencyData currency)
    {
        _context.Attach(currency);
    }

    public IReadOnlyCollection<CurrencyData> GetLastThreeMonths(DateTime now)
    {
        var dateFrom = now.AddMonths(-3);
        var dateTo = now;

        return _context.Currencies
            .Where(c => c.DateStamp >= dateFrom && c.DateStamp <= dateTo)
            .ToList();
    }
}