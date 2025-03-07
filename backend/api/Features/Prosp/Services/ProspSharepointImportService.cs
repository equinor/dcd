using System.Web;

using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Prosp.Models;

using Microsoft.Graph;

namespace api.Features.Prosp.Services;

public class ProspSharepointImportService(
    GraphServiceClient graphServiceClient,
    ProspExcelImportService prospExcelImportService,
    RecalculationService recalculationService,
    DcdDbContext context)
{
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
            Id = x.Id
        })
            .ToList();
    }

    public async Task ImportFilesFromSharePoint(Guid projectId, SharePointImportDto[] dtos)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        await prospExcelImportService.ClearImportedProspData(projectPk, dtos.Select(x => x.CaseId).ToList());

        var (success, siteId, driveId, _) = await GetSharepointInfo(dtos.First().SharePointSiteUrl);

        if (!success)
        {
            return;
        }

        foreach (var dto in dtos)
        {
            if (string.IsNullOrWhiteSpace(dto.SharePointFileId) || string.IsNullOrWhiteSpace(dto.SharePointFileName))
            {
                continue;
            }

            var driveItemStream = await graphServiceClient.Sites[siteId]
                .Drives[driveId]
                .Items[dto.SharePointFileId]
                .Content.Request()
                .GetAsync();

            await prospExcelImportService.ImportProsp(driveItemStream, projectPk, dto.CaseId, dto.SharePointFileId, dto.SharePointFileName);
        }

        await context.SaveChangesAsync();

        foreach (var dto in dtos)
        {
            await context.UpdateCaseUpdatedUtc(dto.CaseId);
            await recalculationService.SaveChangesAndRecalculateCase(dto.CaseId);
        }
    }

    private async Task<(bool Success, string SiteId, string DriveId, string ItemPath)> GetSharepointInfo(string url)
    {
        var validatedUri = new Uri(url);

        var site = await GetSite(validatedUri);

        if (site == null)
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

    private async Task<Site?> GetSite(Uri validatedUri)
    {
        try
        {
            return await graphServiceClient
                .Sites
                .GetByPath($"/sites/{validatedUri.AbsolutePath.Split('/')[2]}", validatedUri.Host)
                .Request()
                .GetAsync();
        }
        catch (Exception)
        {
            return null;
        }
    }
}
