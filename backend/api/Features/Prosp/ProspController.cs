using api.AppInfrastructure.ControllerAttributes;
using api.Exceptions;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;
using api.Features.Prosp.Models;
using api.Features.Prosp.Services;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Prosp;

public class ProspController(ProspSharepointImportService prospSharepointImportImportService, GetProjectDataService getProjectDataService)
    : ControllerBase
{
    [HttpPost("projects/{projectId:guid}/prosp/list")]
    [AuthorizeActionType(ActionType.Read)]
    public async Task<List<SharePointFileDto>> GetFilesFromSharePoint(Guid projectId, [FromBody] SharePointSiteUrlDto sharePointSiteUrlDto)
    {
        if (string.IsNullOrWhiteSpace(sharePointSiteUrlDto.Url))
        {
            return [];
        }

        EnsureValidUrl(sharePointSiteUrlDto.Url);

        return await prospSharepointImportImportService.GetFilesFromSharePoint(sharePointSiteUrlDto.Url);
    }

    [HttpPost("projects/{projectId:guid}/prosp/import")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<ProjectDataDto> ImportFilesFromSharePoint(Guid projectId, [FromBody] SharePointImportDto[] dtos)
    {
        EnsureNotEmpty(dtos);
        EnsureValidUrl(dtos.First().SharePointSiteUrl);

        await prospSharepointImportImportService.ImportFileFromSharePoint(projectId, dtos.First());

        return await getProjectDataService.GetProjectData(projectId);
    }

    [HttpGet("projects/{projectId:guid}/cases/{caseId:guid}/prosp/check-for-update")]
    [AuthorizeActionType(ActionType.Read)]
    public async Task<bool> CheckForUpdate(Guid projectId, Guid caseId)
    {
        return await prospSharepointImportImportService.CheckForUpdate(projectId, caseId);
    }

    private static void EnsureNotEmpty(SharePointImportDto[] dtos)
    {
        if (!dtos.Any())
        {
            throw new InvalidInputException("URL is required.");
        }
    }

    private static void EnsureValidUrl(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out _))
        {
            throw new InvalidInputException("Url is malformed.");
        }
    }
}
