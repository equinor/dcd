using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Cases.TotalFeedStudiesOverrides.Dtos;
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
    public async Task<TotalFeedStudiesOverrideDto> CreateTotalFeedStudiesOverride(
        Guid projectId,
        Guid caseId,
        CreateTotalFeedStudiesOverrideDto createProfileDto)
    {
        return await CreateCaseProfile<TotalFEEDStudiesOverride, TotalFeedStudiesOverrideDto, CreateTotalFeedStudiesOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.TotalFEEDStudiesOverride != null);
    }

    public async Task<TotalFeedStudiesOverrideDto> UpdateTotalFeedStudiesOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTotalFeedStudiesOverrideDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<TotalFEEDStudiesOverride, TotalFeedStudiesOverrideDto, UpdateTotalFeedStudiesOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.TotalFEEDStudiesOverride.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
