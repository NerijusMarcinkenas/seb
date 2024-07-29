using Seb.Server.Domain.Common;

namespace Seb.Server.Domain;

public class CurrencyData
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public CurrencyData() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public CurrencyData(IReadOnlyCollection<Currency> currencies, DateTime dateStamp)
    {
        Currencies = currencies;
        DateStamp = dateStamp;
    }

    public int Id { get; }
    public IReadOnlyCollection<Currency> Currencies { get; }
    public DateTime DateStamp { get; }
    
    public static CurrencyData Create(IReadOnlyCollection<Currency> currencies, DateTime dateStamp)
    {
        if (currencies.Count == 0)
        {
            throw new ArgumentException("Currencies collection cannot be empty", nameof(currencies));
        }

        return new CurrencyData(currencies, dateStamp);
    }

    public Result<decimal> CalculateRate(string baseCurrency, string targetCurrency, decimal amount)
    {
        var baseCurrencyRate = Currencies.FirstOrDefault(c => c.Code == baseCurrency)?.Rate;
        var targetCurrencyRate = Currencies.FirstOrDefault(c => c.Code == targetCurrency)?.Rate;

        if (baseCurrencyRate == null)
        {
            return Result<decimal>.Error("Invalid base currency code");
        }
        if (targetCurrencyRate == null)
        {
            return Result<decimal>.Error("Invalid target currency code");
        }

        var amountInEur = amount / baseCurrencyRate.Value;
        var amountInTargetCurrency = amountInEur * targetCurrencyRate.Value;

        return new Result<decimal>(Math.Round(amountInTargetCurrency, 4, MidpointRounding.ToEven));
    }
}