using api.Dtos;
using api.Models;

namespace api.Services;

public interface IExplorationService
{
    Task<ExplorationDto> CopyExploration(Guid explorationId, Guid sourceCaseId);
    Task<Exploration> CreateExploration(Guid projectId, Guid sourceCaseId, CreateExplorationDto explorationDto);
    Task<ExplorationDto> UpdateExplorationAndCostProfiles(ExplorationDto updatedExplorationDto);
    Task<Exploration> GetExploration(Guid explorationId);

    Task<ExplorationDto> UpdateExploration(
        Guid caseId,
        Guid explorationId,
        UpdateExplorationDto updatedExplorationDto
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
}
