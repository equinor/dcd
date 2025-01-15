using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.DrainageStrategies.NetSalesGasOverrides.Dtos;
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
    public async Task<NetSalesGasOverrideDto> CreateNetSalesGasOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateNetSalesGasOverrideDto createProfileDto)
    {
        return await CreateDrainageStrategyProfile<NetSalesGasOverride, NetSalesGasOverrideDto, CreateNetSalesGasOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            d => d.NetSalesGasOverride != null);
    }

    public async Task<NetSalesGasOverrideDto> UpdateNetSalesGasOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid profileId,
        UpdateNetSalesGasOverrideDto updateDto)
    {
        return await UpdateDrainageStrategyProfile<NetSalesGasOverride, NetSalesGasOverrideDto, UpdateNetSalesGasOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            profileId,
            updateDto,
            id => Context.NetSalesGasOverride.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
