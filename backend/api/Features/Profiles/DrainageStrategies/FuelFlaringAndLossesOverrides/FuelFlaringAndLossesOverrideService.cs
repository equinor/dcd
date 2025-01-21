using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.DrainageStrategies.FuelFlaringAndLossesOverrides;

public class FuelFlaringAndLossesOverrideService(
    DcdDbContext context,
    IConversionMapperService conversionMapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : DrainageStrategyProfileBaseService(context, recalculationService, projectIntegrityService, conversionMapperService)
{
    public async Task<TimeSeriesVolumeOverrideDto> CreateFuelFlaringAndLossesOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateTimeSeriesVolumeOverrideDto createProfileDto)
    {
        return await CreateDrainageStrategyProfile<FuelFlaringAndLossesOverride, TimeSeriesVolumeOverrideDto, CreateTimeSeriesVolumeOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            d => d.FuelFlaringAndLossesOverride != null);
    }

    public async Task<TimeSeriesVolumeOverrideDto> UpdateFuelFlaringAndLossesOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid profileId,
        UpdateTimeSeriesVolumeOverrideDto updateDto)
    {
        return await UpdateDrainageStrategyProfile<FuelFlaringAndLossesOverride, TimeSeriesVolumeOverrideDto, UpdateTimeSeriesVolumeOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            profileId,
            updateDto,
            id => Context.FuelFlaringAndLossesOverride.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
