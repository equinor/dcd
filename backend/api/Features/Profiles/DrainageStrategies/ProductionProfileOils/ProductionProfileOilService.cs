using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.DrainageStrategies.ProductionProfileOils;

public class ProductionProfileOilService(
    DcdDbContext context,
    IConversionMapperService conversionMapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : DrainageStrategyProfileBaseService(context, recalculationService, projectIntegrityService, conversionMapperService)
{
    public async Task<TimeSeriesVolumeDto> CreateProductionProfileOil(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateTimeSeriesVolumeDto createProductionProfileOilDto)
    {
        return await CreateDrainageStrategyProfile<ProductionProfileOil, TimeSeriesVolumeDto, CreateTimeSeriesVolumeDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProductionProfileOilDto,
            d => d.ProductionProfileOil != null);
    }

    public async Task<TimeSeriesVolumeDto> UpdateProductionProfileOil(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileOilId,
        UpdateTimeSeriesVolumeDto updatedProductionProfileOilDto)
    {
        return await UpdateDrainageStrategyProfile<ProductionProfileOil, TimeSeriesVolumeDto, UpdateTimeSeriesVolumeDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileOilId,
            updatedProductionProfileOilDto,
            id => Context.ProductionProfileOil.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
