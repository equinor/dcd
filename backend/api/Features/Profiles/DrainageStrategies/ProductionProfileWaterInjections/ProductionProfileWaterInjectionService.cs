using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.DrainageStrategies.ProductionProfileWaterInjections;

public class ProductionProfileWaterInjectionService(
    DcdDbContext context,
    IConversionMapperService conversionMapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : DrainageStrategyProfileBaseService(context, recalculationService, projectIntegrityService, conversionMapperService)
{
    public async Task<TimeSeriesVolumeDto> CreateProductionProfileWaterInjection(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateTimeSeriesVolumeDto createProfileDto)
    {
        return await CreateDrainageStrategyProfile<ProductionProfileWaterInjection, TimeSeriesVolumeDto, CreateTimeSeriesVolumeDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            d => d.ProductionProfileWaterInjection != null);
    }

    public async Task<TimeSeriesVolumeDto> UpdateProductionProfileWaterInjection(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        UpdateTimeSeriesVolumeDto updatedProductionProfileWaterInjectionDto)
    {
        return await UpdateDrainageStrategyProfile<ProductionProfileWaterInjection, TimeSeriesVolumeDto, UpdateTimeSeriesVolumeDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileId,
            updatedProductionProfileWaterInjectionDto,
            id => Context.ProductionProfileWaterInjection.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
