using MediatR;
using Microsoft.AspNetCore.Mvc;
using Seb.Server.Application;
using Seb.Server.Application.Queries;

namespace Seb.Server.Server.Controllers;
[Route("api/currency")]
public class CurrencyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrencyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("latest")]
    public async Task<IReadOnlyCollection<CurrencyModel>> GetLatestRates(CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetLatestRates(), cancellationToken);
    }
    
    [HttpGet("exchange-rate")]
    public async Task<ActionResult<decimal>> CalculateCurrenciesRate(
        [FromQuery] CalculateCurrenciesRate request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        return result.ToActionResult();
    }
}