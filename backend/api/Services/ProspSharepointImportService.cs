using System.Web;

using api.Dtos;
using api.Models;

using Microsoft.Graph;

namespace api.Services;

public class ProspSharepointImportService
{
    private static ILogger<ProspSharepointImportService>? _logger;
    private readonly IConfiguration _config;
    private readonly GraphServiceClient _graphServiceClient;
    private readonly ProspExcelImportService _service;

    public ProspSharepointImportService(IConfiguration config, GraphServiceClient graphServiceClient,
        ProspExcelImportService service, ILoggerFactory loggerFactory)
    {
        _graphServiceClient = graphServiceClient;
        _config = config;
        _service = service;
        _logger = loggerFactory.CreateLogger<ProspSharepointImportService>();
    }

    public async Task<List<DriveItem>> GetDeltaDriveItemCollectionFromSite(string? url)
    {
        var driveItems = new List<DriveItem>();
        var siteIdAndParentRef = GetSiteIdAndParentReferencePath(url).Result;
        var siteId = siteIdAndParentRef[0];
        var parentRefPath = siteIdAndParentRef.Count > 1 ? siteIdAndParentRef[1] : "";

        if (string.IsNullOrWhiteSpace(siteId))
        {
            return driveItems;
        }

        try
        {
            var documentLibraryName = parentRefPath?.Split('/')[3];
            var itemPath = string.Join('/', parentRefPath?.Split('/').Skip(4) ?? Array.Empty<string>());
            var driveId = await GetDocumentLibraryDriveId(siteId, documentLibraryName);

            return await GetDeltaDriveItemCollectionFromSite(itemPath, siteId, driveId, driveItems);
        }
        catch (Exception? e)
        {
            _logger?.LogError(e, $"failed retrieving list of latest DriveItems in Site: {e.Message}");
        }

        return driveItems;
    }

    private async Task<List<DriveItem>> GetDeltaDriveItemCollectionFromSite(string itemPath, string siteId,
        string? driveId, List<DriveItem> driveItems)
    {
        IDriveItemDeltaCollectionPage? driveItemsDelta;
        if (!string.IsNullOrWhiteSpace(itemPath))
        {
            driveItemsDelta = await _graphServiceClient.Sites[siteId].Drives[driveId].Root
                .ItemWithPath("/" + itemPath).Delta().Request().GetAsync();
        }
        else
        {
            driveItemsDelta = await _graphServiceClient.Sites[siteId].Drives[driveId].Root
                .Delta().Request().GetAsync();
        }


        if (driveItemsDelta == null)
        {
            return driveItems;
        }

        driveItems.AddRange(driveItemsDelta);

        return driveItems;
    }

    private async Task<string?> GetDocumentLibraryDriveId(string siteId, string? documentLibraryName)
    {
        var getDrivesInSite = await _graphServiceClient.Sites[siteId].Drives
            .Request()
            .GetAsync();

        // Sharepoint document library 'Documents' will have the name "Shared Documents" in Url
        var decodedDocumentLibraryName = HttpUtility.UrlDecode(documentLibraryName) == "Shared Documents"
            ? "Documents"
            : HttpUtility.UrlDecode(documentLibraryName);

        var driveIds = getDrivesInSite.Where(x => x.Name == decodedDocumentLibraryName).Select(i => i.Id).ToList();

        var driveId = driveIds?.FirstOrDefault();
        return driveId;
    }


    public static List<DriveItemDto> GetExcelDriveItemsFromSite(
        List<DriveItem>? driveItemDeltaCollectionPage)
    {
        var dto = new List<DriveItemDto>();
        try
        {
            if (driveItemDeltaCollectionPage != null)
            {
                foreach (var driveItem in driveItemDeltaCollectionPage.Where(item =>
                             item.File != null && ValidMimeTypes().Contains(item.File.MimeType)))
                {
                    ConvertToDto(driveItem, dto);
                }
            }

            return dto;
        }
        catch (Exception? e)
        {
            _logger?.LogError(e, $"failed converting filtered driveItem list to driveItemDto in Site: {e.Message}");
        }

        return dto;
    }

