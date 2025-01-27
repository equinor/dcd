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
    public async Task<TimeSeriesCostOverrideDto> CreateImportedElectricityOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateTimeSeriesCostDto createProfileDto)
    {
        return await CreateDrainageStrategyProfile<ImportedElectricityOverride, TimeSeriesCostOverrideDto, CreateTimeSeriesCostDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            d => d.ImportedElectricityOverride != null);
    }

    public async Task<TimeSeriesCostOverrideDto> UpdateImportedElectricityOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid profileId,
        UpdateTimeSeriesCostOverrideDto updateDto)
    {
        return await UpdateDrainageStrategyProfile<ImportedElectricityOverride, TimeSeriesCostOverrideDto, UpdateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            profileId,
            updateDto,
            id => Context.ImportedElectricityOverride.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
