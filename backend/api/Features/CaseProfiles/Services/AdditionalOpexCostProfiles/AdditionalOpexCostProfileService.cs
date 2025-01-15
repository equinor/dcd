using api.Context;
using api.Features.CaseProfiles.Services.AdditionalOpexCostProfiles.Dtos;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.CaseProfiles.Services.AdditionalOpexCostProfiles;

public class AdditionalOpexCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : CaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<AdditionalOpexCostProfileDto> CreateAdditionalOpexCostProfile(
        Guid projectId,
        Guid caseId,
        CreateAdditionalOpexCostProfileDto createProfileDto)
    {
        return await CreateCaseProfile<AdditionalOPEXCostProfile, AdditionalOpexCostProfileDto, CreateAdditionalOpexCostProfileDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.AdditionalOPEXCostProfile != null);
    }

    public async Task<AdditionalOpexCostProfileDto> UpdateAdditionalOpexCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateAdditionalOpexCostProfileDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<AdditionalOPEXCostProfile, AdditionalOpexCostProfileDto, UpdateAdditionalOpexCostProfileDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.AdditionalOPEXCostProfile.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
