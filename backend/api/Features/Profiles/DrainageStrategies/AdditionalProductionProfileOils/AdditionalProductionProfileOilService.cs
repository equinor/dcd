using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileOils.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileOils;

public class AdditionalProductionProfileOilService(
    DcdDbContext context,
    IConversionMapperService conversionMapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : DrainageStrategyProfileBaseService(context, recalculationService, projectIntegrityService, conversionMapperService)
{
    public async Task<AdditionalProductionProfileOilDto> CreateAdditionalProductionProfileOil(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateAdditionalProductionProfileOilDto createAdditionalProductionProfileOilDto)
    {
        return await CreateDrainageStrategyProfile<AdditionalProductionProfileOil, AdditionalProductionProfileOilDto, CreateAdditionalProductionProfileOilDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createAdditionalProductionProfileOilDto,
            d => d.AdditionalProductionProfileOil != null);
    }

    public async Task<AdditionalProductionProfileOilDto> UpdateAdditionalProductionProfileOil(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid additionalProductionProfileOilId,
        UpdateAdditionalProductionProfileOilDto updatedAdditionalProductionProfileOilDto)
    {
        return await UpdateDrainageStrategyProfile<AdditionalProductionProfileOil, AdditionalProductionProfileOilDto, UpdateAdditionalProductionProfileOilDto>(
            projectId,
            caseId,
            drainageStrategyId,
            additionalProductionProfileOilId,
            updatedAdditionalProductionProfileOilDto,
            id => Context.AdditionalProductionProfileOil.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
