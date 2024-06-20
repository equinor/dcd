using api.Dtos;
using api.Models;

namespace api.Services;

public interface IExplorationService
{
    Task<ExplorationWithProfilesDto> CopyExploration(Guid explorationId, Guid sourceCaseId);
    Task<Exploration> CreateExploration(Guid projectId, Guid sourceCaseId, CreateExplorationDto explorationDto);
    Task<ExplorationWithProfilesDto> UpdateExplorationAndCostProfiles(ExplorationWithProfilesDto updatedExplorationDto);
    Task<Exploration> GetExploration(Guid explorationId);

    Task<ExplorationDto> UpdateExploration(
        Guid caseId,
        Guid explorationId,
        UpdateExplorationDto updatedExplorationDto
    );

    Task<ExplorationWellDto> UpdateExplorationWell(
        Guid caseId,
        Guid explorationId,
        Guid wellId,
        UpdateExplorationWellDto updatedExplorationWellDto
    );

    Task<SeismicAcquisitionAndProcessingDto> UpdateSeismicAcquisitionAndProcessing(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateSeismicAcquisitionAndProcessingDto updateDto
    );

    Task<CountryOfficeCostDto> UpdateCountryOfficeCost(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateCountryOfficeCostDto updateDto
    );

    Task<SeismicAcquisitionAndProcessingDto> CreateSeismicAcquisitionAndProcessing(
        Guid caseId,
        Guid explorationId,
        CreateSeismicAcquisitionAndProcessingDto createProfileDto
    );

    Task<CountryOfficeCostDto> CreateCountryOfficeCost(
        Guid caseId,
        Guid explorationId,
        CreateCountryOfficeCostDto createProfileDto
    );
}
