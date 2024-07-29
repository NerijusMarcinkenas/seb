using Microsoft.AspNetCore.Mvc;
using Seb.Server.Domain.Common;

namespace Seb.Server.Server;

public static class ResultExtensions
{
    public static ActionResult<T> ToActionResult<T>(
        this Result<T> result)
    {
        return result.IsSuccess
            ? new OkObjectResult(result.Value)
            : new BadRequestObjectResult(result.Message);
    }
}