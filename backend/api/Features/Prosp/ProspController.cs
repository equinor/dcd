using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;
using api.Features.Prosp.Exceptions;
using api.Features.Prosp.Models;
using api.Features.Prosp.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;

namespace api.Features.Prosp;

public class ProspController(ProspSharepointImportService prospSharepointImportImportService,
    GetProjectDataService getProjectDataService,
    ILogger<ProspController> logger)
    : ControllerBase
{
    [HttpPost("prosp/sharepoint", Name = nameof(GetSharePointFileNamesAndId))]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<ActionResult<List<DriveItemDto>>> GetSharePointFileNamesAndId([FromBody] UrlDto urlDto)
    {
        if (string.IsNullOrWhiteSpace(urlDto.Url))
        {
            return BadRequest("URL is required.");
        }

        try
        {
            var driveItems = await prospSharepointImportImportService.GetDeltaDriveItemCollectionFromSite(urlDto.Url);
            return Ok(driveItems);
        }
        catch (ServiceException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            logger.LogError(ex, "Access Denied when attempting to access SharePoint site: {Url}", urlDto.Url);
            return StatusCode(StatusCodes.Status403Forbidden, "Access to SharePoint resource was denied.");
        }
        catch (AccessDeniedException ex)
        {
            logger.LogError(ex, "Custom Access Denied when attempting to access SharePoint site: {Url}", urlDto.Url);
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
    }

    [HttpPost("prosp/{projectId:guid}/sharepoint")]
    [AuthorizeActionType(ActionType.Edit)]
    [DisableRequestSizeLimit]
    public async Task<ActionResult<ProjectDataDto>> ImportFilesFromSharepointAsync(Guid projectId, [FromBody] SharePointImportDto[] dtos)
    {
        try
        {
            await prospSharepointImportImportService.ConvertSharepointFilesToProjectDto(projectId, dtos);

            return Ok(await getProjectDataService.GetProjectData(projectId));
        }
        catch (ServiceException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            logger.LogError($"Access denied when trying to import files from SharePoint for project {projectId}: {ex.Message}");
            return StatusCode(StatusCodes.Status403Forbidden, "Access to SharePoint resource was denied.");
        }
    }
}
