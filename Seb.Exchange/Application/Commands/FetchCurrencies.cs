using MediatR;
using Seb.Server.Domain;
using Seb.Server.Infrastructure;
using Seb.Server.Infrastructure.Clients;

namespace Seb.Server.Application.Commands;

public record FetchCurrencies(DateTime Now) : IRequest;

public class FetchCurrenciesHandler : IRequestHandler<FetchCurrencies>
{
    private readonly ICurrencyRepository _currencyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrencyExchangeClient _currencyExchangeClient;

    public FetchCurrenciesHandler(ICurrencyRepository currencyRepository, IUnitOfWork unitOfWork,
        ICurrencyExchangeClient currencyExchangeClient)
    {
        _currencyRepository = currencyRepository;
        _unitOfWork = unitOfWork;
        _currencyExchangeClient = currencyExchangeClient;
    }

    public async Task Handle(FetchCurrencies request, CancellationToken cancellationToken)
    {
        var currenciesResult = await _currencyExchangeClient.FetchCurrencyExchangeRates();

        var latestFetchDate = await _currencyRepository.GetLatestFetchDate();
        
        if (currenciesResult.DateStamp >= latestFetchDate)
        {
            var rates = currenciesResult.Rates
                .Select(currency =>
                    Currency.Create(
                        currency.Name,
                        currency.Code,
                        currency.Rate,
                        currenciesResult.DateStamp)).ToList();

            _currencyRepository.Add(CurrencyData.Create(rates, currenciesResult.DateStamp));
            await _unitOfWork.SaveChanges(cancellationToken);
        }
    }
}