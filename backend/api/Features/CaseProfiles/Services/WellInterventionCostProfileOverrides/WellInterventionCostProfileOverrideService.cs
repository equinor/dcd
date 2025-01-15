using api.Context;
using api.Features.CaseProfiles.Services.WellInterventionCostProfileOverrides.Dtos;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.CaseProfiles.Services.WellInterventionCostProfileOverrides;

public class WellInterventionCostProfileOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : CaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<WellInterventionCostProfileOverrideDto> CreateWellInterventionCostProfileOverride(
        Guid projectId,
        Guid caseId,
        CreateWellInterventionCostProfileOverrideDto createProfileDto)
    {
        return await CreateCaseProfile<WellInterventionCostProfileOverride, WellInterventionCostProfileOverrideDto, CreateWellInterventionCostProfileOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.WellInterventionCostProfileOverride != null);
    }

    public async Task<WellInterventionCostProfileOverrideDto> UpdateWellInterventionCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateWellInterventionCostProfileOverrideDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<WellInterventionCostProfileOverride, WellInterventionCostProfileOverrideDto, UpdateWellInterventionCostProfileOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.WellInterventionCostProfileOverride.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
