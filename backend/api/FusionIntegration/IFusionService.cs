using Api.Services.FusionIntegration.Models;

namespace Api.Services.FusionIntegration;

public interface IFusionService
{
    /// <summary>
    ///     Obtain a <see cref="ProjectMaster" /> based on the given context ID/projectMasterId.
    /// </summary>
    /// <param name="contextId">The projectMaster ID to query for.</param>
    /// <returns>A <see cref="ProjectMaster" /> for the given id.</returns>
    /// <exception cref="OperationFailed">If no projectMaster was found for the given ID.</exception>
    public Task<FusionProjectMaster> ProjectMasterAsync(Guid contextId);
}
