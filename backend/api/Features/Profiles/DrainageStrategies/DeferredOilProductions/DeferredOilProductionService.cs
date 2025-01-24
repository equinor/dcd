using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
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
    public async Task<TimeSeriesVolumeDto> CreateDeferredOilProduction(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateTimeSeriesVolumeDto createProfileDto)
    {
        return await CreateDrainageStrategyProfile<DeferredOilProduction, TimeSeriesVolumeDto, CreateTimeSeriesVolumeDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            d => d.DeferredOilProduction != null);
    }

    public async Task<TimeSeriesVolumeDto> UpdateDeferredOilProduction(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        UpdateTimeSeriesVolumeDto updatedDeferredOilProductionDto)
    {
        return await UpdateDrainageStrategyProfile<DeferredOilProduction, TimeSeriesVolumeDto, UpdateTimeSeriesVolumeDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileId,
            updatedDeferredOilProductionDto,
            id => Context.DeferredOilProduction.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
