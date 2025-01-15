using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.DrainageStrategies.ProductionProfileGases.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.DrainageStrategies.ProductionProfileGases;

public class ProductionProfileGasService(
    DcdDbContext context,
    IConversionMapperService conversionMapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : DrainageStrategyProfileBaseService(context, recalculationService, projectIntegrityService, conversionMapperService)
{
    public async Task<ProductionProfileGasDto> CreateProductionProfileGas(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateProductionProfileGasDto createProfileDto)
    {
        return await CreateDrainageStrategyProfile<ProductionProfileGas, ProductionProfileGasDto, CreateProductionProfileGasDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            d => d.ProductionProfileGas != null);
    }

    public async Task<ProductionProfileGasDto> UpdateProductionProfileGas(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        UpdateProductionProfileGasDto updatedProductionProfileGasDto)
    {
        return await UpdateDrainageStrategyProfile<ProductionProfileGas, ProductionProfileGasDto, UpdateProductionProfileGasDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileId,
            updatedProductionProfileGasDto,
            id => Context.ProductionProfileGas.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
