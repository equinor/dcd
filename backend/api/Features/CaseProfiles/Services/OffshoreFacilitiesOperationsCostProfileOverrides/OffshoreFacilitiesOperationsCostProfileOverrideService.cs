using api.Context;
using api.Features.CaseProfiles.Services.OffshoreFacilitiesOperationsCostProfileOverrides.Dtos;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.CaseProfiles.Services.OffshoreFacilitiesOperationsCostProfileOverrides;

public class OffshoreFacilitiesOperationsCostProfileOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : UpdateCaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<OffshoreFacilitiesOperationsCostProfileOverrideDto> CreateOffshoreFacilitiesOperationsCostProfileOverride(
        Guid projectId,
        Guid caseId,
        CreateOffshoreFacilitiesOperationsCostProfileOverrideDto createProfileDto)
    {
        return await CreateCaseProfile<OffshoreFacilitiesOperationsCostProfileOverride, OffshoreFacilitiesOperationsCostProfileOverrideDto, CreateOffshoreFacilitiesOperationsCostProfileOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.OffshoreFacilitiesOperationsCostProfileOverride != null);
    }

    public async Task<OffshoreFacilitiesOperationsCostProfileOverrideDto> UpdateOffshoreFacilitiesOperationsCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<OffshoreFacilitiesOperationsCostProfileOverride, OffshoreFacilitiesOperationsCostProfileOverrideDto, UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.OffshoreFacilitiesOperationsCostProfileOverride.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
