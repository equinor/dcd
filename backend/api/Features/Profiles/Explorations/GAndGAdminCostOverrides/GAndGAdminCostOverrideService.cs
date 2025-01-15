using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Explorations.GAndGAdminCostOverrides.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Explorations.GAndGAdminCostOverrides;

public class GAndGAdminCostOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService) : ExplorationProfileBaseService(context, mapperService, projectIntegrityService, recalculationService)
{
    public async Task<GAndGAdminCostOverrideDto> CreateGAndGAdminCostOverride(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        CreateGAndGAdminCostOverrideDto createProfileDto)
    {
        return await CreateExplorationProfile<GAndGAdminCostOverride, GAndGAdminCostOverrideDto, CreateGAndGAdminCostOverrideDto>(
            projectId,
            caseId,
            explorationId,
            createProfileDto,
            d => d.GAndGAdminCostOverride != null);
    }

    public async Task<GAndGAdminCostOverrideDto> UpdateGAndGAdminCostOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateGAndGAdminCostOverrideDto updateDto)
    {
        return await UpdateExplorationCostProfile<GAndGAdminCostOverride, GAndGAdminCostOverrideDto, UpdateGAndGAdminCostOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            id => Context.GAndGAdminCostOverride.Include(x => x.Exploration).SingleAsync(x => x.Id == id));
    }
}
