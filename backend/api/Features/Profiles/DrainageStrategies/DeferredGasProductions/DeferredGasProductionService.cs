using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
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
    public async Task<TimeSeriesVolumeDto> CreateDeferredGasProduction(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateTimeSeriesVolumeDto createProfileDto)
    {
        return await CreateDrainageStrategyProfile<DeferredGasProduction, TimeSeriesVolumeDto, CreateTimeSeriesVolumeDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            d => d.DeferredGasProduction != null);
    }

    public async Task<TimeSeriesVolumeDto> UpdateDeferredGasProduction(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        UpdateTimeSeriesVolumeDto updatedDeferredGasProductionDto)
    {
        return await UpdateDrainageStrategyProfile<DeferredGasProduction, TimeSeriesVolumeDto, UpdateTimeSeriesVolumeDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileId,
            updatedDeferredGasProductionDto,
            id => Context.DeferredGasProduction.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
