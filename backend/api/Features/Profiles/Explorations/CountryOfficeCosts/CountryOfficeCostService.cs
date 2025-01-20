using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
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
    public async Task<TimeSeriesCostDto> CreateCountryOfficeCost(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        CreateTimeSeriesCostDto createProfileDto)
    {
        return await CreateExplorationProfile<CountryOfficeCost, TimeSeriesCostDto, CreateTimeSeriesCostDto>(
            projectId,
            caseId,
            explorationId,
            createProfileDto,
            d => d.CountryOfficeCost != null);
    }

    public async Task<TimeSeriesCostDto> UpdateCountryOfficeCost(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateTimeSeriesCostDto updateDto)
    {
        return await UpdateExplorationCostProfile<CountryOfficeCost, TimeSeriesCostDto, UpdateTimeSeriesCostDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            id => Context.CountryOfficeCost.Include(x => x.Exploration).SingleAsync(x => x.Id == id));
    }
}
