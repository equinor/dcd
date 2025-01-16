using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.DrainageStrategies.DeferredGasProductions.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.DrainageStrategies.DeferredGasProductions;

public class DeferredGasProductionService(
    DcdDbContext context,
    IConversionMapperService conversionMapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : DrainageStrategyProfileBaseService(context, recalculationService, projectIntegrityService, conversionMapperService)
{
    public async Task<DeferredGasProductionDto> CreateDeferredGasProduction(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateDeferredGasProductionDto createProfileDto)
    {
        return await CreateDrainageStrategyProfile<DeferredGasProduction, DeferredGasProductionDto, CreateDeferredGasProductionDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            d => d.DeferredGasProduction != null);
    }

    public async Task<DeferredGasProductionDto> UpdateDeferredGasProduction(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        UpdateDeferredGasProductionDto updatedDeferredGasProductionDto)
    {
        return await UpdateDrainageStrategyProfile<DeferredGasProduction, DeferredGasProductionDto, UpdateDeferredGasProductionDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileId,
            updatedDeferredGasProductionDto,
            id => Context.DeferredGasProduction.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
