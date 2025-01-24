using System.Text.Json;

using api.Context;
using api.Models.Infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Features.FrontendLogging;

public class FrontendLoggingController(DcdDbContext context) : ControllerBase
{
    [Authorize]
    [HttpPost("log-exception")]
    public async Task<NoContentResult> LogException([FromBody] ExceptionDto exception)
    {
        context.FrontendExceptions.Add(new FrontendException
        {
            DetailsJson = JsonSerializer.Serialize(exception.Details)
        });

        await context.SaveChangesAsync();

        return NoContent();
    }
}

public class ExceptionDto
{
    public required Dictionary<string, string> Details { get; set; }
}
