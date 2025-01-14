using api.Context;
using api.Features.CaseProfiles.Services.OnshoreRelatedOpexCostProfiles.Dtos;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.CaseProfiles.Services.OnshoreRelatedOpexCostProfiles;

public class OnshoreRelatedOpexCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : UpdateCaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<OnshoreRelatedOpexCostProfileDto> CreateOnshoreRelatedOpexCostProfile(
        Guid projectId,
        Guid caseId,
        CreateOnshoreRelatedOpexCostProfileDto createProfileDto)
    {
        return await CreateCaseProfile<OnshoreRelatedOPEXCostProfile, OnshoreRelatedOpexCostProfileDto, CreateOnshoreRelatedOpexCostProfileDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.OnshoreRelatedOPEXCostProfile != null);
    }

    public async Task<OnshoreRelatedOpexCostProfileDto> UpdateOnshoreRelatedOpexCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateOnshoreRelatedOpexCostProfileDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<OnshoreRelatedOPEXCostProfile, OnshoreRelatedOpexCostProfileDto, UpdateOnshoreRelatedOpexCostProfileDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.OnshoreRelatedOPEXCostProfile.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
