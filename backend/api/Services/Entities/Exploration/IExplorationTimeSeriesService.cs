using api.Dtos;

namespace api.Services;

public interface IExplorationTimeSeriesService
{
    Task<GAndGAdminCostOverrideDto> UpdateGAndGAdminCostOverride(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateGAndGAdminCostOverrideDto updateDto
    );
    Task<SeismicAcquisitionAndProcessingDto> UpdateSeismicAcquisitionAndProcessing(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateSeismicAcquisitionAndProcessingDto updateDto
    );

    Task<CountryOfficeCostDto> UpdateCountryOfficeCost(
        Guid projectId,
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateCountryOfficeCostDto updateDto
    );

    Task<GAndGAdminCostOverrideDto> CreateGAndGAdminCostOverride(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        CreateGAndGAdminCostOverrideDto createProfileDto
    );

    Task<SeismicAcquisitionAndProcessingDto> CreateSeismicAcquisitionAndProcessing(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        CreateSeismicAcquisitionAndProcessingDto createProfileDto
    );

    Task<CountryOfficeCostDto> CreateCountryOfficeCost(
        Guid projectId,
        Guid caseId,
        Guid explorationId,
        CreateCountryOfficeCostDto createProfileDto
    );
}
