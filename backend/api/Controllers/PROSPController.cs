using api.Dtos;
using api.Services;

using Api.Authorization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.ReadOnly,
    ApplicationRole.User
)]
public class PROSPController : ControllerBase
{
    private const string isCheckedAsset = "true";
    private readonly IConfiguration _config;
    private readonly GraphServiceClient _graphServiceClient;
    private readonly ILogger<PROSPController> _logger;
    private readonly IProjectService _projectService;
    private readonly ProspExcelImportService _prospExcelImportService;
    private readonly ProspSharepointImportService _prospSharepointImportService;


    public PROSPController(ProspExcelImportService prospExcelImportService,
        GraphServiceClient graphService,
        IConfiguration config,
        ProspSharepointImportService prospSharepointImportImportService,
        IProjectService projectService,
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
        var deltaDriveItemCollection =
            _prospSharepointImportService.GetDeltaDriveItemCollectionFromSite(dto.url);

        return ProspSharepointImportService.GetExcelDriveItemsFromSite(deltaDriveItemCollection.Result);
    }

    [HttpPost("{projectId}/sharepoint", Name = nameof(ImportFilesFromSharepointAsync))]
    [DisableRequestSizeLimit]
    public async Task<ProjectDto?> ImportFilesFromSharepointAsync([FromQuery] Guid projectId,
        [FromBody] SharePointImportDto[] dtos)
    {
        try
        {
            var projectDto = await _prospSharepointImportService.ConvertSharepointFilesToProjectDto(projectId, dtos);

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

    public class urlDto
    {
        public string? url { get; set; }
    }
}
