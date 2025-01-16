using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.DrainageStrategies.FuelFlaringAndLossesOverrides.Dtos;
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
    public async Task<FuelFlaringAndLossesOverrideDto> CreateFuelFlaringAndLossesOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateFuelFlaringAndLossesOverrideDto createProfileDto)
    {
        return await CreateDrainageStrategyProfile<FuelFlaringAndLossesOverride, FuelFlaringAndLossesOverrideDto, CreateFuelFlaringAndLossesOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            d => d.FuelFlaringAndLossesOverride != null);
    }

    public async Task<FuelFlaringAndLossesOverrideDto> UpdateFuelFlaringAndLossesOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid profileId,
        UpdateFuelFlaringAndLossesOverrideDto updateDto)
    {
        return await UpdateDrainageStrategyProfile<FuelFlaringAndLossesOverride, FuelFlaringAndLossesOverrideDto, UpdateFuelFlaringAndLossesOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            profileId,
            updateDto,
            id => Context.FuelFlaringAndLossesOverride.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
