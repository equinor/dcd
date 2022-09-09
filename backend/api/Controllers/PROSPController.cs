using api.Dtos;
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
    private readonly ProspSharepointImportService _prospSharepointImportService;
    private readonly ProspExcelImportService _prospExcelImportService;
    private readonly ProjectService _projectService;
    private readonly ILogger<PROSPController> _logger;


    public PROSPController(ProspExcelImportService prospExcelImportService,
        GraphServiceClient graphService,
        IConfiguration config,
        ProspSharepointImportService prospSharepointImportImportService,
        ProjectService projectService,
        ILoggerFactory loggerFactory)
    {
        _prospExcelImportService = prospExcelImportService;
        _graphServiceClient = graphService;
        _config = config;
        _prospSharepointImportService = prospSharepointImportImportService;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<PROSPController>();
    }

    [HttpGet("sharepoint", Name = nameof(GetSharePointFileNamesAndId))]
    public List<DriveItemDto> GetSharePointFileNamesAndId()
    {
        var siteId = _config["SharePoint:Prosp:SiteId"];
        var dto = new List<DriveItemDto>();
        var validMimeTypes = _prospSharepointImportService.ValidMimeTypes();

        var driveItemSearchCollectionPage = _graphServiceClient.Sites[siteId]
            .Drive.Root
            .Search(string.Empty)
            .Request()
            .GetAsync()
            .GetAwaiter()
            .GetResult();

        foreach (var driveItem in driveItemSearchCollectionPage.Where(item =>
                     item.File != null && validMimeTypes.Contains(item.File.MimeType)))
        {
            _prospSharepointImportService.ConvertToDto(driveItem, dto);
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
            var projectDto = await _prospSharepointImportService.ConvertSharepointFilesToProjectDto(projectId, dto);

            return projectDto.ProjectId == projectId
                ? projectDto
                : _projectService.GetProjectDto(projectId);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return _projectService.GetProjectDto(projectId);
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

                var dto = _prospExcelImportService.ImportProsp(file, sourceCaseId, projectId, assets);
                return dto;
            }

            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return _projectService.GetProjectDto(projectId);
        }
    }
}
