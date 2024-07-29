using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Seb.Server.Application.Commands;

namespace Seb.Functions;

public class FetchCurrenciesFunction
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public FetchCurrenciesFunction(ILoggerFactory loggerFactory, IMediator mediator)
    {
        _mediator = mediator;
        _logger = loggerFactory.CreateLogger<FetchCurrenciesFunction>();
    }

    [Function("FetchCurrenciesFunction")]
    public async Task Run([TimerTrigger("%CurrenciesFetchCron%")] TimerInfo myTimer)
    {
        _logger.LogInformation("Starting to fetch currencies");

        await _mediator.Send(new FetchCurrencies(DateTime.UtcNow));
        
        _logger.LogInformation("Finished fetching currencies");
    }
}