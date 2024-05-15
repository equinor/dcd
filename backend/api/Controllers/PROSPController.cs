using api.Authorization;
using api.Dtos;
using api.Services;

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
    public async Task<ActionResult<List<DriveItemDto>>> GetSharePointFileNamesAndId([FromBody] urlDto urlDto)
    {
        if (urlDto == null || string.IsNullOrWhiteSpace(urlDto.url))
        {
            return BadRequest("URL is required.");
        }

        try
        {
            var driveItems = await _prospSharepointImportService.GetDeltaDriveItemCollectionFromSite(urlDto.url);
            return Ok(driveItems);
        }
        catch (ServiceException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            _logger.LogError(ex, "Access Denied when attempting to access SharePoint site: {Url}", urlDto.url);
            return StatusCode(StatusCodes.Status403Forbidden, "Access to SharePoint resource was denied.");
        }
        catch (ProspSharepointImportService.AccessDeniedException ex)
        {
            _logger.LogError(ex, "Custom Access Denied when attempting to access SharePoint site: {Url}", urlDto.url);
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing your request for URL: {Url}", urlDto.url);
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal server error occurred.");
        }
    }

    [HttpPost("{projectId}/sharepoint", Name = nameof(ImportFilesFromSharepointAsync))]
    [DisableRequestSizeLimit]
    public async Task<ActionResult<ProjectDto>> ImportFilesFromSharepointAsync([FromQuery] Guid projectId,
        [FromBody] SharePointImportDto[] dtos)
    {
        try
        {
            var projectDto = await _prospSharepointImportService.ConvertSharepointFilesToProjectDto(projectId, dtos);

            if (projectDto.Id == projectId)
            {
                return Ok(projectDto);
            }
            else
            {
                var fallbackDto = _projectService.GetProjectDto(projectId);
                return Ok(fallbackDto);
            }
        }
        catch (ServiceException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            _logger.LogError($"Access denied when trying to import files from SharePoint for project {projectId}: {ex.Message}");
            return StatusCode(StatusCodes.Status403Forbidden, "Access to SharePoint resource was denied.");
        }
        // Handle other potential ServiceException cases, if necessary
        catch (Exception e)
        {
            _logger.LogError($"An error occurred while importing files from SharePoint for project {projectId}: {e.Message}");
            // Consider returning a more generic error message to avoid exposing sensitive details
            // and ensure it's a client-friendly message
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }

    public class urlDto
    {
        public string? url { get; set; }
    }
}
