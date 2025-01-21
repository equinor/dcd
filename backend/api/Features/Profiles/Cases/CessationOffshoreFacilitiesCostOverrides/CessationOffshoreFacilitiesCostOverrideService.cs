using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Cases.CessationOffshoreFacilitiesCostOverrides;

public class CessationOffshoreFacilitiesCostOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : CaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<TimeSeriesCostOverrideDto> CreateCessationOffshoreFacilitiesCostOverride(
        Guid projectId,
        Guid caseId,
        CreateTimeSeriesCostOverrideDto createProfileDto)
    {
        return await CreateCaseProfile<CessationOffshoreFacilitiesCostOverride, TimeSeriesCostOverrideDto, CreateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.CessationOffshoreFacilitiesCostOverride != null);
    }

    public async Task<TimeSeriesCostOverrideDto> UpdateCessationOffshoreFacilitiesCostOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTimeSeriesCostOverrideDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<CessationOffshoreFacilitiesCostOverride, TimeSeriesCostOverrideDto, UpdateTimeSeriesCostOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.CessationOffshoreFacilitiesCostOverride.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
