using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
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
    public async Task<TimeSeriesCostOverrideDto> CreateGAndGAdminCostOverride(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        CreateTimeSeriesCostOverrideDto createProfileDto)
    {
        return await CreateExplorationProfile<GAndGAdminCostOverride, TimeSeriesCostOverrideDto, CreateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            explorationId,
            createProfileDto,
            d => d.GAndGAdminCostOverride != null);
    }

    public async Task<TimeSeriesCostOverrideDto> UpdateGAndGAdminCostOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateTimeSeriesCostOverrideDto updateDto)
    {
        return await UpdateExplorationCostProfile<GAndGAdminCostOverride, TimeSeriesCostOverrideDto, UpdateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            id => Context.GAndGAdminCostOverride.Include(x => x.Exploration).SingleAsync(x => x.Id == id));
    }
}
