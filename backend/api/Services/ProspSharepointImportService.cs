using api.Dtos;
using api.Models;

using Microsoft.Graph;

namespace api.Services;

public class ProspSharepointImportService
{
    private readonly IConfiguration _config;
    private readonly GraphServiceClient _graphServiceClient;
    private readonly ProspExcelImportService _service;

    public ProspSharepointImportService(IConfiguration config, GraphServiceClient graphServiceClient,
        ProspExcelImportService service)
    {
        _graphServiceClient = graphServiceClient;
        _config = config;
        _service = service;
    }

    public IDriveItemDeltaCollectionPage GetDeltaDriveItemCollectionFromSite(string url)
    {
        var siteId = GetSiteId(url);

        var driveItemSearchCollectionPage = _graphServiceClient.Sites[siteId]
            .Drive.Root.Delta()
            .Request()
            .GetAsync()
            .GetAwaiter()
            .GetResult();

        return driveItemSearchCollectionPage;
    }

    public List<DriveItemDto> GetFilesFromSite(IDriveItemDeltaCollectionPage driveItemDeltaCollectionPage)
    {
        var dto = new List<DriveItemDto>();
        foreach (var driveItem in driveItemDeltaCollectionPage.Where(item =>
                     item.File != null && ValidMimeTypes().Contains(item.File.MimeType)))
        {
            ConvertToDto(driveItem, dto);
        }

        return dto;
    }

    public string GetSiteId(string url)
    {
        var siteUri = new Uri(url);
        var hostName = siteUri.Host;
        var relativePath = $"/sites/{siteUri.AbsolutePath.Split('/', 3)[2].Split('/')[0]}";

        var site = _graphServiceClient.Sites.GetByPath(relativePath, hostName)
            .Request()
            .GetAsync()
            .GetAwaiter().GetResult();

        return site.Id;
    }

    public async Task<ProjectDto> ConvertSharepointFilesToProjectDto(Guid projectId, SharePointImportDto[] dtos)
    {
        var projectDto = new ProjectDto();
        if (!string.IsNullOrWhiteSpace(dtos.FirstOrDefault()?.SharePointSiteUrl))
        {
            var siteId = GetSiteId(dtos.FirstOrDefault()!.SharePointSiteUrl);
            var fileIdsOnCases = new Dictionary<Guid, string>();
            foreach (var dto in dtos)
            {
                fileIdsOnCases.Add(new Guid(dto.Id!), dto.SharePointFileId);
            }

            var fileStreamsOnCases = new Dictionary<Guid, Stream>();
            foreach (var item in fileIdsOnCases.Where(d => !string.IsNullOrWhiteSpace(d.Value)))
            {
                var driveItemStream = await _graphServiceClient.Sites[siteId]
                    .Drive.Items[item.Value]
                    .Content.Request()
                    .GetAsync();

                fileStreamsOnCases.Add(item.Key, driveItemStream);
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
                        projectDto = _service.ImportProsp(caseWithFileStream.Value, caseWithFileStream.Key, projectId,
                            assets,
                            iteminfo.SharePointFileId);
                    }
                }
            }
        }

        return projectDto;
    }

    public Dictionary<string, bool> MapAssets(bool surf, bool substructure, bool topside, bool transport)
    {
        return new Dictionary<string, bool>
        {
            { nameof(Surf), surf },
            { nameof(Topside), topside },
            { nameof(Substructure), substructure },
            { nameof(Transport), transport }
        };
    }

    public void ConvertToDto(DriveItem driveItem, List<DriveItemDto> dto)
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

    public List<string> ValidMimeTypes()
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
