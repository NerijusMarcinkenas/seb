using MediatR;
using Seb.Server.Domain.Common;

namespace Seb.Server.Application.Queries;

public record CalculateCurrenciesRate(
    string BaseCurrency,
    string TargetCurrency,
    decimal Amount) : IRequest<Result<decimal>>;

public class CalculateCurrenciesRateHandler : IRequestHandler<CalculateCurrenciesRate, Result<decimal>>
{
    private readonly ICurrencyRepository _currencyRepository;

    public CalculateCurrenciesRateHandler(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<Result<decimal>> Handle(CalculateCurrenciesRate request, CancellationToken cancellationToken)
    {
        var currency = await _currencyRepository.GetLatestRates();

        if (currency is null)
        {
            throw new InvalidOperationException("No currency data found");
        }

        var result = currency.CalculateRate(request.BaseCurrency, request.TargetCurrency, request.Amount);
        
        return result;
    }
}