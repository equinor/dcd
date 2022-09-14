using api.Dtos;
using api.Models;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class PROSPController : ControllerBase
{
    private const string isCheckedAsset = "true";
    private readonly IConfiguration _config;
    private readonly GraphServiceClient _graphServiceClient;
    private readonly ILogger<PROSPController> _logger;
    private readonly ProjectService _projectService;
    private readonly ProspExcelImportService _prospExcelImportService;
    private readonly ProspSharepointImportService _prospSharepointImportService;


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

    [HttpPost("sharepoint", Name = nameof(GetSharePointFileNamesAndId))]
    public List<DriveItemDto> GetSharePointFileNamesAndId([FromBody] urlDto dto)
    {
        var driveItemCollectionFromSite =
            _prospSharepointImportService.GetDeltaDriveItemCollectionFromSite(dto.url);
        var filesFromSite = _prospSharepointImportService.GetFilesFromSite(driveItemCollectionFromSite);

        return filesFromSite;
    }

    [HttpPost("{projectId}/sharepoint", Name = nameof(ImportFilesFromSharepointAsync))]
    [DisableRequestSizeLimit]
    public async Task<ProjectDto?> ImportFilesFromSharepointAsync([FromQuery] Guid projectId,
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

    public class urlDto
    {
        public string url { get; set; }
    }
}
