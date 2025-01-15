using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.DrainageStrategies.DeferredOilProductions.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.DrainageStrategies.DeferredOilProductions;

public class DeferredOilProductionService(
    DcdDbContext context,
    IConversionMapperService conversionMapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : DrainageStrategyProfileBaseService(context, recalculationService, projectIntegrityService, conversionMapperService)
{
    public async Task<DeferredOilProductionDto> CreateDeferredOilProduction(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateDeferredOilProductionDto createProfileDto)
    {
        return await CreateDrainageStrategyProfile<DeferredOilProduction, DeferredOilProductionDto, CreateDeferredOilProductionDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            d => d.DeferredOilProduction != null);
    }

    public async Task<DeferredOilProductionDto> UpdateDeferredOilProduction(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        UpdateDeferredOilProductionDto updatedDeferredOilProductionDto)
    {
        return await UpdateDrainageStrategyProfile<DeferredOilProduction, DeferredOilProductionDto, UpdateDeferredOilProductionDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileId,
            updatedDeferredOilProductionDto,
            id => Context.DeferredOilProduction.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
