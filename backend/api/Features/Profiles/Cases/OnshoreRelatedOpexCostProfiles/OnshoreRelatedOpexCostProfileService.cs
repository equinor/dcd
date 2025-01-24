using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Cases.OnshoreRelatedOpexCostProfiles;

public class OnshoreRelatedOpexCostProfileService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : CaseProfileBaseService(context, recalculationService, projectIntegrityService, mapperService)
{
    public async Task<TimeSeriesCostDto> CreateOnshoreRelatedOpexCostProfile(
        Guid projectId,
        Guid caseId,
        CreateTimeSeriesCostDto createProfileDto)
    {
        return await CreateCaseProfile<OnshoreRelatedOPEXCostProfile, TimeSeriesCostDto, CreateTimeSeriesCostDto>(
            projectId,
            caseId,
            createProfileDto,
            d => d.OnshoreRelatedOPEXCostProfile != null);
    }

    public async Task<TimeSeriesCostDto> UpdateOnshoreRelatedOpexCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTimeSeriesCostDto updatedCostProfileDto)
    {
        return await UpdateCaseCostProfile<OnshoreRelatedOPEXCostProfile, TimeSeriesCostDto, UpdateTimeSeriesCostDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            id => Context.OnshoreRelatedOPEXCostProfile.Include(x => x.Case).SingleAsync(x => x.Id == id));
    }
}
