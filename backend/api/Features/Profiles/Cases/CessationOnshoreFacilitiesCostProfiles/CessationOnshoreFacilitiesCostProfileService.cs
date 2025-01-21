using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Cases.CessationOnshoreFacilitiesCostProfiles;

public class CessationOnshoreFacilitiesCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : CaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<TimeSeriesCostDto> CreateCessationOnshoreFacilitiesCostProfile(
        Guid projectId,
        Guid caseId,
        CreateTimeSeriesCostDto createProfileDto)
    {
        return await CreateCaseProfile<CessationOnshoreFacilitiesCostProfile, TimeSeriesCostDto, CreateTimeSeriesCostDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.CessationOnshoreFacilitiesCostProfile != null);
    }

    public async Task<TimeSeriesCostDto> UpdateCessationOnshoreFacilitiesCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTimeSeriesCostDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<CessationOnshoreFacilitiesCostProfile, TimeSeriesCostDto, UpdateTimeSeriesCostDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.CessationOnshoreFacilitiesCostProfile.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
