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
public class GenerateCo2IntensityTotalController : ControllerBase
{
    private readonly GenerateCo2IntensityTotal _generateCo2IntensityTotal;
    public GenerateCo2IntensityTotalController(IServiceProvider serviceProvider)
    {
        _generateCo2IntensityTotal = serviceProvider.GetRequiredService<GenerateCo2IntensityTotal>();
    }

    [HttpPost("{caseId}/generateCo2IntensityTotal", Name = "GenerateCo2IntensityTotal")]
    public double GenerateCo2IntensityTotal(Guid caseId)
    {
        return _generateCo2IntensityTotal.Calculate(caseId);
    }
}
