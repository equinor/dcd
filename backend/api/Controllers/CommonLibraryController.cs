using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class CommonLibraryController : ControllerBase
{
    private readonly CommonLibraryService _commonLibraryService;
    private readonly ILogger<CommonLibraryController> _logger;

    public CommonLibraryController(ILogger<CommonLibraryController> logger, CommonLibraryService commonLibraryService)
    {
        _logger = logger;
        _commonLibraryService = commonLibraryService;
    }

    [HttpGet("projects", Name = "GetProjectsFromCommonLibrary")]
    public async Task<IEnumerable<CommonLibraryProjectDto>>? GetProjectsFromCommonLibrary()
    {
        return await _commonLibraryService.GetProjectsFromCommonLibrary();
    }
}
