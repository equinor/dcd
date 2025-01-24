using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.DrainageStrategies.NetSalesGasOverrides;

public class NetSalesGasOverrideService(
    DcdDbContext context,
    IConversionMapperService conversionMapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : DrainageStrategyProfileBaseService(context, recalculationService, projectIntegrityService, conversionMapperService)
{
    public async Task<TimeSeriesVolumeOverrideDto> CreateNetSalesGasOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateTimeSeriesVolumeOverrideDto createProfileDto)
    {
        return await CreateDrainageStrategyProfile<NetSalesGasOverride, TimeSeriesVolumeOverrideDto, CreateTimeSeriesVolumeOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            d => d.NetSalesGasOverride != null);
    }

    public async Task<TimeSeriesVolumeOverrideDto> UpdateNetSalesGasOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid profileId,
        UpdateTimeSeriesVolumeOverrideDto updateDto)
    {
        return await UpdateDrainageStrategyProfile<NetSalesGasOverride, TimeSeriesVolumeOverrideDto, UpdateTimeSeriesVolumeOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            profileId,
            updateDto,
            id => Context.NetSalesGasOverride.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
