using api.Context;
using api.Features.CaseProfiles.Services.TotalOtherStudiesCostProfiles.Dtos;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.CaseProfiles.Services.TotalOtherStudiesCostProfiles;

public class TotalOtherStudiesCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : UpdateCaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<TotalOtherStudiesCostProfileDto> CreateTotalOtherStudiesCostProfile(
        Guid projectId,
        Guid caseId,
        CreateTotalOtherStudiesCostProfileDto createProfileDto)
    {
        return await CreateCaseProfile<TotalOtherStudiesCostProfile, TotalOtherStudiesCostProfileDto, CreateTotalOtherStudiesCostProfileDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.TotalOtherStudiesCostProfile != null);
    }

    public async Task<TotalOtherStudiesCostProfileDto> UpdateTotalOtherStudiesCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTotalOtherStudiesCostProfileDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<TotalOtherStudiesCostProfile, TotalOtherStudiesCostProfileDto, UpdateTotalOtherStudiesCostProfileDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.TotalOtherStudiesCostProfile.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
