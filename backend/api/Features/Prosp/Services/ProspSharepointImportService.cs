using System.Web;

using api.Context;
using api.Context.Extensions;
using api.Features.Cases.Recalculation;
using api.Features.Prosp.Exceptions;
using api.Features.Prosp.Models;

using Microsoft.Graph;

namespace api.Features.Prosp.Services;

public class ProspSharepointImportService(GraphServiceClient graphServiceClient, ProspExcelImportService prospExcelImportService, RecalculationService recalculationService, DcdDbContext context)
{
    public async Task<List<DriveItemDto>> GetFilesFromSharepoint(string url)
    {
        var siteIdAndParentRef = await GetSiteIdAndParentReferencePath(url);

        var siteId = siteIdAndParentRef[0];
        var parentRefPath = siteIdAndParentRef.Count > 1 ? siteIdAndParentRef[1] : "";

        if (string.IsNullOrWhiteSpace(siteId))
        {
            return [];
        }

        var itemPath = string.Join('/', parentRefPath.Split('/').Skip(4));
        var driveId = await GetDocumentLibraryDriveId(siteId, parentRefPath.Split('/')[3]);

        var driveItemsDelta = string.IsNullOrWhiteSpace(itemPath)
            ? await graphServiceClient.Sites[siteId].Drives[driveId].Root.Delta().Request().GetAsync()
            : await graphServiceClient.Sites[siteId].Drives[driveId].Root.ItemWithPath("/" + itemPath).Delta().Request().GetAsync();

        if (driveItemsDelta == null)
        {
            return [];
        }

        return driveItemsDelta.Select(x => new DriveItemDto
        {
            Name = x.Name,
            Id = x.Id
        })
            .ToList();
    }

    public async Task ImportFilesFromSharepoint(Guid projectId, SharePointImportDto[] dtos)
    {
        if (!dtos.Any())
        {
            return;
        }

        await prospExcelImportService.ClearImportedProspData(projectId, dtos.Select(x => x.CaseId).ToList());

        var siteIdAndParentRef = await GetSiteIdAndParentReferencePath(dtos.First().SharePointSiteUrl);

        var siteId = siteIdAndParentRef.First();

        var parentRefPath = siteIdAndParentRef.Count > 1 ? siteIdAndParentRef[1] : "";
        var driveId = await GetDocumentLibraryDriveId(siteId, parentRefPath.Split('/')[3]);

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

            if (driveItemStream.Length == 0)
            {
                continue;
            }

            await prospExcelImportService.ImportProsp(driveItemStream, projectId, dto.CaseId, dto.SharePointFileId, dto.SharePointFileName);

            await context.UpdateCaseUpdatedUtc(dto.CaseId);
        }

        await context.SaveChangesAsync();

        foreach (var dto in dtos)
        {
            await recalculationService.SaveChangesAndRecalculateCase(dto.CaseId);
        }
    }

    private async Task<string?> GetDocumentLibraryDriveId(string siteId, string? documentLibraryName)
    {
        var getDrivesInSite = await graphServiceClient.Sites[siteId].Drives.Request().GetAsync();

        // Sharepoint document library 'Documents' will have the name "Shared Documents" in Url
        var decodedDocumentLibraryName = HttpUtility.UrlDecode(documentLibraryName) == "Shared Documents"
            ? "Documents"
            : HttpUtility.UrlDecode(documentLibraryName);

        return getDrivesInSite.Where(x => x.Name == decodedDocumentLibraryName).Select(i => i.Id).FirstOrDefault();
    }

    private async Task<List<string>> GetSiteIdAndParentReferencePath(string url)
    {
        var siteData = new List<string>();

        try
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var validatedUri))
            {
                return [];
            }

            var site = await graphServiceClient
                .Sites
                .GetByPath($"/sites/{validatedUri.AbsolutePath.Split('/')[2]}", validatedUri.Host)
                .Request()
                .GetAsync();

            siteData.Add(site.Id);

            var pathFromIdParameter = HttpUtility.ParseQueryString(validatedUri.Query).Get("id");

            siteData.Add(pathFromIdParameter != null
                ? $"/drive/root:/{string.Join('/', pathFromIdParameter.Split('/').Skip(3))}"
                : $"/drive/root:/{validatedUri.AbsolutePath.Split('/')[3]}");
        }
        catch (ServiceException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            throw new AccessDeniedException("Access to SharePoint resource was denied.", ex);
        }

        return siteData;
    }
}
