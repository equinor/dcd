using api.Features.FusionIntegration.ProjectMaster.Models;

namespace api.Features.FusionIntegration.ProjectMaster;

public interface IFusionService
{
    Task<FusionProjectMaster?> GetProjectMasterFromFusionContextId(Guid contextId);
}
