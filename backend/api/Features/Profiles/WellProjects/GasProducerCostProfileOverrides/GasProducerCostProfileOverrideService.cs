using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.WellProjects.GasProducerCostProfileOverrides;

public class GasProducerCostProfileOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : WellProjectProfileBaseService(context, mapperService, projectIntegrityService, recalculationService)
{
    public async Task<TimeSeriesCostOverrideDto> CreateGasProducerCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        CreateTimeSeriesCostOverrideDto createProfileDto)
    {
        return await CreateWellProjectProfile<GasProducerCostProfileOverride, TimeSeriesCostOverrideDto, CreateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            createProfileDto,
            d => d.GasProducerCostProfileOverride != null);
    }

    public async Task<TimeSeriesCostOverrideDto> UpdateGasProducerCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateTimeSeriesCostOverrideDto updateDto)
    {
        return await UpdateWellProjectCostProfile<GasProducerCostProfileOverride, TimeSeriesCostOverrideDto, UpdateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            id => Context.GasProducerCostProfileOverride.Include(x => x.WellProject).SingleAsync(x => x.Id == id));
    }
}
