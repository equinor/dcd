using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.WellProjects.OilProducerCostProfileOverrides.Dtos;
using api.Features.ProjectIntegrity;
using api.Features.TechnicalInput.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.WellProjects.OilProducerCostProfileOverrides;

public class OilProducerCostProfileOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : WellProjectProfileBaseService(context, mapperService, projectIntegrityService, recalculationService)
{
    public async Task<OilProducerCostProfileOverrideDto> CreateOilProducerCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        CreateOilProducerCostProfileOverrideDto createProfileDto)
    {
        return await CreateWellProjectProfile<OilProducerCostProfileOverride, OilProducerCostProfileOverrideDto, CreateOilProducerCostProfileOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            createProfileDto,
            d => d.OilProducerCostProfileOverride != null);
    }

    public async Task<OilProducerCostProfileOverrideDto> UpdateOilProducerCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateOilProducerCostProfileOverrideDto updateDto)
    {
        return await UpdateWellProjectCostProfile<OilProducerCostProfileOverride, OilProducerCostProfileOverrideDto, UpdateOilProducerCostProfileOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            id => Context.OilProducerCostProfileOverride.Include(x => x.WellProject).SingleAsync(x => x.Id == id));
    }
}
