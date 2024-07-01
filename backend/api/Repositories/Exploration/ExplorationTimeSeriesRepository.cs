using System.Linq.Expressions;

using api.Context;
using api.Enums;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Repositories;

public class ExplorationTimeSeriesRepository : BaseRepository, IExplorationTimeSeriesRepository
{

    public ExplorationTimeSeriesRepository(DcdDbContext context) : base(context)
    {
    }
    public GAndGAdminCostOverride CreateGAndGAdminCostOverride(GAndGAdminCostOverride profile)
    {
        _context.GAndGAdminCostOverride.Add(profile);
        return profile;
    }
    public async Task<GAndGAdminCostOverride?> GetGAndGAdminCostOverride(Guid profileId)
    {
        return await Get<GAndGAdminCostOverride>(profileId);
    }

    public GAndGAdminCostOverride UpdateGAndGAdminCostOverride(GAndGAdminCostOverride costProfile)
    {
        return Update(costProfile);
    }

    public async Task<SeismicAcquisitionAndProcessing?> GetSeismicAcquisitionAndProcessing(Guid seismicAcquisitionAndProcessingId)
    {
        return await Get<SeismicAcquisitionAndProcessing>(seismicAcquisitionAndProcessingId);
    }

    public SeismicAcquisitionAndProcessing CreateSeismicAcquisitionAndProcessing(SeismicAcquisitionAndProcessing profile)
    {
        _context.SeismicAcquisitionAndProcessing.Add(profile);
        return profile;
    }

    public CountryOfficeCost CreateCountryOfficeCost(CountryOfficeCost profile)
    {
        _context.CountryOfficeCost.Add(profile);
        return profile;
    }

    public SeismicAcquisitionAndProcessing UpdateSeismicAcquisitionAndProcessing(SeismicAcquisitionAndProcessing seismicAcquisitionAndProcessing)
    {
        return Update(seismicAcquisitionAndProcessing);
    }

    public async Task<CountryOfficeCost?> GetCountryOfficeCost(Guid countryOfficeCostId)
    {
        return await Get<CountryOfficeCost>(countryOfficeCostId);
    }

    public CountryOfficeCost UpdateCountryOfficeCost(CountryOfficeCost countryOfficeCost)
    {
        return Update(countryOfficeCost);
    }
}
