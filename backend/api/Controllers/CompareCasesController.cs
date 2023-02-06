using api.Dtos;
using api.Services;

using Api.Authorization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.ReadOnly,
    ApplicationRole.User
)]
public class CompareCasesController : ControllerBase
{
    private readonly ICompareCasesService _compareCasesService;
    public CompareCasesController(ICompareCasesService compareCasesService)
    {
        _compareCasesService = compareCasesService;
    }

    [HttpPost("{projectId}/calculateCompareCasesTotals", Name = "CalculateCompareCasesTotals")]
    public List<CompareCasesDto> CalculateCompareCasesTotals(Guid projectId)
    {
        return new List<CompareCasesDto>(_compareCasesService.Calculate(projectId));
    }
}
