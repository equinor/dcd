using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileGases.Dtos;
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
    public async Task<AdditionalProductionProfileGasDto> CreateAdditionalProductionProfileGas(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateAdditionalProductionProfileGasDto createProfileDto)
    {
        return await CreateDrainageStrategyProfile<AdditionalProductionProfileGas, AdditionalProductionProfileGasDto, CreateAdditionalProductionProfileGasDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            d => d.AdditionalProductionProfileGas != null);
    }

    public async Task<AdditionalProductionProfileGasDto> UpdateAdditionalProductionProfileGas(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        UpdateAdditionalProductionProfileGasDto updatedAdditionalProductionProfileGasDto)
    {
        return await UpdateDrainageStrategyProfile<AdditionalProductionProfileGas, AdditionalProductionProfileGasDto, UpdateAdditionalProductionProfileGasDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileId,
            updatedAdditionalProductionProfileGasDto,
            id => Context.AdditionalProductionProfileGas.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id));
    }
}
