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
    public async Task<TimeSeriesCostDto> CreateProductionProfileOil(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateTimeSeriesCostDto createProductionProfileOilDto)
    {
        return await CreateDrainageStrategyProfile<ProductionProfileOil, TimeSeriesCostDto, CreateTimeSeriesCostDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProductionProfileOilDto,
            d => d.ProductionProfileOil != null);
    }

    public async Task<TimeSeriesCostDto> UpdateProductionProfileOil(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileOilId,
        UpdateTimeSeriesCostDto updatedProductionProfileOilDto)
    {
        return await UpdateDrainageStrategyProfile<ProductionProfileOil, TimeSeriesCostDto, UpdateTimeSeriesCostDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileOilId,
            updatedProductionProfileOilDto,
            id => Context.ProductionProfileOil.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
