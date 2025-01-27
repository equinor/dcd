using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileGases;

public class AdditionalProductionProfileGasService(
    DcdDbContext context,
    IConversionMapperService conversionMapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : DrainageStrategyProfileBaseService(context, recalculationService, projectIntegrityService, conversionMapperService)
{
    public async Task<TimeSeriesCostDto> CreateAdditionalProductionProfileGas(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateTimeSeriesCostDto createProfileDto)
    {
        return await CreateDrainageStrategyProfile<AdditionalProductionProfileGas, TimeSeriesCostDto, CreateTimeSeriesCostDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            d => d.AdditionalProductionProfileGas != null);
    }

    public async Task<TimeSeriesCostDto> UpdateAdditionalProductionProfileGas(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        UpdateTimeSeriesCostDto updatedAdditionalProductionProfileGasDto)
    {
        return await UpdateDrainageStrategyProfile<AdditionalProductionProfileGas, TimeSeriesCostDto, UpdateTimeSeriesCostDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileId,
            updatedAdditionalProductionProfileGasDto,
            id => Context.AdditionalProductionProfileGas.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
