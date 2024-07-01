using api.Dtos;
using api.Models;

namespace api.Services;

public interface IExplorationTimeSeriesService
{
    Task<GAndGAdminCostOverrideDto> UpdateGAndGAdminCostOverride(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateGAndGAdminCostOverrideDto updateDto
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

    Task<GAndGAdminCostOverrideDto> CreateGAndGAdminCostOverride(
        Guid caseId,
        Guid explorationId,
        CreateGAndGAdminCostOverrideDto createProfileDto
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
