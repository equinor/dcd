using api.AppInfrastructure.Authorization;
using api.Controllers;
using api.Dtos;
using api.Features.Prosp.Exceptions;
using api.Features.Prosp.Models;
using api.Features.Prosp.Services;
using api.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;

namespace api.Features.Prosp;

[Route("[controller]")]
[ApiController]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.User
)]
[ActionType(ActionType.Edit)]
public class ProspController(
    ProspSharepointImportService prospSharepointImportImportService,
    IProjectService projectService,
    ILogger<ProspController> logger)
    : ControllerBase
{
    [HttpPost("sharepoint", Name = nameof(GetSharePointFileNamesAndId))]
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
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing your request for URL: {Url}", urlDto.Url);
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal server error occurred.");
        }
    }

    [HttpPost("{projectId}/sharepoint", Name = nameof(ImportFilesFromSharepointAsync))]
    [DisableRequestSizeLimit]
    public async Task<ActionResult<ProjectWithAssetsDto>> ImportFilesFromSharepointAsync([FromQuery] Guid projectId,
        [FromBody] SharePointImportDto[] dtos)
    {
        try
        {
            await prospSharepointImportImportService.ConvertSharepointFilesToProjectDto(projectId, dtos);

            return Ok(await projectService.GetProjectDto(projectId));
        }
        catch (ServiceException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            logger.LogError($"Access denied when trying to import files from SharePoint for project {projectId}: {ex.Message}");
            return StatusCode(StatusCodes.Status403Forbidden, "Access to SharePoint resource was denied.");
        }
        // Handle other potential ServiceException cases, if necessary
        catch (Exception e)
        {
            logger.LogError($"An error occurred while importing files from SharePoint for project {projectId}: {e.Message}");
            // Consider returning a more generic error message to avoid exposing sensitive details
            // and ensure it's a client-friendly message
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }
}
