using api.Context;
using api.Features.CaseProfiles.Services.CessationWellsCostOverrides.Dtos;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.CaseProfiles.Services.CessationWellsCostOverrides;

public class CessationWellsCostOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : UpdateCaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<CessationWellsCostOverrideDto> CreateCessationWellsCostOverride(Guid projectId,
        Guid caseId,
        CreateCessationWellsCostOverrideDto createProfileDto)
    {
        return await CreateCaseProfile<CessationWellsCostOverride, CessationWellsCostOverrideDto, CreateCessationWellsCostOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.CessationWellsCostOverride != null);
    }

    public async Task<CessationWellsCostOverrideDto> UpdateCessationWellsCostOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateCessationWellsCostOverrideDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<CessationWellsCostOverride, CessationWellsCostOverrideDto, UpdateCessationWellsCostOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.CessationWellsCostOverride.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
