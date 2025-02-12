using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;
using api.Features.Prosp.Exceptions;
using api.Features.Prosp.Models;
using api.Features.Prosp.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;

namespace api.Features.Prosp;

public class ProspController(ProspSharepointImportService prospSharepointImportImportService, GetProjectDataService getProjectDataService)
    : ControllerBase
{
    [HttpPost("prosp/projects/{projectId:guid}/sharepoint")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<ActionResult<List<DriveItemDto>>> GetFilesFromSharepoint(Guid projectId, [FromBody] UrlDto urlDto)
    {
        if (string.IsNullOrWhiteSpace(urlDto.Url))
        {
            return Ok(new List<DriveItemDto>());
        }

        if (!IsValidUrl(urlDto.Url))
        {
            return BadRequest("URL is malformed.");
        }

        try
        {
            return Ok(await prospSharepointImportImportService.GetFilesFromSharepoint(urlDto.Url));
        }
        catch (ServiceException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            return StatusCode(StatusCodes.Status403Forbidden, "Access to SharePoint resource was denied.");
        }
        catch (AccessDeniedException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
    }

    [HttpPost("prosp/{projectId:guid}/sharepoint")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<ActionResult<ProjectDataDto>> ImportFilesFromSharepoint(Guid projectId, [FromBody] SharePointImportDto[] dtos)
    {
        if (!dtos.Any())
        {
            return BadRequest("URL is required.");
        }

        if (!IsValidUrl(dtos.First().SharePointSiteUrl))
        {
            return BadRequest("URL is malformed.");
        }

        try
        {
            await prospSharepointImportImportService.ImportFilesFromSharepoint(projectId, dtos);

            return Ok(await getProjectDataService.GetProjectData(projectId));
        }
        catch (ServiceException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            return StatusCode(StatusCodes.Status403Forbidden, "Access to SharePoint resource was denied.");
        }
    }

    private static bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}
