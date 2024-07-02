using api.Dtos;
using api.Models;

namespace api.Services;

public interface IExplorationTimeSeriesService
{
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
