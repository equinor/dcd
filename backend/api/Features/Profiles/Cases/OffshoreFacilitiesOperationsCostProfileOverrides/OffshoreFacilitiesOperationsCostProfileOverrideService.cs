using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Cases.OffshoreFacilitiesOperationsCostProfileOverrides.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Cases.OffshoreFacilitiesOperationsCostProfileOverrides;

public class OffshoreFacilitiesOperationsCostProfileOverrideService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : CaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
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
