using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.DrainageStrategies.ImportedElectricityOverrides.Dtos;
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
    public async Task<ImportedElectricityOverrideDto> CreateImportedElectricityOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateImportedElectricityOverrideDto createProfileDto)
    {
        return await CreateDrainageStrategyProfile<ImportedElectricityOverride, ImportedElectricityOverrideDto, CreateImportedElectricityOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            d => d.ImportedElectricityOverride != null);
    }

    public async Task<ImportedElectricityOverrideDto> UpdateImportedElectricityOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid profileId,
        UpdateImportedElectricityOverrideDto updateDto)
    {
        return await UpdateDrainageStrategyProfile<ImportedElectricityOverride, ImportedElectricityOverrideDto, UpdateImportedElectricityOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            profileId,
            updateDto,
            id => Context.ImportedElectricityOverride.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
