using MediatR;

namespace Seb.Server.Application.Queries;

public record GetLatestRates : IRequest<IReadOnlyCollection<CurrencyModel>>;

public class GetLatestRatesHandler : IRequestHandler<GetLatestRates, IReadOnlyCollection<CurrencyModel>>
{
    private readonly ICurrencyRepository _currencyRepository;

    public GetLatestRatesHandler(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<IReadOnlyCollection<CurrencyModel>> Handle(GetLatestRates request,
        CancellationToken cancellationToken)
    {
        var latestRates = await _currencyRepository.GetLatestRates();
        if (latestRates is null)
        {
            return Array.Empty<CurrencyModel>();
        }

        return latestRates.Currencies.Select(x =>
                new CurrencyModel(
                    x.Name,
                    x.Code,
                    x.Rate,
                    x.DateStamp))
            .ToList();
    }
}