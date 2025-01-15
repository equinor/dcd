using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Explorations.CountryOfficeCosts.Dtos;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Profiles.Explorations.CountryOfficeCosts;

public class CountryOfficeCostService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService) : ExplorationProfileBaseService(context, mapperService, projectIntegrityService, recalculationService)
{
    public async Task<CountryOfficeCostDto> CreateCountryOfficeCost(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        CreateCountryOfficeCostDto createProfileDto)
    {
        return await CreateExplorationProfile<CountryOfficeCost, CountryOfficeCostDto, CreateCountryOfficeCostDto>(
            projectId,
            caseId,
            explorationId,
            createProfileDto,
            d => d.CountryOfficeCost != null);
    }

    public async Task<CountryOfficeCostDto> UpdateCountryOfficeCost(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateCountryOfficeCostDto updateDto)
    {
        return await UpdateExplorationCostProfile<CountryOfficeCost, CountryOfficeCostDto, UpdateCountryOfficeCostDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            id => Context.CountryOfficeCost.Include(x => x.Exploration).SingleAsync(x => x.Id == id));
    }
}
