using api.Dtos;
using api.Helpers;
using api.Models;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;

using Surf = api.Models.Surf;
using Transport = api.Models.Transport;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class PROSPController : ControllerBase
{
    private const string isCheckedAsset = "true";
    private readonly IConfiguration _config;
    private readonly GraphServiceClient _graphServiceClient;
    private readonly ProspImportHelper _prospHelper;
    private readonly ImportProspService _prospService;


    public PROSPController(ImportProspService prospService,
        GraphServiceClient graphService,
        IConfiguration config,
        ProspImportHelper prospImportHelper)
    {
        _prospService = prospService;
        _graphServiceClient = graphService;
        _config = config;
        _prospHelper = prospImportHelper;
    }

    [HttpGet("sharepoint", Name = nameof(GetSharePointFileNamesAndId))]
    public List<DriveItemDto> GetSharePointFileNamesAndId()
    {
        var siteId = _config["SharePoint:Prosp:SiteId"];
        var dto = new List<DriveItemDto>();
        var query = _config["SharePoint:Prosp:FileQuery"];
        var validMimeTypes = _prospHelper.ValidMimeTypes();

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
            _prospHelper.ConvertToDto(driveItem, dto);
        }

        return dto;
    }

    [HttpPost("sharepoint", Name = nameof(ImportFromSharepointAsync))]
    [DisableRequestSizeLimit]
    public async Task<ProjectDto?> ImportFromSharepointAsync([FromQuery] Guid projectId,
        [FromBody] SharePointImportDto[] dto)
    {
        try
        {
            var projectDto = await _prospHelper.ConvertSharepointFilesToProjectDto(projectId, dto);

            return projectDto;
        }
        catch (Exception)
        {
            return null;
        }
    }

    [HttpPost("local", Name = "Upload")]
    [DisableRequestSizeLimit]
    public async Task<ProjectDto?> Upload([FromQuery] Guid projectId, [FromQuery] Guid sourceCaseId)
    {
        try
        {
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.First();
            var assets = new Dictionary<string, bool>
            {
                { nameof(Surf), false },
                { nameof(Topside), false },
                { nameof(Substructure), false },
                { nameof(Transport), false }
            };

            if (file.Length > 0)
            {
                foreach (var item in assets)
                {
                    if (formCollection.TryGetValue(nameof(item.Key), out var asset) && asset == isCheckedAsset)
                    {
                        assets[nameof(asset)] = true;
                    }
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
}
