using api.Models;

namespace api.Repositories;

public interface IExplorationRepository
{
    Task<Exploration?> GetExploration(Guid explorationId);
    Task<Exploration> UpdateExploration(Exploration exploration);
    Task<SeismicAcquisitionAndProcessing?> GetSeismicAcquisitionAndProcessing(Guid seismicAcquisitionAndProcessingId);
    Task<SeismicAcquisitionAndProcessing> UpdateSeismicAcquisitionAndProcessing(SeismicAcquisitionAndProcessing seismicAcquisitionAndProcessing);
    Task<CountryOfficeCost?> GetCountryOfficeCost(Guid countryOfficeCostId);
    Task<CountryOfficeCost> UpdateCountryOfficeCost(CountryOfficeCost countryOfficeCost);
}
