using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.DrainageStrategies.Co2EmissionsOverrides;

public class Co2EmissionsOverrideService(
    DcdDbContext context,
    IConversionMapperService conversionMapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : DrainageStrategyProfileBaseService(context, recalculationService, projectIntegrityService, conversionMapperService)
{
    public async Task<TimeSeriesCostOverrideDto> CreateCo2EmissionsOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateTimeSeriesCostOverrideDto createProfileDto)
    {
        return await CreateDrainageStrategyProfile<Co2EmissionsOverride, TimeSeriesCostOverrideDto, CreateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            d => d.Co2EmissionsOverride != null);
    }

    public async Task<TimeSeriesCostOverrideDto> UpdateCo2EmissionsOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid profileId,
        UpdateTimeSeriesCostOverrideDto updateDto)
    {
        return await UpdateDrainageStrategyProfile<Co2EmissionsOverride, TimeSeriesCostOverrideDto, UpdateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            profileId,
            updateDto,
            id => Context.Co2EmissionsOverride.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
