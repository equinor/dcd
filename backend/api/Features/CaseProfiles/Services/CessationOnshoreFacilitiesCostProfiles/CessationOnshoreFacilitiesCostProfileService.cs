using api.Context;
using api.Features.CaseProfiles.Services.CessationOnshoreFacilitiesCostProfiles.Dtos;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.CaseProfiles.Services.CessationOnshoreFacilitiesCostProfiles;

public class CessationOnshoreFacilitiesCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : UpdateCaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<CessationOnshoreFacilitiesCostProfileDto> CreateCessationOnshoreFacilitiesCostProfile(
        Guid projectId,
        Guid caseId,
        CreateCessationOnshoreFacilitiesCostProfileDto createProfileDto)
    {
        return await CreateCaseProfile<CessationOnshoreFacilitiesCostProfile, CessationOnshoreFacilitiesCostProfileDto, CreateCessationOnshoreFacilitiesCostProfileDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.CessationOnshoreFacilitiesCostProfile != null);
    }

    public async Task<CessationOnshoreFacilitiesCostProfileDto> UpdateCessationOnshoreFacilitiesCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateCessationOnshoreFacilitiesCostProfileDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<CessationOnshoreFacilitiesCostProfile, CessationOnshoreFacilitiesCostProfileDto, UpdateCessationOnshoreFacilitiesCostProfileDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.CessationOnshoreFacilitiesCostProfile.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
