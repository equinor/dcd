using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Cases.CessationWellsCostOverrides;

public class CessationWellsCostOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : CaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<TimeSeriesCostOverrideDto> CreateCessationWellsCostOverride(Guid projectId,
        Guid caseId,
        CreateTimeSeriesCostOverrideDto createProfileDto)
    {
        return await CreateCaseProfile<CessationWellsCostOverride, TimeSeriesCostOverrideDto, CreateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.CessationWellsCostOverride != null);
    }

    public async Task<TimeSeriesCostOverrideDto> UpdateCessationWellsCostOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTimeSeriesCostOverrideDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<CessationWellsCostOverride, TimeSeriesCostOverrideDto, UpdateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.CessationWellsCostOverride.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
