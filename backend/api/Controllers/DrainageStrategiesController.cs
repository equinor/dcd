using api.Adapters;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class DrainageStrategiesController : ControllerBase
{
    private readonly DrainageStrategyService _drainageStrategyService;

    public DrainageStrategiesController(DrainageStrategyService drainageStrategyService)
    {
        _drainageStrategyService = drainageStrategyService;
    }

    [HttpPost(Name = "CreateDrainageStrategy")]
    public ProjectDto CreateDrainageStrategy([FromQuery] Guid sourceCaseId, [FromBody] DrainageStrategyDto drainageStrategyDto)
    {
        return _drainageStrategyService.CreateDrainageStrategy(drainageStrategyDto, sourceCaseId);
    }

    [HttpDelete("{drainageStrategyId}", Name = "DeleteDrainageStrategy")]
    public ProjectDto DeleteDrainageStrategy(Guid drainageStrategyId)
    {
        return _drainageStrategyService.DeleteDrainageStrategy(drainageStrategyId);
    }

    [HttpPut(Name = "UpdateDrainageStrategy")]
    public ProjectDto UpdateDrainageStrategy([FromBody] DrainageStrategyDto drainageStrategyDto)
    {
        return _drainageStrategyService.UpdateDrainageStrategy(drainageStrategyDto);
    }
}