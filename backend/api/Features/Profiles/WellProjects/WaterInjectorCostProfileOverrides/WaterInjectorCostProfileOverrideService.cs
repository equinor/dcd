using api.Context;
using api.Features.Assets.CaseAssets.WellProjects.Dtos;
using api.Features.Assets.CaseAssets.WellProjects.Services;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.WellProjects.WaterInjectorCostProfileOverrides.Dtos;
using api.Features.ProjectIntegrity;
using api.Features.TechnicalInput.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.WellProjects.WaterInjectorCostProfileOverrides;

public class WaterInjectorCostProfileOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : WellProjectProfileBaseService(context, mapperService, projectIntegrityService, recalculationService)
{
    public async Task<WaterInjectorCostProfileOverrideDto> CreateWaterInjectorCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        CreateWaterInjectorCostProfileOverrideDto createProfileDto)
    {
        return await CreateWellProjectProfile<WaterInjectorCostProfileOverride, WaterInjectorCostProfileOverrideDto, CreateWaterInjectorCostProfileOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            createProfileDto,
            d => d.WaterInjectorCostProfileOverride != null);
    }

    public async Task<WaterInjectorCostProfileOverrideDto> UpdateWaterInjectorCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateWaterInjectorCostProfileOverrideDto updateDto)
    {
        return await UpdateWellProjectCostProfile<WaterInjectorCostProfileOverride, WaterInjectorCostProfileOverrideDto, UpdateWaterInjectorCostProfileOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            id => Context.WaterInjectorCostProfileOverride.Include(x => x.WellProject).SingleAsync(x => x.Id == id));
    }
}
