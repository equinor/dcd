using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.DrainageStrategies.ProductionProfileOils.Dtos;
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
    public async Task<ProductionProfileOilDto> CreateProductionProfileOil(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateProductionProfileOilDto createProductionProfileOilDto)
    {
        return await CreateDrainageStrategyProfile<ProductionProfileOil, ProductionProfileOilDto, CreateProductionProfileOilDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProductionProfileOilDto,
            d => d.ProductionProfileOil != null);
    }

    public async Task<ProductionProfileOilDto> UpdateProductionProfileOil(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileOilId,
        UpdateProductionProfileOilDto updatedProductionProfileOilDto)
    {
        return await UpdateDrainageStrategyProfile<ProductionProfileOil, ProductionProfileOilDto, UpdateProductionProfileOilDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileOilId,
            updatedProductionProfileOilDto,
            id => Context.ProductionProfileOil.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
