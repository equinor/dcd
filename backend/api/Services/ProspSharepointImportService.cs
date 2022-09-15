using api.Dtos;
using api.Models;

using Microsoft.Graph;

namespace api.Services;

public class ProspSharepointImportService
{
    private readonly IConfiguration _config;
    private readonly GraphServiceClient _graphServiceClient;
    private readonly ILogger<ProspSharepointImportService> _logger;
    private readonly ProspExcelImportService _service;

    public ProspSharepointImportService(IConfiguration config, GraphServiceClient graphServiceClient,
        ProspExcelImportService service, ILoggerFactory loggerFactory)
    {
        _graphServiceClient = graphServiceClient;
        _config = config;
        _service = service;
        _logger = loggerFactory.CreateLogger<ProspSharepointImportService>();
    }

    public async Task<IDriveItemDeltaCollectionPage?>? GetDeltaDriveItemCollectionFromSite(string? url)
    {
        var siteId = GetSiteId(url)?.Result;

        if (string.IsNullOrWhiteSpace(siteId))
        {
            return null;
        }

        try
        {
            var driveItemSearchCollectionPage = await _graphServiceClient.Sites[siteId]
                .Drive.Root.Delta()
                .Request()
                .GetAsync();

            return driveItemSearchCollectionPage;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"failed retrieving list of latest DriveItems in Site: {e.Message}");
        }

        return null;
    }

    public static List<DriveItemDto> GetExcelDriveItemsFromSite(
        IDriveItemDeltaCollectionPage? driveItemDeltaCollectionPage)
    {
        var dto = new List<DriveItemDto>();
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

    private async Task<string?>? GetSiteId(string? url)
    {
        try
        {
            if (url != null)
            {
                var siteUri = new Uri(url);
                var hostName = siteUri.Host;

                // Example of valid relativepath: /sites/{your site name} such as /sites/ConceptApp-Test
                var relativePath = $"/sites/{siteUri.AbsolutePath.Split('/', 3)[2].Split('/')[0]}";

                var site = await _graphServiceClient.Sites.GetByPath(relativePath, hostName)
                    .Request()
                    .GetAsync();

                return site.Id;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"failed retrieving Site id : {e.Message}");
        }

        return null;
    }

    public async Task<ProjectDto> ConvertSharepointFilesToProjectDto(Guid projectId, SharePointImportDto[] dtos)
    {
        var projectDto = new ProjectDto();
        if (!string.IsNullOrWhiteSpace(dtos.FirstOrDefault()?.SharePointSiteUrl))
        {
            var siteId = GetSiteId(dtos.FirstOrDefault()!.SharePointSiteUrl)?.Result;
            if (siteId != null)
            {
                var fileIdsOnCases = new Dictionary<Guid, string>();
                foreach (var dto in dtos)
                {
                    fileIdsOnCases.Add(new Guid(dto.Id!), dto.SharePointFileId);
                }

                var fileStreamsOnCases = new Dictionary<Guid, Stream>();
                foreach (var item in fileIdsOnCases.Where(d => !string.IsNullOrWhiteSpace(d.Value)))
                {
                    try
                    {
                        var driveItemStream = await _graphServiceClient.Sites[siteId]
                            .Drive.Items[item.Value]
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
                    if (caseWithFileStream.Value.Length > 0)
                    {
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
            }
        }

        return projectDto;
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
