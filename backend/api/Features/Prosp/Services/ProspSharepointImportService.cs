using System.Web;

using api.Features.Prosp.Exceptions;
using api.Features.Prosp.Models;
using api.Models;

using Microsoft.Graph;

namespace api.Features.Prosp.Services;

public class ProspSharepointImportService(
    GraphServiceClient graphServiceClient,
    ProspExcelImportService prospExcelImportService,
    ILogger<ProspSharepointImportService> logger)
{
    public async Task<List<DriveItemDto>> GetDeltaDriveItemCollectionFromSite(string? url)
    {
        var siteIdAndParentRef = await GetSiteIdAndParentReferencePath(url);
        var siteId = siteIdAndParentRef[0];
        var parentRefPath = siteIdAndParentRef.Count > 1 ? siteIdAndParentRef[1] : "";

        if (string.IsNullOrWhiteSpace(siteId))
        {
            return [];
        }

        try
        {
            var documentLibraryName = parentRefPath?.Split('/')[3];
            var itemPath = string.Join('/', parentRefPath?.Split('/').Skip(4) ?? Array.Empty<string>());
            var driveId = await GetDocumentLibraryDriveId(siteId, documentLibraryName);

            return await GetDeltaDriveItemCollectionFromSite(itemPath, siteId, driveId);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"failed retrieving list of latest DriveItems in Site: {e.Message}");
        }

        return [];
    }

    private async Task<List<DriveItemDto>> GetDeltaDriveItemCollectionFromSite(string itemPath, string siteId, string? driveId)
    {
        IDriveItemDeltaCollectionPage? driveItemsDelta;
        if (!string.IsNullOrWhiteSpace(itemPath))
        {
            driveItemsDelta = await graphServiceClient.Sites[siteId].Drives[driveId].Root
                .ItemWithPath("/" + itemPath).Delta().Request().GetAsync();
        }
        else
        {
            driveItemsDelta = await graphServiceClient.Sites[siteId].Drives[driveId].Root
                .Delta().Request().GetAsync();
        }

        if (driveItemsDelta == null)
        {
            return [];
        }

        return driveItemsDelta.Select(x => new DriveItemDto
        {
            Name = x.Name,
            Id = x.Id,
            SharepointFileUrl = null,
            CreatedDateTime = x.CreatedDateTime,
            Content = x.Content,
            Size = x.Size,
            SharepointIds = x.SharepointIds,
            CreatedBy = x.CreatedBy,
            LastModifiedBy = x.LastModifiedBy,
            LastModifiedDateTime = x.LastModifiedDateTime
        })
            .ToList();
    }

    private async Task<string?> GetDocumentLibraryDriveId(string siteId, string? documentLibraryName)
    {
        var getDrivesInSite = await graphServiceClient.Sites[siteId].Drives
            .Request()
            .GetAsync();

        // Sharepoint document library 'Documents' will have the name "Shared Documents" in Url
        var decodedDocumentLibraryName = HttpUtility.UrlDecode(documentLibraryName) == "Shared Documents"
            ? "Documents"
            : HttpUtility.UrlDecode(documentLibraryName);

        var driveIds = getDrivesInSite.Where(x => x.Name == decodedDocumentLibraryName).Select(i => i.Id).ToList();

        return driveIds.FirstOrDefault();
    }

    private async Task<List<string>> GetSiteIdAndParentReferencePath(string? url)
    {
        var siteData = new List<string>();
        try
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("URL cannot be null or empty.", nameof(url));
            }
            // Basic validation of URL format
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? validatedUri))
            {
                throw new UriFormatException($"Invalid URL format: {url}");
            }

            var hostName = validatedUri.Host;
            var pathFromIdParameter = HttpUtility.ParseQueryString(validatedUri.Query).Get("id");
            var siteNameFromUrl = validatedUri.AbsolutePath.Split('/')[2];

            // Example of valid relativepath: /sites/{your site name} such as /sites/ConceptApp-Test
            var relativePath = $@"/sites/{siteNameFromUrl}";

            var site = await graphServiceClient.Sites.GetByPath(relativePath, hostName)
                .Request()
                .GetAsync();

            siteData.Add(site.Id);

            var documentLibraryNameFromUrl = validatedUri.AbsolutePath.Split('/')[3];
            // DriveItem path to get content from subfolder, if no subfolder given then set folder from absolute path
            var parentReferencePath = pathFromIdParameter != null
                ? $@"/drive/root:/{GetDriveItemPathFromUrl(pathFromIdParameter)}"
                : $@"/drive/root:/{documentLibraryNameFromUrl}";

            siteData.Add(parentReferencePath);
        }
        catch (UriFormatException ex)
        {
            logger.LogError(ex, "Invalid URI format: {Url}", url);
            throw; // Consider how to handle this error. Maybe wrap it in a custom exception for higher-level handling.
        }
        catch (ServiceException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            logger.LogError(ex, "Access Denied when attempting to access SharePoint site: {Url}", url);
            throw new AccessDeniedException("Access to SharePoint resource was denied.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while attempting to access SharePoint site: {Url}", url);
            throw; // Re-throw the exception to be handled upstream.
        }

        return siteData;
    }

    private static string? GetDriveItemPathFromUrl(string? pathFromIdParameter)
    {
        return pathFromIdParameter != null
            ? string.Join('/', pathFromIdParameter.Split('/').Skip(3))
            : null;
    }

    public async Task ConvertSharepointFilesToProjectDto(Guid projectId, SharePointImportDto[] dtos)
    {
        if (string.IsNullOrWhiteSpace(dtos.FirstOrDefault()?.SharePointSiteUrl))
        {
            return;
        }

        foreach (var importDto in dtos)
        {
            if (!string.IsNullOrWhiteSpace(importDto.SharePointFileId) || importDto.Id == null)
            {
                continue;
            }

            var caseId = new Guid(importDto.Id);
            await prospExcelImportService.ClearImportedProspData(caseId, projectId);
        }

        var siteId = GetSiteIdAndParentReferencePath(dtos.FirstOrDefault()!.SharePointSiteUrl)?.Result[0];
        if (siteId == null)
        {
            return;
        }

        var driveId = await GetDriveIdFromSharePointSiteUrl(dtos, siteId);

        var fileIdsOnCases = dtos.ToDictionary(dto => new Guid(dto.Id!), dto => dto.SharePointFileId);

        var fileStreamsOnCases = new Dictionary<Guid, Stream>();
        foreach (var item in fileIdsOnCases.Where(d => !string.IsNullOrWhiteSpace(d.Value)))
        {
            try
            {
                var driveItemStream = await graphServiceClient.Sites[siteId]
                    .Drives[driveId].Items[item.Value]
                    .Content.Request()
                    .GetAsync();

                fileStreamsOnCases.Add(item.Key, driveItemStream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        foreach (var caseWithFileStream in fileStreamsOnCases)
        {
            if (caseWithFileStream.Value.Length <= 0)
            {
                continue;
            }

            foreach (var itemInfo in dtos.Where(importDto =>
                         importDto.Id != null && new Guid(importDto.Id) == caseWithFileStream.Key))
            {
                var assets = MapAssets(itemInfo.Surf, itemInfo.Substructure, itemInfo.Topside,
                    itemInfo.Transport, itemInfo.OnshorePowerSupply);
                await prospExcelImportService.ImportProsp(caseWithFileStream.Value, caseWithFileStream.Key,
                    projectId,
                    assets,
                    itemInfo.SharePointFileId,
                    itemInfo.SharePointFileName,
                    itemInfo.SharePointFileUrl);
            }
        }
    }

    private async Task<string?> GetDriveIdFromSharePointSiteUrl(SharePointImportDto[] dtos, string siteId)
    {
        var siteIdAndParentRef = GetSiteIdAndParentReferencePath(dtos.FirstOrDefault()?.SharePointSiteUrl).Result;
        var parentRefPath = siteIdAndParentRef.Count > 1 ? siteIdAndParentRef[1] : "";
        var documentLibraryName = parentRefPath.Split('/')[3];
        return await GetDocumentLibraryDriveId(siteId, documentLibraryName);
    }

    private static Dictionary<string, bool> MapAssets(bool surf, bool substructure, bool topside, bool transport, bool onshorePowerSupply)
    {
        return new Dictionary<string, bool>
        {
            { nameof(Surf), surf },
            { nameof(Topside), topside },
            { nameof(Substructure), substructure },
            { nameof(Transport), transport },
            { nameof(OnshorePowerSupply), onshorePowerSupply }
        };
    }
}
