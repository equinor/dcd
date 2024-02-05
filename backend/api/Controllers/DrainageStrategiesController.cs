using api.Adapters;
using api.Dtos;
using api.Services;

using Api.Authorization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.ReadOnly,
        ApplicationRole.User

    )]
public class DrainageStrategiesController : ControllerBase
{
    private readonly IDrainageStrategyService _drainageStrategyService;

    public DrainageStrategiesController(IDrainageStrategyService drainageStrategyService)
    {
        _drainageStrategyService = drainageStrategyService;
    }

    [HttpPost(Name = "CreateDrainageStrategy")]
    public async Task<ProjectDto> CreateDrainageStrategy([FromQuery] Guid sourceCaseId, [FromBody] DrainageStrategyDto drainageStrategyDto)
    {
        return await _drainageStrategyService.CreateDrainageStrategy(drainageStrategyDto, sourceCaseId);
    }

    [HttpDelete("{drainageStrategyId}", Name = "DeleteDrainageStrategy")]
    public async Task<ProjectDto> DeleteDrainageStrategy(Guid drainageStrategyId)
    {
        return await _drainageStrategyService.DeleteDrainageStrategy(drainageStrategyId);
    }

    [HttpPut(Name = "UpdateDrainageStrategy")]
    public async Task<ProjectDto> UpdateDrainageStrategy([FromBody] DrainageStrategyDto drainageStrategyDto)
    {
        return await _drainageStrategyService.UpdateDrainageStrategy(drainageStrategyDto);
    }

    [HttpPost("{drainageStrategyId}/copy", Name = "CopyDrainageStrategy")]
    public async Task<DrainageStrategyDto> CopyDrainageStrategy([FromQuery] Guid caseId, Guid drainageStrategyId)
    {
        return await _drainageStrategyService.CopyDrainageStrategy(drainageStrategyId, caseId);
    }

    [HttpPut("new", Name = "NewUpdateDrainageStrategy")]
    public async Task<DrainageStrategyDto> NewUpdateDrainageStrategy([FromBody] DrainageStrategyDto drainageStrategyDto)
    {
        return await _drainageStrategyService.NewUpdateDrainageStrategy(drainageStrategyDto);
    }
}
