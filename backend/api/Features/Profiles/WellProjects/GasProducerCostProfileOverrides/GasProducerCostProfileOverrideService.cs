using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.WellProjects.GasProducerCostProfileOverrides.Dtos;
using api.Features.ProjectIntegrity;
using api.Features.TechnicalInput.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.WellProjects.GasProducerCostProfileOverrides;

public class GasProducerCostProfileOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : WellProjectProfileBaseService(context, mapperService, projectIntegrityService, recalculationService)
{
    public async Task<GasProducerCostProfileOverrideDto> CreateGasProducerCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        CreateGasProducerCostProfileOverrideDto createProfileDto)
    {
        return await CreateWellProjectProfile<GasProducerCostProfileOverride, GasProducerCostProfileOverrideDto, CreateGasProducerCostProfileOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            createProfileDto,
            d => d.GasProducerCostProfileOverride != null);
    }

    public async Task<GasProducerCostProfileOverrideDto> UpdateGasProducerCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateGasProducerCostProfileOverrideDto updateDto)
    {
        return await UpdateWellProjectCostProfile<GasProducerCostProfileOverride, GasProducerCostProfileOverrideDto, UpdateGasProducerCostProfileOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            id => Context.GasProducerCostProfileOverride.Include(x => x.WellProject).SingleAsync(x => x.Id == id));
    }
}
