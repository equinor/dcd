using api.Features.FusionIntegration.ProjectMaster.Models;

using Fusion.Integration;

using Newtonsoft.Json;

namespace api.Features.FusionIntegration.ProjectMaster;

public class FusionService(IFusionContextResolver fusionContextResolver, ILogger<FusionService> logger) : IFusionService
{
    public async Task<FusionProjectMaster?> GetProjectMasterFromFusionContextId(Guid contextId)
    {
        var projectMasterContext = await ResolveProjectMasterContext(contextId);

        if (projectMasterContext == null)
        {
            logger.LogInformation($"Could not resolve ProjectMaster context from Fusion using GUID '{contextId}'");

            return null;
        }

        var serializedProjectMaster = JsonConvert.SerializeObject(projectMasterContext.Value);
        var fusionProjectMaster = JsonConvert.DeserializeObject<FusionProjectMaster>(serializedProjectMaster);

        if (fusionProjectMaster == null)
        {
            logger.LogError("Project Master with ID '{contextId}' was obtained from Fusion, but conversion to explicit type failed", contextId);

            return null;
        }

        return fusionProjectMaster;
    }

    private async Task<FusionContext?> ResolveProjectMasterContext(Guid contextId)
    {
        var projectMasterContext = await fusionContextResolver.ResolveContextAsync(contextId, FusionContextType.ProjectMaster);

        // It might be the GUID provided was the ProjectMaster ID and not the GUID of the Fusion Context. Will
        // thus attempt to query for the ProjectMaster "directly" if not found.
        if (projectMasterContext == null)
        {
            var queryContextsAsync = await fusionContextResolver.QueryContextsAsync(
                query => query
                    .WhereTypeIn(FusionContextType.ProjectMaster)
                    .WhereExternalId(contextId.ToString(), QueryOperator.Equals));

            projectMasterContext = queryContextsAsync.FirstOrDefault();
        }

        return projectMasterContext;
    }
}
