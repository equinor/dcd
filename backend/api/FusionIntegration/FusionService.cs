using api.Services.FusionIntegration.Models;

using Fusion.Integration;

using Newtonsoft.Json;

namespace api.Services.FusionIntegration;

public class FusionService(
    IFusionContextResolver fusionContextResolver,
    ILogger<FusionService> logger) : IFusionService
{
    public async Task<FusionProjectMaster?> GetProjectMasterFromFusionContextId(Guid contextId)
    {
        var projectMasterContext = await ResolveProjectMasterContext(contextId);

        // ?: Did we obtain a ProjectMaster context?
        if (projectMasterContext == null)
        {
            // -> No, still not found. Then we log this and fail hard, as the callee should have provided with a
            // valid ProjectMaster (context) ID.
            logger.LogInformation($"Could not resolve ProjectMaster context from Fusion using GUID '{{contextId}}'", contextId);
            return null;
        }

        var serializedProjectMaster = JsonConvert.SerializeObject(projectMasterContext.Value);
        FusionProjectMaster? fusionProjectMaster = JsonConvert.DeserializeObject<FusionProjectMaster>(serializedProjectMaster);

        if (fusionProjectMaster == null)
        {
            logger.LogError("Project Master with ID '{contextId}' was obtained from Fusion, but conversion to explicit type failed", contextId);
            return null;
        }

        return fusionProjectMaster;
    }

    private async Task<FusionContext?> ResolveProjectMasterContext(Guid contextId)
    {
        FusionContext? projectMasterContext = await fusionContextResolver.ResolveContextAsync(contextId, FusionContextType.ProjectMaster);

        Console.WriteLine("ResolveProjectMasterContext - contextId: " + contextId);
        Console.WriteLine("ResolveProjectMasterContext - projectMasterContext: " + projectMasterContext);
        // It might be the GUID provided was the ProjectMaster ID and not the GUID of the Fusion Context. Will
        // thus attempt to query for the ProjectMaster "directly" if not found.
        if (projectMasterContext == null)
        {
            IEnumerable<FusionContext> queryContextsAsync = await fusionContextResolver.QueryContextsAsync(
                query =>
                {
                    query
                        .WhereTypeIn(FusionContextType.ProjectMaster)
                        .WhereExternalId(contextId.ToString(), QueryOperator.Equals);
                });
            projectMasterContext = queryContextsAsync.FirstOrDefault();
        }

        return projectMasterContext;
    }
}
