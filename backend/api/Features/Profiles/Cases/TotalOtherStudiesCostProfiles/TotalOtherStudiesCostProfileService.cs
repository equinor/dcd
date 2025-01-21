using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Cases.TotalOtherStudiesCostProfiles;

public class TotalOtherStudiesCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : CaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<TimeSeriesCostDto> CreateTotalOtherStudiesCostProfile(
        Guid projectId,
        Guid caseId,
        CreateTimeSeriesCostDto createProfileDto)
    {
        return await CreateCaseProfile<TotalOtherStudiesCostProfile, TimeSeriesCostDto, CreateTimeSeriesCostDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.TotalOtherStudiesCostProfile != null);
    }

    public async Task<TimeSeriesCostDto> UpdateTotalOtherStudiesCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTimeSeriesCostDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<TotalOtherStudiesCostProfile, TimeSeriesCostDto, UpdateTimeSeriesCostDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.TotalOtherStudiesCostProfile.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
