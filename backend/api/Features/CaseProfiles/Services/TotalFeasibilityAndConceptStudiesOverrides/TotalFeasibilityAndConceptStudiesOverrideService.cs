using api.Context;
using api.Features.CaseProfiles.Services.TotalFeasibilityAndConceptStudiesOverrides.Dtos;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.CaseProfiles.Services.TotalFeasibilityAndConceptStudiesOverrides;

public class TotalFeasibilityAndConceptStudiesOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : UpdateCaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<TotalFeasibilityAndConceptStudiesOverrideDto> CreateTotalFeasibilityAndConceptStudiesOverride(
        Guid projectId,
        Guid caseId,
        CreateTotalFeasibilityAndConceptStudiesOverrideDto createProfileDto)
    {
        return await CreateCaseProfile<TotalFeasibilityAndConceptStudiesOverride, TotalFeasibilityAndConceptStudiesOverrideDto, CreateTotalFeasibilityAndConceptStudiesOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.TotalFeasibilityAndConceptStudiesOverride != null);
    }

    public async Task<TotalFeasibilityAndConceptStudiesOverrideDto> UpdateTotalFeasibilityAndConceptStudiesOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTotalFeasibilityAndConceptStudiesOverrideDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<TotalFeasibilityAndConceptStudiesOverride, TotalFeasibilityAndConceptStudiesOverrideDto, UpdateTotalFeasibilityAndConceptStudiesOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.TotalFeasibilityAndConceptStudiesOverride.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
