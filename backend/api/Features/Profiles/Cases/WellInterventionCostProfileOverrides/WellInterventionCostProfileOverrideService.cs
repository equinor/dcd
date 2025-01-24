using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Cases.WellInterventionCostProfileOverrides;

public class WellInterventionCostProfileOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : CaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<TimeSeriesCostOverrideDto> CreateWellInterventionCostProfileOverride(
        Guid projectId,
        Guid caseId,
        CreateTimeSeriesCostOverrideDto createProfileDto)
    {
        return await CreateCaseProfile<WellInterventionCostProfileOverride, TimeSeriesCostOverrideDto, CreateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.WellInterventionCostProfileOverride != null);
    }

    public async Task<TimeSeriesCostOverrideDto> UpdateWellInterventionCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTimeSeriesCostOverrideDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<WellInterventionCostProfileOverride, TimeSeriesCostOverrideDto, UpdateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.WellInterventionCostProfileOverride.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
