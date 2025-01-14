using System.Linq.Expressions;

using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Assets.CaseAssets.Explorations.Profiles.Dtos;
using api.Features.Assets.CaseAssets.Explorations.Profiles.Dtos.Create;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.Features.TechnicalInput.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Explorations.Profiles.Services;

public class ExplorationTimeSeriesService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<GAndGAdminCostOverrideDto> CreateGAndGAdminCostOverride(
        Guid projectId,
            Guid caseId,
            Guid explorationId,
            CreateGAndGAdminCostOverrideDto createProfileDto
        )
    {
        return await CreateExplorationProfile<GAndGAdminCostOverride, GAndGAdminCostOverrideDto, CreateGAndGAdminCostOverrideDto>(
            projectId,
            caseId,
            explorationId,
            createProfileDto,
            ExplorationProfileNames.GAndGAdminCostOverride
        );
    }
    public async Task<GAndGAdminCostOverrideDto> UpdateGAndGAdminCostOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateGAndGAdminCostOverrideDto updateDto
    )
    {
        return await UpdateExplorationCostProfile<GAndGAdminCostOverride, GAndGAdminCostOverrideDto, UpdateGAndGAdminCostOverrideDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            id => context.GAndGAdminCostOverride.Include(x => x.Exploration).SingleAsync(x => x.Id == id)
        );
    }
    public async Task<SeismicAcquisitionAndProcessingDto> UpdateSeismicAcquisitionAndProcessing(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateSeismicAcquisitionAndProcessingDto updateDto
    )
    {
        return await UpdateExplorationCostProfile<SeismicAcquisitionAndProcessing, SeismicAcquisitionAndProcessingDto, UpdateSeismicAcquisitionAndProcessingDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            id => context.SeismicAcquisitionAndProcessing.Include(x => x.Exploration).SingleAsync(x => x.Id == id)
        );
    }

    public async Task<CountryOfficeCostDto> UpdateCountryOfficeCost(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateCountryOfficeCostDto updateDto
    )
    {
        return await UpdateExplorationCostProfile<CountryOfficeCost, CountryOfficeCostDto, UpdateCountryOfficeCostDto>(
            projectId,
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            id => context.CountryOfficeCost.Include(x => x.Exploration).SingleAsync(x => x.Id == id)
        );
    }

    public async Task<SeismicAcquisitionAndProcessingDto> CreateSeismicAcquisitionAndProcessing(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        CreateSeismicAcquisitionAndProcessingDto createProfileDto
    )
    {
        return await CreateExplorationProfile<SeismicAcquisitionAndProcessing, SeismicAcquisitionAndProcessingDto, CreateSeismicAcquisitionAndProcessingDto>(
            projectId,
            caseId,
            explorationId,
            createProfileDto,
            ExplorationProfileNames.SeismicAcquisitionAndProcessing
        );
    }

    public async Task<CountryOfficeCostDto> CreateCountryOfficeCost(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        CreateCountryOfficeCostDto createProfileDto
    )
    {
        return await CreateExplorationProfile<CountryOfficeCost, CountryOfficeCostDto, CreateCountryOfficeCostDto>(
            projectId,
            caseId,
            explorationId,
            createProfileDto,
            ExplorationProfileNames.CountryOfficeCost
        );
    }

    private async Task<TDto> UpdateExplorationCostProfile<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile>> getProfile
    )
        where TProfile : class, IExplorationTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId);

        await projectIntegrityService.EntityIsConnectedToProject<Exploration>(projectId, existingProfile.Exploration.Id);

        mapperService.MapToEntity(updatedProfileDto, existingProfile, explorationId);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(existingProfile, profileId);
    }

    private async Task<TDto> CreateExplorationProfile<TProfile, TDto, TCreateDto>(
            Guid projectId,
            Guid caseId,
            Guid explorationId,
            TCreateDto createExplorationProfileDto,
            ExplorationProfileNames profileName
        )
            where TProfile : class, IExplorationTimeSeries, new()
            where TDto : class
            where TCreateDto : class
    {
        await projectIntegrityService.EntityIsConnectedToProject<Exploration>(projectId, explorationId);

        var exploration = await context.Explorations.SingleAsync(x => x.Id == explorationId);

        var resourceHasProfile = await ExplorationHasProfile(explorationId, profileName);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Exploration with id {explorationId} already has a profile of type {typeof(TProfile).Name}.");
        }

        TProfile profile = new()
        {
            Exploration = exploration
        };

        var newProfile = mapperService.MapToEntity(createExplorationProfileDto, profile, explorationId);

        context.Add(newProfile);
        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(newProfile, newProfile.Id);
    }

    private async Task<bool> ExplorationHasProfile(Guid explorationId, ExplorationProfileNames profileType)
    {
        Expression<Func<Exploration, bool>> profileExistsExpression = profileType switch
        {
            ExplorationProfileNames.GAndGAdminCostOverride => d => d.GAndGAdminCostOverride != null,
            ExplorationProfileNames.SeismicAcquisitionAndProcessing => d => d.SeismicAcquisitionAndProcessing != null,
            ExplorationProfileNames.CountryOfficeCost => d => d.CountryOfficeCost != null,
        };

        return await context.Explorations
            .Where(d => d.Id == explorationId)
            .AnyAsync(profileExistsExpression);
    }
}
