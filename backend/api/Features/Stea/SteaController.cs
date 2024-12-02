using api.AppInfrastructure.Authorization;
using api.Controllers;
using api.Features.Stea.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Stea;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class SteaController(SteaService steaService) : ControllerBase
{
    [HttpPost("stea/{projectId:guid}")]
    [ActionType(ActionType.Read)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.ReadOnly, ApplicationRole.User)]
    public async Task<FileResult> ExportSteaToExcel(Guid projectId)
    {
        var (bytes, filename) = await steaService.GetExcelFile(projectId);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
    }

    [HttpGet("stea/{projectId:guid}")]
    [ActionType(ActionType.Read)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.ReadOnly, ApplicationRole.User)]
    public async Task<SteaProjectDto> GetInputToStea(Guid projectId)
    {
        return await steaService.GetInputToStea(projectId);
    }
}
