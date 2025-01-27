using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
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
    public async Task<TimeSeriesCostDto> CreateAdditionalProductionProfileOil(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateTimeSeriesCostDto createAdditionalProductionProfileOilDto)
    {
        return await CreateDrainageStrategyProfile<AdditionalProductionProfileOil, TimeSeriesCostDto, CreateTimeSeriesCostDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createAdditionalProductionProfileOilDto,
            d => d.AdditionalProductionProfileOil != null);
    }

    public async Task<TimeSeriesCostDto> UpdateAdditionalProductionProfileOil(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid additionalProductionProfileOilId,
        UpdateTimeSeriesCostDto updatedAdditionalProductionProfileOilDto)
    {
        return await UpdateDrainageStrategyProfile<AdditionalProductionProfileOil, TimeSeriesCostDto, UpdateTimeSeriesCostDto>(
            projectId,
            caseId,
            drainageStrategyId,
            additionalProductionProfileOilId,
            updatedAdditionalProductionProfileOilDto,
            id => Context.AdditionalProductionProfileOil.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
