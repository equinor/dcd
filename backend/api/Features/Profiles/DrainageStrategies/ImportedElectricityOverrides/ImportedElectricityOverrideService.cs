using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.DrainageStrategies.ImportedElectricityOverrides;

public class ImportedElectricityOverrideService(
    DcdDbContext context,
    IConversionMapperService conversionMapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : DrainageStrategyProfileBaseService(context, recalculationService, projectIntegrityService, conversionMapperService)
{
    public async Task<TimeSeriesEnergyOverrideDto> CreateImportedElectricityOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateTimeSeriesEnergyDto createProfileDto)
    {
        return await CreateDrainageStrategyProfile<ImportedElectricityOverride, TimeSeriesEnergyOverrideDto, CreateTimeSeriesEnergyDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            d => d.ImportedElectricityOverride != null);
    }

    public async Task<TimeSeriesEnergyOverrideDto> UpdateImportedElectricityOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid profileId,
        UpdateTimeSeriesEnergyOverrideDto updateDto)
    {
        return await UpdateDrainageStrategyProfile<ImportedElectricityOverride, TimeSeriesEnergyOverrideDto, UpdateTimeSeriesEnergyOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            profileId,
            updateDto,
            id => Context.ImportedElectricityOverride.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
