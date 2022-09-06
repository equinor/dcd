using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.Graph;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class UploadController : ControllerBase
{
    // private const string testDriveItemId = "01LF7VUDUW3IAIVUAVBNAJALIVG7JK62EZ";
    private readonly GraphServiceClient _graphServiceClient;
    private readonly ImportProspService _prospService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _config;
    private readonly GraphRestService _graphRestService;


    public UploadController(ImportProspService prospService, GraphServiceClient  graphService, IServiceProvider serviceProvider, IConfiguration config, GraphRestService graphRestService)
    {
        _prospService = prospService;
        _graphServiceClient = graphService;
        _serviceProvider = serviceProvider;
        _config = config;
        _graphRestService = graphRestService;

    }

    [HttpGet(Name = nameof(GetSharePointFileNamesAndId))]
    public List<DriveItemDto> GetSharePointFileNamesAndId()
    {
        var siteId = _config["SharePoint:Prosp:SiteId"];
        var dto = new List<DriveItemDto>();
        var query = _config["SharePoint:Prosp:FileQuery"];
        var validMimeTypes = new List<string>
        {
            ExcelMimeTypes.Xls,
            ExcelMimeTypes.Xlsb,
            ExcelMimeTypes.Xlsm,
            ExcelMimeTypes.Xlsx
        };

        var driveItemSearchCollectionPage = _graphServiceClient.Sites[siteId]
            .Drive.Root
            .Search(query)
            .Request()
            .GetAsync()
            .GetAwaiter()
            .GetResult();

        foreach (var driveItem in driveItemSearchCollectionPage.Where(item =>
                     item.File != null && validMimeTypes.Contains(item.File.MimeType)))
        {
            ConvertToDto(driveItem, dto);
        }

        return dto;
        // return _graphRestService.GetFilesFromSite();
    }

    [HttpPost(Name = "Upload"), DisableRequestSizeLimit]
    public async Task<ProjectDto?> Upload([FromQuery] Guid projectId, [FromQuery] Guid sourceCaseId)
    {
        try
        {
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.First();
            var assets = new Dictionary<string, bool>()
                {
                    {"Surf", false},
                    {"Topside", false},
                    {"Substructure", false},
                    {"Transport", false},

                };

            if (file.Length > 0)
            {
                if (formCollection.TryGetValue("Surf", out var surf) && surf == "true")
                {
                    assets["Surf"] = true;
                }
                if (formCollection.TryGetValue("Topside", out var topside) && topside == "true")
                {
                    assets["Topside"] = true;
                }
                if (formCollection.TryGetValue("Substructure", out var substructure) && substructure == "true")
                {
                    assets["Substructure"] = true;
                }
                if (formCollection.TryGetValue("Transport", out var transport) && transport == "true")
                {
                    assets["Transport"] = true;
                }
                var dto = _prospService.ImportProsp(file, sourceCaseId, projectId, assets);
                return dto;
            }
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    [HttpPost("sharepoint", Name = nameof(ImportFromSharepointAsync))]
    [DisableRequestSizeLimit]
    public async Task<ProjectDto?> ImportFromSharepointAsync([FromQuery] Guid projectId, [FromBody] SharePointImportDto[] dto)
    {
        foreach (var item in dto)
        {
            Console.WriteLine(item.Id);
        }

        try
        {
            var projectDto = await ConvertSharepointFilesToProjectDto(projectId, dto);

            return projectDto;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private async Task<ProjectDto> ConvertSharepointFilesToProjectDto(Guid projectId, SharePointImportDto[] dto)
    {
        var projectDto = new ProjectDto();
        var siteId = _config["SharePoint:Prosp:SiteId"];
        var fileIds = new Dictionary<Guid, string>();
        foreach (var fileInfo in dto)
        {
            fileIds.Add(new Guid(fileInfo.Id!), fileInfo.SharePointFileId);
        }

        var caseAndStream = new Dictionary<Guid, Stream>();
        foreach (var item in fileIds)
        {
            var driveItemStream = await _graphServiceClient.Sites[siteId]
                .Drive.Items[item.Value]
                .Content.Request()
                .GetAsync();

            caseAndStream.Add(item.Key, driveItemStream);
        }

        foreach (var item in caseAndStream)
        {
            if (item.Value.Length > 0)
            {
                foreach (var iteminfo in dto.Where(d => new Guid(d.Id) == item.Key))
                {
                    var assets = MapAssets(iteminfo.Surf, iteminfo.Substructure, iteminfo.Topside, iteminfo.Transport);
                    projectDto = _prospService.ImportProsp(item.Value, item.Key, projectId, assets);
                }

                // Thread.Sleep(3000);
            }
        }

        return projectDto;
    }

    private Dictionary<string, bool> MapAssets(bool surf, bool substructure, bool topside, bool transport)
    {
        var assets = new Dictionary<string, bool>()
                {
                    {"Surf", false},
                    {"Topside", false},
                    {"Substructure", false},
                    {"Transport", false},
                };

        if (surf) { assets["Surf"] = true; }
        if (substructure) { assets["Substructure"] = true; }
        if (topside) { assets["Topside"] = true; }
        if (transport) { assets["Transport"] = true; }

        return assets;
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

}