    private async Task<List<string>> GetSiteIdAndParentReferencePath(string? url)
    {
        var siteData = new List<string>();
        try
        {
            if (url != null)
            {
                var siteUrl = new Uri(url);
                var hostName = siteUrl.Host;
                var pathFromIdParameter = HttpUtility.ParseQueryString(siteUrl.Query).Get("id");
                var siteNameFromUrl = siteUrl.AbsolutePath.Split('/')[2];

                // Example of valid relativepath: /sites/{your site name} such as /sites/ConceptApp-Test
                var relativePath = $@"/sites/{siteNameFromUrl}";


                var site = await _graphServiceClient.Sites.GetByPath(relativePath, hostName)
                    .Request()
                    .GetAsync();

                siteData.Add(site.Id);

                var documentLibraryNameFromUrl = siteUrl.AbsolutePath.Split('/')[3];
                // DriveItem path to get content from subfolder, if no subfolder given then set folder from absolute path
                var parentReferencePath = pathFromIdParameter != null
                    ? $@"/drive/root:/{GetDriveItemPathFromUrl(pathFromIdParameter)}"
                    : $@"/drive/root:/{documentLibraryNameFromUrl}";

                siteData.Add(parentReferencePath);


                return siteData;
            }
        }
        catch (Exception e)
        {
            _logger?.LogError(e, $"Invalid url: {e.Message}");
        }

        return siteData;
    }

    private static string? GetDriveItemPathFromUrl(string? pathFromIdParameter)
    {
        return pathFromIdParameter != null
            ? string.Join('/', pathFromIdParameter.Split('/').Skip(3))
            : null;
    }

    public async Task<ProjectDto> ConvertSharepointFilesToProjectDto(Guid projectId, SharePointImportDto[] dtos)
    {
        var projectDto = new ProjectDto();
        if (!string.IsNullOrWhiteSpace(dtos.FirstOrDefault()?.SharePointSiteUrl))
        {
            var siteId = GetSiteIdAndParentReferencePath(dtos.FirstOrDefault()!.SharePointSiteUrl)?.Result[0];
            if (siteId == null)
            {
                return projectDto;
            }
            var driveId = await GetDriveIdFromSharePointSiteUrl(dtos, siteId);

            var fileIdsOnCases = dtos.ToDictionary(dto => new Guid(dto.Id!), dto => dto.SharePointFileId);

            var fileStreamsOnCases = new Dictionary<Guid, Stream>();
            foreach (var item in fileIdsOnCases.Where(d => !string.IsNullOrWhiteSpace(d.Value)))
            {
                try
                {
                    var driveItemStream = await _graphServiceClient.Sites[siteId]
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
                foreach (var iteminfo in dtos.Where(importDto =>
                             importDto.Id != null && new Guid(importDto.Id) == caseWithFileStream.Key))
                {
                    var assets = MapAssets(iteminfo.Surf, iteminfo.Substructure, iteminfo.Topside,
                        iteminfo.Transport);

                    projectDto = _service.ImportProsp(caseWithFileStream.Value, caseWithFileStream.Key,
                        projectId,
                        assets,
                        iteminfo.SharePointFileId);
                }
            }
        }

        return projectDto;
    }

    private async Task<string?> GetDriveIdFromSharePointSiteUrl(SharePointImportDto[] dtos, string siteId)
    {
        var siteIdAndParentRef =
            GetSiteIdAndParentReferencePath(dtos.FirstOrDefault()?.SharePointSiteUrl).Result;
        var parentRefPath = siteIdAndParentRef.Count > 1 ? siteIdAndParentRef[1] : "";
        var documentLibraryName = parentRefPath.Split('/')[3];
        var driveId = await GetDocumentLibraryDriveId(siteId, documentLibraryName);
        return driveId;
    }

    private static Dictionary<string, bool> MapAssets(bool surf, bool substructure, bool topside, bool transport)
    {
        return new Dictionary<string, bool>
        {
            { nameof(Surf), surf },
            { nameof(Topside), topside },
            { nameof(Substructure), substructure },
            { nameof(Transport), transport }
        };
    }

    private static void ConvertToDto(DriveItem driveItem, List<DriveItemDto> dto)
    {
        var item = new DriveItemDto
        {
            Name = driveItem.Name,
            Id = driveItem.Id,
            CreatedBy = driveItem.CreatedBy,
            Content = driveItem.Content,
            CreatedDateTime = driveItem.CreatedDateTime,
            Size = driveItem.Size,
            SharepointIds = driveItem.SharepointIds,
            LastModifiedBy = driveItem.LastModifiedBy,
            LastModifiedDateTime = driveItem.LastModifiedDateTime
        };
        dto.Add(item);
    }

    private static List<string> ValidMimeTypes()
    {
        var validMimeTypes = new List<string>
        {
            ExcelMimeTypes.Xls,
            ExcelMimeTypes.Xlsb,
            ExcelMimeTypes.Xlsm,
            ExcelMimeTypes.Xlsx
        };
        return validMimeTypes;
    }
}
