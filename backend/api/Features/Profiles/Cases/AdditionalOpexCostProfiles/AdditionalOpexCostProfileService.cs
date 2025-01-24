using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Cases.AdditionalOpexCostProfiles;

public class AdditionalOpexCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : CaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<TimeSeriesCostDto> CreateAdditionalOpexCostProfile(
        Guid projectId,
        Guid caseId,
        CreateTimeSeriesCostDto createProfileDto)
    {
        return await CreateCaseProfile<AdditionalOPEXCostProfile, TimeSeriesCostDto, CreateTimeSeriesCostDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.AdditionalOPEXCostProfile != null);
    }

    public async Task<TimeSeriesCostDto> UpdateAdditionalOpexCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTimeSeriesCostDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<AdditionalOPEXCostProfile, TimeSeriesCostDto, UpdateTimeSeriesCostDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.AdditionalOPEXCostProfile.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
