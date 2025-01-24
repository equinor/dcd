using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Cases.TotalFeasibilityAndConceptStudiesOverrides;

public class TotalFeasibilityAndConceptStudiesOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : CaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<TimeSeriesCostOverrideDto> CreateTotalFeasibilityAndConceptStudiesOverride(
        Guid projectId,
        Guid caseId,
        CreateTimeSeriesCostOverrideDto createProfileDto)
    {
        return await CreateCaseProfile<TotalFeasibilityAndConceptStudiesOverride, TimeSeriesCostOverrideDto, CreateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.TotalFeasibilityAndConceptStudiesOverride != null);
    }

    public async Task<TimeSeriesCostOverrideDto> UpdateTotalFeasibilityAndConceptStudiesOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTimeSeriesCostOverrideDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<TotalFeasibilityAndConceptStudiesOverride, TimeSeriesCostOverrideDto, UpdateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.TotalFeasibilityAndConceptStudiesOverride.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
