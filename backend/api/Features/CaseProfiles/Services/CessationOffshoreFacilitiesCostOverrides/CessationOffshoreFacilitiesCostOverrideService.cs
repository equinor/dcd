using api.Context;
using api.Features.CaseProfiles.Services.CessationOffshoreFacilitiesCostOverrides.Dtos;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.CaseProfiles.Services.CessationOffshoreFacilitiesCostOverrides;

public class CessationOffshoreFacilitiesCostOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : UpdateCaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<CessationOffshoreFacilitiesCostOverrideDto> CreateCessationOffshoreFacilitiesCostOverride(
        Guid projectId,
        Guid caseId,
        CreateCessationOffshoreFacilitiesCostOverrideDto createProfileDto)
    {
        return await CreateCaseProfile<CessationOffshoreFacilitiesCostOverride, CessationOffshoreFacilitiesCostOverrideDto, CreateCessationOffshoreFacilitiesCostOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.CessationOffshoreFacilitiesCostOverride != null);
    }

    public async Task<CessationOffshoreFacilitiesCostOverrideDto> UpdateCessationOffshoreFacilitiesCostOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateCessationOffshoreFacilitiesCostOverrideDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<CessationOffshoreFacilitiesCostOverride, CessationOffshoreFacilitiesCostOverrideDto, UpdateCessationOffshoreFacilitiesCostOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.CessationOffshoreFacilitiesCostOverride.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
