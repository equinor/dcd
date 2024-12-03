using api.Context;
using api.Features.CaseProfiles.Repositories;
using api.Models;

namespace api.Features.Assets.CaseAssets.Explorations.Repositories;

public class ExplorationTimeSeriesRepository(DcdDbContext context) : BaseRepository(context), IExplorationTimeSeriesRepository
{
    public GAndGAdminCostOverride CreateGAndGAdminCostOverride(GAndGAdminCostOverride profile)
    {
        Context.GAndGAdminCostOverride.Add(profile);
        return profile;
    }
    public async Task<GAndGAdminCostOverride?> GetGAndGAdminCostOverride(Guid profileId)
    {
        return await GetWithIncludes<GAndGAdminCostOverride>(profileId, e => e.Exploration);
    }

    public GAndGAdminCostOverride UpdateGAndGAdminCostOverride(GAndGAdminCostOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<SeismicAcquisitionAndProcessing?> GetSeismicAcquisitionAndProcessing(Guid seismicAcquisitionAndProcessingId)
    {
        return await GetWithIncludes<SeismicAcquisitionAndProcessing>(seismicAcquisitionAndProcessingId, e => e.Exploration);
    }

    public SeismicAcquisitionAndProcessing CreateSeismicAcquisitionAndProcessing(SeismicAcquisitionAndProcessing profile)
    {
        Context.SeismicAcquisitionAndProcessing.Add(profile);
        return profile;
    }

    public CountryOfficeCost CreateCountryOfficeCost(CountryOfficeCost profile)
    {
        Context.CountryOfficeCost.Add(profile);
        return profile;
    }

    public SeismicAcquisitionAndProcessing UpdateSeismicAcquisitionAndProcessing(SeismicAcquisitionAndProcessing seismicAcquisitionAndProcessing)
    {
        return Update(seismicAcquisitionAndProcessing);
    }

    public async Task<CountryOfficeCost?> GetCountryOfficeCost(Guid countryOfficeCostId)
    {
        return await GetWithIncludes<CountryOfficeCost>(countryOfficeCostId, e => e.Exploration);
    }

    public CountryOfficeCost UpdateCountryOfficeCost(CountryOfficeCost countryOfficeCost)
    {
        return Update(countryOfficeCost);
    }
}
