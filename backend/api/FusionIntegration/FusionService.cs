namespace Api.Services.FusionIntegration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Api.Services.FusionIntegration.Models;
    using Api.Services.Models;

    using Fusion;
    using Fusion.ApiClients.Org;
    using Fusion.Integration;
    using Fusion.Integration.Org;

    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    using PersonIdentifier = Fusion.Integration.Profile.PersonIdentifier;

    public class FusionService : IFusionService
    {
        private readonly IFusionContextResolver fusionContextResolver;
        private readonly IProjectOrgResolver projectOrgResolver;
        private readonly ILogger<FusionService> logger;
        private readonly IOrgApiClientFactory orgApiClientFactory;
        private readonly IFusionProfileResolver profileResolver;

        public FusionService(
            IFusionContextResolver fusionContextResolver,
            ILogger<FusionService> logger,
            IProjectOrgResolver projectOrgResolver,
            IOrgApiClientFactory orgApiClientFactory,
            IFusionProfileResolver profileResolver)
        {
            this.fusionContextResolver = fusionContextResolver;
            this.logger = logger;
            this.projectOrgResolver = projectOrgResolver;
            this.orgApiClientFactory = orgApiClientFactory;
            this.profileResolver = profileResolver;
        }

        public async Task<FusionProjectMaster> ProjectMasterAsync(Guid contextId)
        {
            var projectMasterContext = await ResolveProjectMasterContext(contextId);

            // ?: Did we obtain a ProjectMaster context?
            if (projectMasterContext == null)
            {
                // -> No, still not found. Then we log this and fail hard, as the callee should have provided with a
                // valid ProjectMaster (context) ID.
                logger.LogError(
                    "Could not resolve ProjectMaster context from Fusion using GUID '{ProjectMasterId}'",
                    contextId);
                throw new Exception();
            }

            // A note on the serializer/deserialize here:
            // First of all, this is the suggested way of getting a strongly typed object from Fusion, as both described
            // in the Fusion examples and when introspecting the .getValue()-method on fusionContext.
            // As Fusion seems to be using Newtonsoft internally we have discovered by trial and error that this also
            // works best here.
            var serializedProjectMaster = JsonConvert.SerializeObject(projectMasterContext.Value);
            FusionProjectMaster? fusionProjectMaster = JsonConvert.DeserializeObject<FusionProjectMaster>(serializedProjectMaster);

            if (fusionProjectMaster == null)
            {
                logger.LogError(
                    "Project Master with ID '{ProjectMasterId}' was obtained from Fusion, but conversion to explicit " +
                    "type failed",
                    contextId);
                throw new Exception();
            }

            // var fusionProject = await ResolveFusionProjectAsync(projectMasterContext);

            // var orgChart = await ResolveOrgChart(projectMasterContext);

            return fusionProjectMaster;
        }

        private async Task<FusionContext?> ResolveProjectMasterContext(Guid contextId)
        {
            FusionContext? projectMasterContext = await fusionContextResolver.ResolveContextAsync(contextId, FusionContextType.ProjectMaster);

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

        private async Task<FusionProject?> ResolveFusionProjectAsync(FusionContext fusionContext)
        {
            try
            {
                ApiProjectV2? project = await ResolveProjectAsync(fusionContext, ApiClientMode.Delegate);

                if (project == null)
                    return null;

                return new FusionProject(
                    new FusionDecisionGateDates(project.Dates.Gates),
                    project.Director?.LatestPositionInstance(DateTime.Now)?.AssignedPerson?.AzureUniqueId);
            }
            catch (OrgApiError apiError) when (apiError.HttpStatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            {
                logger.LogWarning("An attempt was made to obtain Project data from Fusion using a DELEGATED token, " +
                                  "which failed. Doing a fallback using APPLICATION token, but where DG-dates are " +
                                  "being replaced with mock data, as we are not sure if the user actually is " +
                                  "permitted to see this");
                ApiProjectV2? project = await ResolveProjectAsync(fusionContext, ApiClientMode.Application);

                if (project == null)
                    return null;

                return new FusionProject(
                    FusionDecisionGateDates.MockDates(),
                    project.Director.LatestPositionInstance(DateTime.Now)?.AssignedPerson?.AzureUniqueId);
            }
        }

        private async Task<ApiProjectV2?> ResolveProjectAsync(FusionContext projectMasterFusionContext, ApiClientMode clientMode)
        {

            var orgChartContext = await ResolveOrgChart(projectMasterFusionContext);
            if (orgChartContext == null)
                return null;

            IOrgApiClient orgApiClient = orgApiClientFactory.CreateClient(clientMode);
            return await orgApiClient.GetProjectOrDefaultV2Async(orgChartContext.ExternalId);
        }

        private async Task<FusionContext?> ResolveOrgChart(FusionContext projectMasterContext)
        {
            IEnumerable<FusionContext> projectMasterRelations = await FusionContextRelationsAsync(projectMasterContext);
            return projectMasterRelations.FirstOrDefault(relationContext => relationContext.Type == FusionContextType.OrgChart);
        }

        private async Task<IEnumerable<FusionContext>> FusionContextRelationsAsync(FusionContext fusionContext)
        {
            return await fusionContextResolver.GetContextRelationsAsync(fusionContext);
        }

        public Task<IDictionary<Guid, Guid?>> ResolvePersonIdsFromPositionIdsAsync(IEnumerable<Guid> positionIds)
        {
            throw new NotImplementedException();
        }

        public Task<Guid?> ResolvePersonIdFromPositionIdAsync(Guid positionId)
        {
            throw new NotImplementedException();
        }

        public Task<string?> ResolveUserEmailFromPersonId(Guid azureUniqueId)
        {
            throw new NotImplementedException();
        }

        Task<ProjectMaster> IFusionService.ProjectMasterAsync(Guid contextId)
        {
            throw new NotImplementedException();
        }
    }
}
