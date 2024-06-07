using api.Context;
using api.Models;


namespace api.Repositories;

public class ExplorationRepository : BaseRepository, IExplorationRepository
{

    public ExplorationRepository(DcdDbContext context) : base(context)
    {
    }

    public async Task<Exploration?> GetExploration(Guid explorationId)
    {
        return await Get<Exploration>(explorationId);
    }

    public Exploration UpdateExploration(Exploration exploration)
    {
        return Update(exploration);
    }

    public async Task<ExplorationWell?> GetExplorationWell(Guid explorationId, Guid wellId)
    {
        return await _context.ExplorationWell.FindAsync(explorationId, wellId);
    }

    public ExplorationWell UpdateExplorationWell(ExplorationWell explorationWell)
    {
        return Update(explorationWell);
    }

    public async Task<SeismicAcquisitionAndProcessing?> GetSeismicAcquisitionAndProcessing(Guid seismicAcquisitionAndProcessingId)
    {
        return await Get<SeismicAcquisitionAndProcessing>(seismicAcquisitionAndProcessingId);
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
