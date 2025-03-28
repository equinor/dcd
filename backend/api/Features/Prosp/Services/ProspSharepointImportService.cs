using System.Web;

using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Prosp.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;

namespace api.Features.Prosp.Services;

public class ProspSharepointImportService(
    GraphServiceClient graphServiceClient,
    ProspExcelImportService prospExcelImportService,
    RecalculationService recalculationService,
    DcdDbContext context)
{
    public async Task<bool> CheckForUpdate(Guid projectId, Guid caseId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var caseItem = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        if (caseItem.SharepointFileUrl == null || caseItem.SharepointFileId == null)
        {
            return false;
        }

        var sharepointChangeDateUtc = await GetLastModifiedDate(caseItem.SharepointFileUrl, caseItem.SharepointFileId);

        if (sharepointChangeDateUtc == null)
        {
            return false;
        }

        if (caseItem.SharepointUpdatedTimestampUtc == null)
        {
            return true;
        }

        return caseItem.SharepointUpdatedTimestampUtc < sharepointChangeDateUtc;
    }

    public async Task<List<SharePointFileDto>> GetFilesFromSharePoint(string url)
    {
        var (success, siteId, driveId, itemPath) = await GetSharepointInfo(url);

        if (!success)
        {
            return [];
        }

        var driveItemsDelta = string.IsNullOrWhiteSpace(itemPath)
            ? await graphServiceClient.Sites[siteId].Drives[driveId].Root.Delta().Request().GetAsync()
            : await graphServiceClient.Sites[siteId].Drives[driveId].Root.ItemWithPath("/" + itemPath).Delta().Request().GetAsync();

        return driveItemsDelta.Select(x => new SharePointFileDto
            {
                Name = x.Name,
                Id = x.Id,
                LastModifiedUtc = DateTime.SpecifyKind(x.LastModifiedDateTime!.Value.UtcDateTime, DateTimeKind.Unspecified)
            })
            .ToList();
    }

    public async Task ImportFileFromSharePoint(Guid projectId, SharePointImportDto dto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        if (string.IsNullOrEmpty(dto.SharePointFileId))
        {
            await prospExcelImportService.ClearImportedProspData(projectPk, dto.CaseId);

            return;
        }

        var (success, siteId, driveId, _) = await GetSharepointInfo(dto.SharePointSiteUrl);

        if (!success)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(dto.SharePointFileId) || string.IsNullOrWhiteSpace(dto.SharePointFileName))
        {
            return;
        }

        var driveItemStream = await graphServiceClient.Sites[siteId]
            .Drives[driveId]
            .Items[dto.SharePointFileId]
            .Content
            .Request()
            .GetAsync();

        var lastModified = await GetLastModifiedDate(dto.SharePointSiteUrl, dto.SharePointFileId);

        await prospExcelImportService.ImportProsp(driveItemStream, projectPk, dto.CaseId, dto.SharePointFileId, dto.SharePointFileName);

        var caseItem = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == dto.CaseId)
            .SingleAsync();

        caseItem.SharepointUpdatedTimestampUtc = lastModified;

        await context.UpdateCaseUpdatedUtc(dto.CaseId);
        await recalculationService.SaveChangesAndRecalculateCase(dto.CaseId);
    }

    private async Task<DateTime?> GetLastModifiedDate(string url, string sharepointFileId)
    {
        var (success, siteId, driveId, itemPath) = await GetSharepointInfo(url);

        if (!success)
        {
            return null;
        }

        var driveItemsDelta = string.IsNullOrWhiteSpace(itemPath)
            ? await graphServiceClient.Sites[siteId].Drives[driveId].Root.Delta().Request().GetAsync()
            : await graphServiceClient.Sites[siteId].Drives[driveId].Root.ItemWithPath("/" + itemPath).Delta().Request().GetAsync();

        return driveItemsDelta
            .Where(x => x.Id == sharepointFileId)
            .Select(x => x.LastModifiedDateTime!.Value.UtcDateTime)
            .Single();
    }

    private async Task<(bool Success, string SiteId, string DriveId, string ItemPath)> GetSharepointInfo(string url)
    {
        var validatedUri = new Uri(url);

        Site? site;

        try
        {
            site = await graphServiceClient
                .Sites
                .GetByPath($"/sites/{validatedUri.AbsolutePath.Split('/')[2]}", validatedUri.Host)
                .Request()
                .GetAsync();
        }
        catch (Exception)
        {
            return (false, "", "", "");
        }

        var pathFromIdParameter = HttpUtility.ParseQueryString(validatedUri.Query).Get("id");

        var path = pathFromIdParameter != null
            ? $"/drive/root:/{string.Join('/', pathFromIdParameter.Split('/').Skip(3))}"
            : $"/drive/root:/{validatedUri.AbsolutePath.Split('/')[3]}";

        var getDrivesInSite = await graphServiceClient.Sites[site.Id].Drives.Request().GetAsync();

        var decodedDocumentLibraryName = HttpUtility.UrlDecode(path.Split('/')[3]) == "Shared Documents"
            ? "Documents"
            : HttpUtility.UrlDecode(path.Split('/')[3]);

        var driveId = getDrivesInSite.Where(x => x.Name == decodedDocumentLibraryName).Select(i => i.Id).First();

        var itemPath = string.Join('/', path.Split('/').Skip(4));

        return (true, site.Id, driveId, itemPath);
    }
}
