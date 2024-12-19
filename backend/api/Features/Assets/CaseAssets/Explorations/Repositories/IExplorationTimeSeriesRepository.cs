using api.Models;

namespace api.Features.Assets.CaseAssets.Explorations.Repositories;

public interface IExplorationTimeSeriesRepository
{
    GAndGAdminCostOverride CreateGAndGAdminCostOverride(GAndGAdminCostOverride profile);
    Task<GAndGAdminCostOverride?> GetGAndGAdminCostOverride(Guid profileId);
    GAndGAdminCostOverride UpdateGAndGAdminCostOverride(GAndGAdminCostOverride costprofile);
    Task<SeismicAcquisitionAndProcessing?> GetSeismicAcquisitionAndProcessing(Guid seismicAcquisitionAndProcessingId);
    SeismicAcquisitionAndProcessing UpdateSeismicAcquisitionAndProcessing(SeismicAcquisitionAndProcessing seismicAcquisitionAndProcessing);
    Task<CountryOfficeCost?> GetCountryOfficeCost(Guid countryOfficeCostId);
    CountryOfficeCost UpdateCountryOfficeCost(CountryOfficeCost countryOfficeCost);
    SeismicAcquisitionAndProcessing CreateSeismicAcquisitionAndProcessing(SeismicAcquisitionAndProcessing profile);
    CountryOfficeCost CreateCountryOfficeCost(CountryOfficeCost profile);
}
