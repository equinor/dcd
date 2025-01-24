using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Cases.TotalFeedStudiesOverrides;

public class TotalFeedStudiesOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : CaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<TimeSeriesCostOverrideDto> CreateTotalFeedStudiesOverride(
        Guid projectId,
        Guid caseId,
        CreateTimeSeriesCostOverrideDto createProfileDto)
    {
        return await CreateCaseProfile<TotalFEEDStudiesOverride, TimeSeriesCostOverrideDto, CreateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.TotalFEEDStudiesOverride != null);
    }

    public async Task<TimeSeriesCostOverrideDto> UpdateTotalFeedStudiesOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTimeSeriesCostOverrideDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<TotalFEEDStudiesOverride, TimeSeriesCostOverrideDto, UpdateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.TotalFEEDStudiesOverride.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
