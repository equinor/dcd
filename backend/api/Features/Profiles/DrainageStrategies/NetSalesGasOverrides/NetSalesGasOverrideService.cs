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
    public async Task<TimeSeriesCostOverrideDto> CreateNetSalesGasOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateTimeSeriesCostOverrideDto createProfileDto)
    {
        return await CreateDrainageStrategyProfile<NetSalesGasOverride, TimeSeriesCostOverrideDto, CreateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            d => d.NetSalesGasOverride != null);
    }

    public async Task<TimeSeriesCostOverrideDto> UpdateNetSalesGasOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid profileId,
        UpdateTimeSeriesCostOverrideDto updateDto)
    {
        return await UpdateDrainageStrategyProfile<NetSalesGasOverride, TimeSeriesCostOverrideDto, UpdateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            profileId,
            updateDto,
            id => Context.NetSalesGasOverride.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
